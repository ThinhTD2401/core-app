using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoreApp.Models.BlogViewModels;
using CoreApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CoreApp.Controllers
{
    public class BlogController : Controller
    {
        private IBlogService _blogService;
        private IBillService _billService;

        public BlogController(IBlogService blogService, IBillService billService)
        {
            _blogService = blogService;
            _billService = billService;
        }

        [Route("blogs.html")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("{alias}-blog.{id}.html", Name = "BlogDetail")]
        public IActionResult Details(int id)
        {
            ViewData["BodyClass"] = "product-page";
            var model = new DetailViewModel();
            model.Blog = _blogService.GetById(id);
            model.Tags = _blogService.GetListTagById(id);
            return View(model);
        }
    }
}