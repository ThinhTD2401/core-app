using CoreApp.Application.Interfaces;
using CoreApp.Extensions;
using CoreApp.Models.CommonViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace CoreApp.Controllers.Components
{
    public class SearchViewComponent : ViewComponent
    {
        private readonly IConfiguration _configuration;
        private readonly IProductService _productService;

        public SearchViewComponent(IConfiguration configuration, IProductService productService)
        {
            _configuration = configuration;
            _productService = productService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string keyword, int? pageSize, string sortBy, string sortOrder, int pageIndex = 1)
        {
            Task<SearchResultViewModel> task = Task.Factory.StartNew<SearchResultViewModel>(() => {
                return Excute(keyword, pageSize, sortOrder, sortBy, pageIndex); ;
            });
            return View(await task);

        }

        private SearchResultViewModel Excute(string keyword, int? pageSize, string sortOrder, string sortBy, int pageIndex = 1)
        {
            SearchResultViewModel catalog = new SearchResultViewModel();
            ViewData["BodyClass"] = "shop_grid_full_width_page";
            ViewData["isHomePage"] = HttpContext.Session.Get<bool>("isHomePage");
            ViewData["keywordSearch"] = keyword;
            var isNotPageSize = pageSize == null;
            if (isNotPageSize)
            {
                pageSize = _configuration.GetValue<int>("PageSize");
            }
            catalog.PageSize = pageSize;
            catalog.SortType = sortBy;
            catalog.Data = _productService.GetAllPaging(null, keyword, pageIndex, pageSize.Value, sortBy, sortOrder);
            catalog.Keyword = keyword;

            return catalog;
        }
    }
}