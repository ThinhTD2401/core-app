using CoreApp.Application.ViewModels.Product;
using CoreApp.Utilities.Dtos;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace CoreApp.Models.ProductViewModels
{
    public class CatalogViewModel
    {
        public PageResult<ProductViewModel> Data { get; set; }

        public ProductCategoryViewModel Category { set; get; }

        public string SortType { set; get; }

        public int? PageSize { set; get; }

        public List<SelectListItem> SortBies { get; } = new List<SelectListItem>
        {
            new SelectListItem(){Value = "Price",Text = "Giá"},
            new SelectListItem(){Value = "DateCreated",Text = "Sản phẩm mới nhất"},
            new SelectListItem(){Value = "Name",Text = "Tên sản phẩm"},
        };

        public List<SelectListItem> PageSizes { get; } = new List<SelectListItem>
        {
            new SelectListItem(){Value = "12",Text = "12"},
            new SelectListItem(){Value = "24",Text = "24"},
            new SelectListItem(){Value = "48",Text = "48"},
        };

        public List<SelectListItem> SortOrders { get; } = new List<SelectListItem>
        {
            new SelectListItem(){Value = "DESC",Text = "Giảm dần"},
            new SelectListItem(){Value = "ASC",Text = "Tăng dần"}
        };
    }
}