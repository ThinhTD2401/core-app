using CoreApp.Application.ViewModels.Common;
using CoreApp.Application.ViewModels.Product;
using CoreApp.Data.Enums;
using CoreApp.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreApp.Models
{
    public class CheckoutViewModel : BillViewModel
    {
        public List<ShoppingCartViewModel> Carts { get; set; }

        public List<EnumModel> PaymentMethods => ((PaymentMethod[])Enum.GetValues(typeof(PaymentMethod)))
                    .Select(c => new EnumModel
                    {
                        Value = (int)c,
                        Name = c.GetDescription()
                    }).ToList();
    }
}