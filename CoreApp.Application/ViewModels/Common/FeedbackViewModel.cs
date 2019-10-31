using CoreApp.Data.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace CoreApp.Application.ViewModels.Common
{
    public class FeedbackViewModel
    {
        public int Id { set; get; }
        [StringLength(250)]
        [Required(ErrorMessage ="Họ tên cần có thông tin")]
        public string Name { set; get; }

        [StringLength(250, ErrorMessage ="Email không vượt quá 250 ký tự")]
        public string Email { set; get; }

        [StringLength(500, ErrorMessage ="Phản hồi không được vượt quá 500 ký tự")]
        [Required (ErrorMessage ="Phản hồi cần có thông tin")]
        public string Message { set; get; }

        public Status Status { set; get; }
        public DateTime DateCreated { set; get; }
        public DateTime DateModified { set; get; }
    }
}