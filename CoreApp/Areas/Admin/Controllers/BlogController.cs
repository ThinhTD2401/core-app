using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreApp.Application.Interfaces;
using CoreApp.Application.ViewModels.Blog;
using CoreApp.Authorization;
using CoreApp.Utilities.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CoreApp.Areas.Admin.Controllers
{
    public class BlogController : BaseController
    {
        private IAuthorizationService _authorizationService;
        private IBlogService _blogService;
        private IHostingEnvironment _hostingEnvironment;
        public BlogController(IBlogService blogService,
            IAuthorizationService authorizationService, IHostingEnvironment hostingEnvironment)
        {
            _blogService = blogService;
            _authorizationService = authorizationService;
            _hostingEnvironment = hostingEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            var authorize = await _authorizationService.AuthorizeAsync(User, "BLOG", Operation.Read);
            if (!authorize.Succeeded)
            {
                return new RedirectResult("/Admin/Login/Blog");
            }
            return View();
        }

        [HttpGet]
        public IActionResult GetAllPaging(int? categoryId, string keyword, int pageIndex, int pageSize)
        {
            var model = _blogService.GetAllPaging(keyword, pageSize, pageIndex);
            return new OkObjectResult(model);
        }

        [HttpPost]
        public IActionResult SaveEntity(BlogViewModel blogVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                if(blogVm != null && !string.IsNullOrEmpty(blogVm.SeoAlias))
                {
                    blogVm.SeoAlias = TextHelper.ToUnsignString(blogVm.SeoAlias);
                }

                if (blogVm.Id == 0)
                {
                    _blogService.Add(blogVm);
                }
                else
                {
                    _blogService.Update(blogVm);
                }
                _blogService.Save();
                return new OkObjectResult(blogVm);
            }
        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            var model = _blogService.GetById(id);
            return new OkObjectResult(model);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            else
            {
                _blogService.Delete(id);
                _blogService.Save();

                return new OkObjectResult(id);
            }
        }
    }
}