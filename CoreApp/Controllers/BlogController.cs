
using CoreApp.Application.Interfaces;
using CoreApp.Models.BlogViewModels;
using Microsoft.AspNetCore.Mvc;

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

        [Route("{alias}-bai-viet.{id}.html", Name = "BlogDetail")]
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