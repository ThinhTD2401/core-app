using AutoMapper;
using CoreApp.Application.ViewModels.Blog;
using CoreApp.Application.ViewModels.Common;
using CoreApp.Application.ViewModels.Product;
using CoreApp.Application.ViewModels.System;
using CoreApp.Data.Entities;

namespace CoreApp.Application.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile() 
        {
            CreateMap<ProductCategory, ProductCategoryViewModel>();
            CreateMap<Product, ProductViewModel>();
            CreateMap<Function, FunctionViewModel>();
            CreateMap<AppUser, AppUserViewModel>();
            CreateMap<AppRole, AppRoleViewModel>();
            CreateMap<Bill, BillViewModel>();
            CreateMap<BillDetail, BillDetailViewModel>();
            CreateMap<Color, ColorViewModel>();
            CreateMap<Size, SizeViewModel>();
            CreateMap<ProductQuantity, ProductQuantityViewModel>().MaxDepth(2);
            CreateMap<ProductImage, ProductImageViewModel>().MaxDepth(2);
            CreateMap<WholePrice, WholePriceViewModel>().MaxDepth(2);
            CreateMap<Blog, BlogViewModel>();
            CreateMap<BlogTag, BlogTagViewModel>();
            CreateMap<Tag, TagViewModel>();
            CreateMap<SystemConfig, SystemConfigViewModel>();
            CreateMap<Slide, SlideViewModel>();
            CreateMap<Footer, FooterViewModel>();
            CreateMap<Feedback, FeedbackViewModel>();
            CreateMap<Contact, ContactViewModel>();

        }
    }
}