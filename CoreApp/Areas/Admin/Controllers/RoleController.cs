using CoreApp.Application.Interfaces;
using CoreApp.Application.ViewModels;
using CoreApp.Application.ViewModels.System;
using CoreApp.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreApp.Areas.Admin.Controllers
{
    public class RoleController : BaseController
    {
        private readonly IRoleService _roleService;
        private readonly IAuthorizationService _authorizationService;

        public RoleController(IRoleService roleService, IAuthorizationService authorizationService)
        {
            _roleService = roleService;
            _authorizationService = authorizationService;
        }

        public async Task<IActionResult> Index()
        {
            var authorize = await _authorizationService.AuthorizeAsync(User, "ROLE", Operation.Read);
            if (!authorize.Succeeded)
            {
                return new RedirectResult("/Admin/Login/Index");
            }
            return View();
        }

        public async Task<IActionResult> GetAll()
        {
            List<AppRoleViewModel> model = await _roleService.GetAllAsync();

            return new OkObjectResult(model);
        }

        [HttpGet]
        public async Task<ActionResult> GetById(string id)
        {
            var model = await _roleService.GetById(id);
            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetAllPaging(string keyword, int page, int pageSize)
        {
            Utilities.Dtos.PageResult<AppRoleViewModel> model = _roleService.GetAllPagingAsync(keyword, page, pageSize);
            return new OkObjectResult(model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveEntity(AppRoleViewModel roleVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                if (roleVm.Id == null)
                {
                    await _roleService.AddAsync(roleVm);
                }
                else
                {
                    await _roleService.UpdateAsync(roleVm);
                }
                return new OkObjectResult(roleVm);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {   
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            else
            {
                await _roleService.DeleteAsync(id);

                return new OkObjectResult(id);
            }
        }

        [HttpPost]
        public IActionResult ListAllFunction(Guid roleId)
        {
            var functions = _roleService.GetListFunctionWithRole(roleId);
            return new OkObjectResult(functions);
        }

        [HttpPost]
        public IActionResult SavePermission(List<PermissionViewModel> listPermmission, Guid roleId)
        {
            _roleService.SavePermission(listPermmission, roleId);
            return new OkResult();
        }

    }
}