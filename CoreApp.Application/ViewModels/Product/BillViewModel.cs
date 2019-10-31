using CoreApp.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CoreApp.Application.ViewModels.Product
{
    public class BillViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Bạn chưa nhập họ tên")]
        [MaxLength(256, ErrorMessage ="Họ tên dài quá 256 ký tự")]
        public string CustomerName { set; get; }

        [Required(ErrorMessage = "Bạn chưa nhập địa chỉ")]
        [MaxLength(256, ErrorMessage = "Địa chỉ dài quá 256 ký tự")]
        public string CustomerAddress { set; get; }

        [Required(ErrorMessage = "Bạn chưa nhập số điện thoại")]
        [MaxLength(50, ErrorMessage = "Điện thoại quá 50 ký tự")]
        public string CustomerMobile { set; get; }

        [Required(ErrorMessage = "Bạn chưa nhập lưu ý")]
        [MaxLength(256, ErrorMessage = "Lưu ý dài quá 256 ký tự")]
        public string CustomerMessage { set; get; }

        public PaymentMethod PaymentMethod { set; get; }

        public BillStatus BillStatus { set; get; }

        public DateTime DateCreated { set; get; }

        public DateTime DateModified { set; get; }

        public Status Status { set; get; }

        public Guid? CustomerId { set; get; }

        public List<BillDetailViewModel> BillDetails { set; get; }
    }
}
