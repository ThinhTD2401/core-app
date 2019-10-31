using CoreApp.Application.Interfaces;
using CoreApp.Models.CommonViewModels;
using CoreApp.Utilities.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using CoreApp.Extensions;

namespace CoreApp.Controllers
{
    public class SearchController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IProductService _productService;

        public SearchController(IConfiguration configuration, IProductService productService)
        {
            _configuration = configuration;
            _productService = productService;
        }

        [Route("tim-kiem.html")]
        public IActionResult Search(string keyword, int? pageSize, string sortBy, string sortOrder, int pageIndex = 1)
        {
            SearchResultViewModel catalog = new SearchResultViewModel();
            ViewData["BodyClass"] = "shop_grid_full_width_page";
            HttpContext.Session.Set<bool>("isHomePage", false);
            catalog.PageSize = CommonConstants.PagerDefault.PageSize;
            catalog.SortOrder = CommonConstants.PagerDefault.SortOrder;
            catalog.Keyword = keyword;
            catalog.SortBy = CommonConstants.PagerDefault.SortBy;
            catalog.PageIndex = CommonConstants.PagerDefault.PageIndex;

            return View(catalog);
        }
    }
}