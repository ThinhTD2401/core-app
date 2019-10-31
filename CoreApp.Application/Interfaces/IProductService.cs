using CoreApp.Application.ViewModels.Common;
using CoreApp.Application.ViewModels.Product;
using CoreApp.Utilities.Dtos;
using System;
using System.Collections.Generic;

namespace CoreApp.Application.Interfaces
{
    public interface IProductService : IDisposable
    {
        PageResult<ProductViewModel> GetAllPaging(int? categoryId, string keyword, int pageIndex, int pageSize, string sortField, string sortOrder);

        ProductViewModel Add(ProductViewModel product);

        void Update(ProductViewModel product);

        void Delete(int id);

        ProductViewModel GetById(int id);

        void Save();

        List<ProductViewModel> GetAll();

        void ImportExcel(string filePath, int categoryId);

        void AddQuantity(int productId, List<ProductQuantityViewModel> quantities);

        List<ProductQuantityViewModel> GetQuantities(int productId);

        void AddImages(int productId, string[] images);

        List<ProductImageViewModel> GetImages(int productId);

        void AddWholePrice(int productId, List<WholePriceViewModel> wholePrices);

        List<WholePriceViewModel> GetWholePrices(int productId);

        List<ProductViewModel> GetHotProduct(int top);

        List<ProductViewModel> GetLastest(int top);

        List<ProductViewModel> GetRelatedProducts(int id, int top);

        List<ProductViewModel> GetUpsellProducts(int top);

        List<TagViewModel> GetProductTags(int productId);
    }
}