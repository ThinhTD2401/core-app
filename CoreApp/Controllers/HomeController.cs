using CoreApp.Application.Interfaces;
using CoreApp.Extensions;
using CoreApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CoreApp.Controllers
{
    public class HomeController : Controller
    {
        private IProductService _productService;
        private IProductCategoryService _productCategoryService;

        private IBlogService _blogService;
        private ICommonService _commonService;

        public HomeController(IProductService productService,
        IBlogService blogService, ICommonService commonService,
       IProductCategoryService productCategoryService)
        {
            _blogService = blogService;
            _commonService = commonService;
            _productService = productService;
            _productCategoryService = productCategoryService;
        }
        [Route("~/")] //Specifies that this is the default action for the entire application. Route: /
        //[Route("")] //Specifies that this is the default action for this route prefix. Route: /Users
        [Route("trang-chu.html", Name ="trang-chu")]
        public IActionResult Index()
        {
            ViewData["BodyClass"] = "cms-index-index cms-home-page";
            ViewData["isHomePage"] = true;
            HttpContext.Session.Set<bool>("isHomePage", true);
            HomeViewModel homeVm = new HomeViewModel
            {
                HomeCategories = _productCategoryService.GetHomeCategories(5),
                HotProducts = _productService.GetHotProduct(5),
                TopSellProducts = _productService.GetLastest(5),
                LastestBlogs = _blogService.GetLastest(5),
                HomeSlides = _commonService.GetSlides("top")
            };
            return View(homeVm);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}