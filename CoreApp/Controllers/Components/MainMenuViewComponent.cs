using CoreApp.Application.Interfaces;
using CoreApp.Application.ViewModels.Product;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CoreApp.Controllers.Components
{
    public class MainMenuViewComponent : ViewComponent
    {
        private IProductCategoryService _productCategoryService;

        public MainMenuViewComponent(IProductCategoryService productCategoryService)
        {
            _productCategoryService = productCategoryService;
        }

        // GET: /<controller>/
        public async Task<IViewComponentResult> InvokeAsync()
        {
            Task<List<ProductCategoryViewModel>> task = new Task<List<ProductCategoryViewModel>>(_productCategoryService.GetAll);
            task.Start();
            return View(await task);
        }
    }
}