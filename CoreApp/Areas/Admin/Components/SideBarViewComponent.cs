using CoreApp.Application.Interfaces;
using CoreApp.Application.ViewModels.System;
using CoreApp.Extensions;
using CoreApp.Utilities.Constants;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CoreApp.Areas.Admin.Components
{
    public class SideBarViewComponent : ViewComponent
    {
        private IFunctionService _functionService;

        public SideBarViewComponent(IFunctionService functionService)
        {
            _functionService = functionService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            string roles = ((ClaimsPrincipal)User).GetSpecificClaim(CommonConstants.UserClaims.Roles);
            List<FunctionViewModel> functions;
            if (roles.Split(CommonConstants.SepRoles).Contains(CommonConstants.AppRole.AdminRole))
            {
                functions = await _functionService.GetAll(string.Empty);
            }
            else
            {
                //TODO: Get by permission
                functions = await _functionService.GetsByRole(roles);
            }
            return View(functions);
        }
    }
}