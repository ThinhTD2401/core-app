using System.ComponentModel;

namespace CoreApp.Data.Enums
{
    public enum PaymentMethod
    {
        //[Description("Cash on delivery")]
        [Description("Thanh toán khi nhận hàng")]
        CashOnDelivery,

        //[Description("Onlin Banking")]
        [Description("Thanh toán qua chuyển khoản")]
        OnlinBanking,

        //[Description("Payment Gateway")]
        //PaymentGateway,

        //[Description("Visa")]
        //Visa,

        //[Description("Master Card")]
        //MasterCard,

        //[Description("PayPal")]
        //PayPal,

        //[Description("Atm")]
        //Atm
    }
}