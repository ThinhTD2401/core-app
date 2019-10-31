using CoreApp.Application.Interfaces;
using CoreApp.Models.CommonViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace CoreApp.Controllers.Components
{
    public class BlogViewComponent : ViewComponent
    {
        private readonly IConfiguration _configuration;
        private readonly IBlogService _blogService;

        public BlogViewComponent(IConfiguration configuration, IBlogService blogService)
        {
            _configuration = configuration;
            _blogService = blogService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string keyword, int? pageSize, string sortBy, string sortOrder, int pageIndex = 1)
        {
            Task<BlogComponentViewModel> task = Task.Factory.StartNew<BlogComponentViewModel>(() => {
                return Excute(keyword, pageSize, sortOrder, sortBy, pageIndex); ;
            });
            var result = await task;
            return View(await task);

        }

        private BlogComponentViewModel Excute(string keyword, int? pageSize, string sortOrder, string sortBy, int pageIndex = 1)
        {
            BlogComponentViewModel blogComponentViewModel = new BlogComponentViewModel();
           // ViewData["BodyClass"] = "shop_grid_full_width_page";
            var isNotPageSize = pageSize == null;
            if (isNotPageSize)
            {
                pageSize = _configuration.GetValue<int>("PageSize");
            }
            blogComponentViewModel.PageSize = pageSize;
            blogComponentViewModel.SortType = sortBy;
            blogComponentViewModel.Data = _blogService.GetAllPaging(keyword, pageIndex, pageSize.Value, sortBy, sortOrder);
            blogComponentViewModel.Keyword = keyword;
            return blogComponentViewModel;
        }

    }
}
