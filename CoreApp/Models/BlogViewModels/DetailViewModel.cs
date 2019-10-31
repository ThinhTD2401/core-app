using CoreApp.Application.ViewModels.Blog;
using CoreApp.Application.ViewModels.Common;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreApp.Models.BlogViewModels
{
    public class DetailViewModel
    {
        public BlogViewModel Blog { get; set; }
        public bool Available { set; get; }
        public List<TagViewModel> Tags { set; get; }
        public List<SelectListItem> Colors { set; get; }
        public List<SelectListItem> Sizes { set; get; }
    }
}
