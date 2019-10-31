
using CoreApp.Application.Interfaces;
using CoreApp.Application.ViewModels.Product;
using CoreApp.Data.Enums;
using CoreApp.Extensions;
using CoreApp.Models;
using CoreApp.Services;
using CoreApp.Utilities.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PaulMiami.AspNetCore.Mvc.Recaptcha;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreApp.Controllers
{
    public class CartController : Controller
    {
        IProductService _productService;
        IBillService _billService;
        IEmailSender _emailSender;
        IViewRenderService _viewRenderService;
        ILogger _logger;
        readonly IConfiguration _configuration;

        public CartController(IProductService productService, IBillService billService, IEmailSender emailSender, IViewRenderService viewRenderService, ILogger<CartController> logger, IConfiguration configuration)
        {
            _productService = productService;
            _billService = billService;
            _emailSender = emailSender;
            _viewRenderService = viewRenderService;
            _logger = logger;
            _configuration = configuration;
        }

        [Route("gio-hang.html", Name = "gio-hang")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("thanh-toan.html", Name = "thanh-toan")]
        public IActionResult Checkout()
        {
            var model = new CheckoutViewModel();
            model.PaymentMethod = PaymentMethod.CashOnDelivery;
            var session = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.CartSession);
            if(session == null)
            {
                return Redirect("/trang-chu.html");
            }
            model.Carts = session;
            return View(model);
        }
        [Route("thanh-toan.html", Name = "thanh-toan")]
        [ValidateAntiForgeryToken]
        //[ValidateRecaptcha]
        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutViewModel model)
        {
            var session = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.CartSession);

            if (ModelState.IsValid)
            {
                if (session != null)
                {
                    var details = new List<BillDetailViewModel>();
                    foreach (var item in session)
                    {
                        details.Add(new BillDetailViewModel()
                        {
                            Product = item.Product,
                            Price = item.Price,
                            ColorId = item.Color.Id,
                            SizeId = item.Size.Id,
                            Quantity = item.Quantity,
                            ProductId = item.Product.Id
                        });
                    }

                    Guid? guidUserId = (Guid?)null;
                    var userId = User.GetSpecificClaim(CommonConstants.UserId);
                    guidUserId = string.IsNullOrEmpty(userId) ? (Guid?)null : Guid.Parse(userId);

                    var billViewModel = new BillViewModel()
                    {
                        CustomerMobile = model.CustomerMobile,
                        BillStatus = BillStatus.New,
                        CustomerAddress = model.CustomerAddress,
                        CustomerName = model.CustomerName,
                        CustomerMessage = model.CustomerMessage,
                        BillDetails = details,
                        DateCreated = DateTime.Now,
                        CustomerId = guidUserId
                    };
                   
                    _billService.Create(billViewModel);
                    try
                    {

                        _billService.Save();
                        var content = await _viewRenderService.RenderToStringAsync("~/Views/Cart/_BillMail.cshtml", billViewModel);
                        //Send mail
                        await _emailSender.SendEmailAsync(_configuration["MailSettings:AdminMail"], "Đơn hàng mới", content);
                        ViewData["Success"] = true;
                        model.Carts = session;
                        HttpContext.Session.Remove(CommonConstants.CartSession);
                        ViewComponent("HeaderCart");
                    }
                    catch (Exception ex)
                    {
                       ViewData["Success"] = false;
                       ModelState.AddModelError("", ex.Message);
                        _logger.LogError("Send Mail" + ex.ToString());
                    }
                }
            }
            return View(model);
        }
        [Route("billemail.cshtml", Name ="billmail")]
        public ActionResult BillEmail()
        {
            var session = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.CartSession);
            var details = new List<BillDetailViewModel>();
            foreach (var item in session)
            {
                details.Add(new BillDetailViewModel()
                {
                    Product = item.Product,
                    Price = item.Price,
                    ColorId = item.Color.Id,
                    SizeId = item.Size.Id,
                    Quantity = item.Quantity,
                    ProductId = item.Product.Id
                });
            }
            var billViewModel = new BillViewModel()
            {
                CustomerMobile = "123123214124",
                BillStatus = BillStatus.New,
                CustomerAddress = "Số 178, ngách 119, Cầu Giấy Hà Nội",
                CustomerName = "Trịnh Đức Thịnh",
                CustomerMessage = "Giao hàng trong giờ hành chính",
                BillDetails = details,
                DateCreated = DateTime.Now
            };
            if (User.Identity.IsAuthenticated == true)
            {
                billViewModel.CustomerId = Guid.Parse(User.GetSpecificClaim("UserId"));
            }
            return View("_BillMail", billViewModel);
        }
        #region AJAX Request
        /// <summary>
        /// Get list item
        /// </summary>
        /// <returns></returns>
        public IActionResult GetCart()
        {
            var session = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.CartSession);
            if (session == null)
                session = new List<ShoppingCartViewModel>();
            return new OkObjectResult(session);
        }

        /// <summary>
        /// Remove all products in cart
        /// </summary>
        /// <returns></returns>
        public IActionResult ClearCart()
        {
            HttpContext.Session.Remove(CommonConstants.CartSession);
            return new OkObjectResult("OK");
        }

        /// <summary>
        /// Add product to cart
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity, int color, int size)
        {
            //Get product detail
            var product = _productService.GetById(productId);

            //Get session with item list from cart
            var session = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.CartSession);
            if (session != null)
            {
                //Convert string to list object
                bool hasChanged = false;

                //Check exist with item product id
                if (session.Any(x => x.Product.Id == productId))
                {
                    foreach (var item in session)
                    {
                        //Update quantity for product if match product id
                        if (item.Product.Id == productId)
                        {
                            item.Quantity += quantity;
                            item.Price = product.PromotionPrice ?? product.Price;
                            hasChanged = true;
                        }
                    }
                }
                else
                {
                    session.Add(new ShoppingCartViewModel()
                    {
                        Product = product,
                        Quantity = quantity,
                        Color = color == 0 ? _billService.GetColor(1) : _billService.GetColor(color),
                        Size = size  == 0 ? _billService.GetSize(1) : _billService.GetSize(size),
                        Price = product.PromotionPrice ?? product.Price
                    });
                    hasChanged = true;
                }

                //Update back to cart
                if (hasChanged)
                {
                    HttpContext.Session.Set(CommonConstants.CartSession, session);
                }
            }
            else
            {
                //Add new cart
                var cart = new List<ShoppingCartViewModel>();
                cart.Add(new ShoppingCartViewModel()
                {
                    Product = product,
                    Quantity = quantity,
                    Color = color == 0 ? _billService.GetColor(1) : _billService.GetColor(color),
                    Size = size == 0 ? _billService.GetSize(1) : _billService.GetSize(size),
                    Price = product.PromotionPrice ?? product.Price
                });
                HttpContext.Session.Set(CommonConstants.CartSession, cart);
            }
            return new OkObjectResult(productId);
        }

        /// <summary>
        /// Remove a product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public IActionResult RemoveFromCart(int productId)
        {
            var session = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.CartSession);
            if (session != null)
            {
                bool hasChanged = false;
                foreach (var item in session)
                {
                    if (item.Product.Id == productId)
                    {
                        session.Remove(item);
                        hasChanged = true;
                        break;
                    }
                }
                if (hasChanged)
                {
                    HttpContext.Session.Set(CommonConstants.CartSession, session);
                }
                return new OkObjectResult(productId);
            }
            return new EmptyResult();
        }

        /// <summary>
        /// Update product quantity
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public IActionResult UpdateCart(int productId, int quantity, int color, int size)
        {
            var session = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.CartSession);
            if (session != null)
            {
                bool hasChanged = false;
                foreach (var item in session)
                {
                    if (item.Product.Id == productId)
                    {
                        var product = _productService.GetById(productId);
                        item.Product = product;
                        item.Size = _billService.GetSize(size);
                        item.Color = _billService.GetColor(color);
                        item.Quantity = quantity;
                        item.Price = product.PromotionPrice ?? product.Price;
                        hasChanged = true;
                    }
                }
                if (hasChanged)
                {
                    HttpContext.Session.Set(CommonConstants.CartSession, session);
                }
                return new OkObjectResult(productId);
            }
            return new EmptyResult();
        }

        [HttpGet]
        public IActionResult GetColors()
        {
            var colors = _billService.GetColors();
            return new OkObjectResult(colors);
        }

        [HttpGet]
        public IActionResult GetSizes()
        {
            var sizes = _billService.GetSizes();
            return new OkObjectResult(sizes);
        }
        #endregion
    }
}