using CoreApp.Models.ProductViewModels;

namespace CoreApp.Models.CommonViewModels
{
    public class SearchResultViewModel : CatalogViewModel

    {
        public string Keyword { get; set; }

        public string SortBy { get; set; }

        public string SortOrder { get; set; }

        public int PageIndex { get; set; }
        


    }
}