using CoreApp.Models.CommonViewModels;
using System;
using System.ComponentModel.DataAnnotations;

namespace CoreApp.Models.AccountViewModels
{
    public class RegisterViewModel 
    {
        [Required(ErrorMessage = "Bạn chưa nhập họ và tên", AllowEmptyStrings = false)]
        [Display(Name = "FullName")]
        public string FullName { set; get; }

        [Display(Name = "BirthDay")]
        public DateTime? BirthDay { set; get; }

        [Required(ErrorMessage = "Bạn chưa nhập email")]
        [EmailAddress(ErrorMessage ="Email đã nhập không hợp lệ")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        
        [Required(ErrorMessage = "Bạn chưa nhập mật khẩu")]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [StringLength(100, ErrorMessage = "Mật khẩu {0} phải ít nhất có 6 ký tự", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword")]
       // [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [Compare("Password", ErrorMessage = "Xác nhận mật khẩu và mật khẩu không đúng")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "Phone number")]
        public string PhoneNumber { set; get; }

        [Display(Name = "Avatar")]
        public string Avatar { get; set; }
    }
}