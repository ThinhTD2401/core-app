using CoreApp.Application.ViewModels.Blog;
using CoreApp.Utilities.Dtos;

namespace CoreApp.Models.CommonViewModels
{
    public class BlogComponentViewModel 
    {
        public string Keyword { get; set; }

        public string SortType { get; set; }

        public string SortOrder { get; set; }

        public int PageIndex { get; set; }

        public int? PageSize { get; set; }

        public PageResult<BlogViewModel> Data { get; set; }
    }
}
