using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CoreApp.Controllers
{
    public class AjaxContentController : Controller
    {
        public IActionResult HeaderCart()
        {
            return ViewComponent("HeaderCart");
        }

        public IActionResult SearchComponent(string keyword, int? pageSize, string sortBy, string sortOrder, int pageIndex = 1)
        {
            return ViewComponent("Search", new { keyword, pageSize, sortBy, sortOrder, pageIndex });
        }
    }
}