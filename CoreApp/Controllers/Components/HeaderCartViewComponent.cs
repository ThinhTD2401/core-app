using CoreApp.Models;
using CoreApp.Utilities.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreApp.Controllers.Components
{
    public class HeaderCartViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            Task<List<ShoppingCartViewModel>> task = new Task<List<ShoppingCartViewModel>>(Excute);
            task.Start();
            var cart = await task;
            return View(cart);
        }

        public List<ShoppingCartViewModel> Excute()
        {
            string session = HttpContext.Session.GetString(CommonConstants.CartSession);
            List<ShoppingCartViewModel> cart = new List<ShoppingCartViewModel>();
            if (session != null)
            {
                cart = JsonConvert.DeserializeObject<List<ShoppingCartViewModel>>(session);
            }
            return cart;
        }
    }
}