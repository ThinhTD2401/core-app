using CoreApp.Application.Interfaces;
using CoreApp.Application.ViewModels.Product;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CoreApp.Controllers.Components
{
    
    public class CategoryMenuViewComponent : ViewComponent
    {
        private IProductCategoryService _productCategoryService;
        public CategoryMenuViewComponent(IProductCategoryService productCategoryService)
        {
            _productCategoryService = productCategoryService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            Task<List<ProductCategoryViewModel>> task = new Task<List<ProductCategoryViewModel>>(Excute);
            task.Start();
            return View(await task);
        }

        private List<ProductCategoryViewModel> Excute()
        {
            try
            {
                if ((bool)ViewData["isHomePage"])
                {
                    ViewData["toogleMenu"] = "display: block";
                }
            }
            catch (System.Exception)
            {
                ViewData["toogleMenu"] = "display: none";
            }
            return _productCategoryService.GetAll();
        }
    }
}