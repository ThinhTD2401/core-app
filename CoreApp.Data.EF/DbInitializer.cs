using CoreApp.Data.Entities;
using CoreApp.Data.Enums;
using CoreApp.Utilities.Constants;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreApp.Data.EF
{
    public class DbInitializer
    {
        private readonly AppDbContext _context;
        private UserManager<AppUser> _userManager;
        private RoleManager<AppRole> _roleManager;

        public DbInitializer(AppDbContext context, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task Seed()
        {
            try
            {
                if (!_roleManager.Roles.Any())
                {
                    await _roleManager.CreateAsync(new AppRole()
                    {
                        Name = "Admin",
                        NormalizedName = "Admin",
                        Description = "Quản lý"
                    });
                    await _roleManager.CreateAsync(new AppRole()
                    {
                        Name = "Staff",
                        NormalizedName = "Staff",
                        Description = "Nhân viên"
                    });
                    await _roleManager.CreateAsync(new AppRole()
                    {
                        Name = "Customer",
                        NormalizedName = "Customer",
                        Description = "Khách hàng"
                    });
                }
                if (!_userManager.Users.Any())
                {
                    await _userManager.CreateAsync(new AppUser()
                    {
                        UserName = "thinhtd",
                        FullName = "Trịnh Đức Thịnh",
                        Email = "thinhtd2401@gmail.com",
                        Balance = 0,
                        DateCreated = DateTime.Now,
                        DateModified = DateTime.Now,
                        Status = Status.Active
                    }, "123654$");
                    AppUser user = await _userManager.FindByNameAsync("thinhtd");
                    await _userManager.AddToRoleAsync(user, "Admin");
                }
                if (_context.Functions.Count() == 0)
                {
                    _context.Functions.AddRange(new List<Function>()
                {
                    new Function() {Id = "SYSTEM", Name = "System",ParentId = null,SortOrder = 1,Status = Status.Active,URL = "/",IconCss = "fa-desktop"  },
                    new Function() {Id = "ROLE", Name = "Role",ParentId = "SYSTEM",SortOrder = 1,Status = Status.Active,URL = "/admin/role/index",IconCss = "fa-home"  },
                    new Function() {Id = "FUNCTION", Name = "Function",ParentId = "SYSTEM",SortOrder = 2,Status = Status.Active,URL = "/admin/function/index",IconCss = "fa-home"  },
                    new Function() {Id = "USER", Name = "User",ParentId = "SYSTEM",SortOrder =3,Status = Status.Active,URL = "/admin/user/index",IconCss = "fa-home"  },
                    new Function() {Id = "ACTIVITY", Name = "Activity",ParentId = "SYSTEM",SortOrder = 4,Status = Status.Active,URL = "/admin/activity/index",IconCss = "fa-home"  },
                    new Function() {Id = "ERROR", Name = "Error",ParentId = "SYSTEM",SortOrder = 5,Status = Status.Active,URL = "/admin/error/index",IconCss = "fa-home"  },
                    new Function() {Id = "SETTING", Name = "Configuration",ParentId = "SYSTEM",SortOrder = 6,Status = Status.Active,URL = "/admin/setting/index",IconCss = "fa-home"  },
                    new Function() {Id = "PRODUCT",Name = "Product Management",ParentId = null,SortOrder = 2,Status = Status.Active,URL = "/",IconCss = "fa-chevron-down"  },
                    new Function() {Id = "PRODUCT_CATEGORY",Name = "Category",ParentId = "PRODUCT",SortOrder =1,Status = Status.Active,URL = "/admin/productcategory/index",IconCss = "fa-chevron-down"  },
                    new Function() {Id = "PRODUCT_LIST",Name = "Product",ParentId = "PRODUCT",SortOrder = 2,Status = Status.Active,URL = "/admin/product/index",IconCss = "fa-chevron-down"  },
                    new Function() {Id = "BILL",Name = "Bill",ParentId = "PRODUCT",SortOrder = 3,Status = Status.Active,URL = "/admin/bill/index",IconCss = "fa-chevron-down"  },
                    new Function() {Id = "CONTENT",Name = "Content",ParentId = null,SortOrder = 3,Status = Status.Active,URL = "/",IconCss = "fa-table"  },
                    new Function() {Id = "BLOG",Name = "Blog",ParentId = "CONTENT",SortOrder = 1,Status = Status.Active,URL = "/admin/blog/index",IconCss = "fa-table"  },
                    new Function() {Id = "UTILITY",Name = "Utilities",ParentId = null,SortOrder = 4,Status = Status.Active,URL = "/",IconCss = "fa-clone"  },
                    new Function() {Id = "FOOTER",Name = "Footer",ParentId = "UTILITY",SortOrder = 1,Status = Status.Active,URL = "/admin/footer/index",IconCss = "fa-clone"  },
                    new Function() {Id = "FEEDBACK",Name = "Feedback",ParentId = "UTILITY",SortOrder = 2,Status = Status.Active,URL = "/admin/feedback/index",IconCss = "fa-clone"  },
                    new Function() {Id = "ANNOUNCEMENT",Name = "Announcement",ParentId = "UTILITY",SortOrder = 3,Status = Status.Active,URL = "/admin/announcement/index",IconCss = "fa-clone"  },
                    new Function() {Id = "CONTACT",Name = "Contact",ParentId = "UTILITY",SortOrder = 4,Status = Status.Active,URL = "/admin/contact/index",IconCss = "fa-clone"  },
                    new Function() {Id = "SLIDE",Name = "Slide",ParentId = "UTILITY",SortOrder = 5,Status = Status.Active,URL = "/admin/slide/index",IconCss = "fa-clone"  },
                    new Function() {Id = "ADVERTISMENT",Name = "Advertisment",ParentId = "UTILITY",SortOrder = 6,Status = Status.Active,URL = "/admin/advertistment/index",IconCss = "fa-clone"  },

                    new Function() {Id = "REPORT",Name = "Report",ParentId = null,SortOrder = 5,Status = Status.Active,URL = "/",IconCss = "fa-bar-chart-o"  },
                    new Function() {Id = "REVENUES",Name = "Revenue report",ParentId = "REPORT",SortOrder = 1,Status = Status.Active,URL = "/admin/report/revenues",IconCss = "fa-bar-chart-o"  },
                    new Function() {Id = "ACCESS",Name = "Visitor Report",ParentId = "REPORT",SortOrder = 2,Status = Status.Active,URL = "/admin/report/visitor",IconCss = "fa-bar-chart-o"  },
                    new Function() {Id = "READER",Name = "Reader Report",ParentId = "REPORT",SortOrder = 3,Status = Status.Active,URL = "/admin/report/reader",IconCss = "fa-bar-chart-o"  },
                });
                }
                if (!_context.Contacts.Any())
                {
                    _context.Contacts.Add(new Contact()
                    {
                        Id = CommonConstants.DefaultContactId,
                        Name = "TD Shop",
                        Phone = "0923 666 889",
                        Email = "tdshop2401@gmail.com",
                        Website = "http://tdshop.com",
                        Address = "No 17 - Lane 119 - Quan Hoa - Cau Giay",
                        Other = "Other",
                        Lng = 105.8045186,
                        Lat = 21.033672,
                        Status = Status.Active,
                    });
                }
                if (_context.Footers.Count(x => x.Id == CommonConstants.DefaultFooterId) == 0)
                {
                    string content = "Footer";
                    _context.Footers.Add(new Footer()
                    {
                        Id = CommonConstants.DefaultFooterId,
                        Content = content
                    });
                }

                if (_context.Colors.Count() == 0)
                {
                    List<Color> listColor = new List<Color>()
                {
                    new Color() {Name="Đen", Code="#000000" },
                    new Color() {Name="Trắng", Code="#FFFFFF"},
                    new Color() {Name="Đỏ", Code="#ff0000" },
                    new Color() {Name="Xanh", Code="#1000ff" },
                };
                    _context.Colors.AddRange(listColor);
                }
                if (_context.AdvertistmentPages.Count() == 0)
                {
                    List<AdvertistmentPage> pages = new List<AdvertistmentPage>()
                {
                    new AdvertistmentPage() {Id="home", Name="Home",AdvertistmentPositions = new List<AdvertistmentPosition>(){
                        new AdvertistmentPosition(){Id="home-left",Name="Bên trái"}
                    } },
                    new AdvertistmentPage() {Id="product-cate", Name="Product category" ,
                        AdvertistmentPositions = new List<AdvertistmentPosition>(){
                        new AdvertistmentPosition(){Id="product-cate-left",Name="Bên trái"}
                    }},
                    new AdvertistmentPage() {Id="product-detail", Name="Product detail",
                        AdvertistmentPositions = new List<AdvertistmentPosition>(){
                        new AdvertistmentPosition(){Id="product-detail-left",Name="Bên trái"}
                    } },
                };
                    _context.AdvertistmentPages.AddRange(pages);
                }

                if (_context.Slides.Count() == 0)
                {
                    List<Slide> slides = new List<Slide>()
                {
                    new Slide() {Name="Slide 1",Image="/client-side/images/slider/slide-1.jpg",Url="#",DisplayOrder = 0,GroupAlias = "top",Status = true },
                    new Slide() {Name="Slide 2",Image="/client-side/images/slider/slide-2.jpg",Url="#",DisplayOrder = 1,GroupAlias = "top",Status = true },
                    new Slide() {Name="Slide 3",Image="/client-side/images/slider/slide-3.jpg",Url="#",DisplayOrder = 2,GroupAlias = "top",Status = true },

                    new Slide() {Name="Slide 1",Image="/client-side/images/brand1.png",Url="#",DisplayOrder = 1,GroupAlias = "brand",Status = true },
                    new Slide() {Name="Slide 2",Image="/client-side/images/brand2.png",Url="#",DisplayOrder = 2,GroupAlias = "brand",Status = true },
                    new Slide() {Name="Slide 3",Image="/client-side/images/brand3.png",Url="#",DisplayOrder = 3,GroupAlias = "brand",Status = true },
                    new Slide() {Name="Slide 4",Image="/client-side/images/brand4.png",Url="#",DisplayOrder = 4,GroupAlias = "brand",Status = true },
                    new Slide() {Name="Slide 5",Image="/client-side/images/brand5.png",Url="#",DisplayOrder = 5,GroupAlias = "brand",Status = true },
                    new Slide() {Name="Slide 6",Image="/client-side/images/brand6.png",Url="#",DisplayOrder = 6,GroupAlias = "brand",Status = true },
                    new Slide() {Name="Slide 7",Image="/client-side/images/brand7.png",Url="#",DisplayOrder = 7,GroupAlias = "brand",Status = true },
                    new Slide() {Name="Slide 8",Image="/client-side/images/brand8.png",Url="#",DisplayOrder = 8,GroupAlias = "brand",Status = true },
                    new Slide() {Name="Slide 9",Image="/client-side/images/brand9.png",Url="#",DisplayOrder = 9,GroupAlias = "brand",Status = true },
                    new Slide() {Name="Slide 10",Image="/client-side/images/brand10.png",Url="#",DisplayOrder = 10,GroupAlias = "brand",Status = true },
                    new Slide() {Name="Slide 11",Image="/client-side/images/brand11.png",Url="#",DisplayOrder = 11,GroupAlias = "brand",Status = true },
                };
                    _context.Slides.AddRange(slides);
                }

                if (_context.Sizes.Count() == 0)
                {
                    List<Size> listSize = new List<Size>()
                {
                    new Size() { Name="XXL" },
                    new Size() { Name="XL"},
                    new Size() { Name="L" },
                    new Size() { Name="M" },
                    new Size() { Name="S" },
                    new Size() { Name="XS" }
                };
                    _context.Sizes.AddRange(listSize);
                }

                if (_context.ProductCategories.Count() == 0)
                {
                    List<ProductCategory> listProductCategory = new List<ProductCategory>()
                {
                    new ProductCategory() { Name="Áo sơ mi nam",SeoAlias="ao-so-mi-nam",ParentId = null,Status=Status.Active,SortOrder=1,
                        Products = new List<Product>()
                        {
                            new Product(){Name = "Sản phẩm 1",DateCreated=DateTime.Now,Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-1",Price = 250000,Status = Status.Active,OriginalPrice = 23000},
                            new Product(){Name = "Sản phẩm 2",DateCreated=DateTime.Now,Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-2",Price = 250000,Status = Status.Active,OriginalPrice = 23000},
                            new Product(){Name = "Sản phẩm 3",DateCreated=DateTime.Now,Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-3",Price = 250000,Status = Status.Active,OriginalPrice = 23000},
                            new Product(){Name = "Sản phẩm 4",DateCreated=DateTime.Now,Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-4",Price = 250000,Status = Status.Active,OriginalPrice = 23000},
                            new Product(){Name = "Sản phẩm 5",DateCreated=DateTime.Now,Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-5",Price = 250000,Status = Status.Active,OriginalPrice = 23000},
                        }
                    },
                    new ProductCategory() { Name="Áo sơ mi nữ",SeoAlias="ao-so-mi-nu",ParentId = null,Status=Status.Active ,SortOrder=2,
                        Products = new List<Product>()
                        {
                            new Product(){Name = "Sản phẩm 6",DateCreated=DateTime.Now,Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-6",Price = 280000,Status = Status.Active,OriginalPrice = 280000},
                            new Product(){Name = "Sản phẩm 7",DateCreated=DateTime.Now,Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-7",Price = 280000,Status = Status.Active,OriginalPrice = 280000},
                            new Product(){Name = "Sản phẩm 8",DateCreated=DateTime.Now,Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-8",Price = 280000,Status = Status.Active,OriginalPrice = 280000},
                            new Product(){Name = "Sản phẩm 9",DateCreated=DateTime.Now,Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-9",Price = 280000,Status = Status.Active,OriginalPrice = 280000},
                            new Product(){Name = "Sản phẩm 10",DateCreated=DateTime.Now,Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-10",Price = 280000,Status = Status.Active,OriginalPrice = 280000},
                        }},
                    new ProductCategory() { Name="Áo phông nam",SeoAlias="ao-phong-nam",ParentId = null,Status=Status.Active ,SortOrder=3,
                        Products = new List<Product>()
                        {
                            new Product(){Name = "Sản phẩm 11",DateCreated=DateTime.Now,Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-11",Price = 300000,Status = Status.Active,OriginalPrice = 280000},
                            new Product(){Name = "Sản phẩm 12",DateCreated=DateTime.Now,Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-12",Price = 300000,Status = Status.Active,OriginalPrice = 280000},
                            new Product(){Name = "Sản phẩm 13",DateCreated=DateTime.Now,Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-13",Price = 300000,Status = Status.Active,OriginalPrice = 280000},
                            new Product(){Name = "Sản phẩm 14",DateCreated=DateTime.Now,Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-14",Price = 300000,Status = Status.Active,OriginalPrice = 280000},
                            new Product(){Name = "Sản phẩm 15",DateCreated=DateTime.Now,Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-15",Price = 300000,Status = Status.Active,OriginalPrice = 280000},
                        }},
                    new ProductCategory() { Name="Áo phông nữ",SeoAlias="ao-phong-nu",ParentId = null,Status=Status.Active,SortOrder=4,
                        Products = new List<Product>()
                        {
                            new Product(){Name = "Sản phẩm 16",DateCreated=DateTime.Now, Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-16",Price = 300000,Status = Status.Active,OriginalPrice = 280000},
                            new Product(){Name = "Sản phẩm 17",DateCreated=DateTime.Now,Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-17",Price = 300000,Status = Status.Active,OriginalPrice = 280000},
                            new Product(){Name = "Sản phẩm 18",DateCreated=DateTime.Now,Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-18",Price = 300000,Status = Status.Active,OriginalPrice = 280000},
                            new Product(){Name = "Sản phẩm 19",DateCreated=DateTime.Now,Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-19",Price = 300000,Status = Status.Active,OriginalPrice = 280000},
                            new Product(){Name = "Sản phẩm 20",DateCreated=DateTime.Now,Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-20",Price = 300000,Status = Status.Active,OriginalPrice = 280000},
                        }}
                };
                    _context.ProductCategories.AddRange(listProductCategory);
                }


                if (_context.Blogs.Count() == 0)
                {
                    List<Blog> lstBlog = new List<Blog>() {
                        new Blog()
                        {
                            Name = "Chọn áo phông/áo thun nam giới dáng người gầy: Không khó như bạn vẫn nghĩ!",
                            Content = "Chọn áo phông/áo thun nam giới dáng người gầy: Không khó như bạn vẫn nghĩ!",
                            Image = "/uploaded/images/20190724/t-shirt.jpg",
                            Status= Status.Active,
                            DateCreated=DateTime.Now
                        },
                        new Blog()
                        {
                            Name = "Chọn áo phông/áo thun nam giới dáng người gầy: Không khó như bạn vẫn nghĩ!",
                            Content = "Chọn áo phông/áo thun nam giới dáng người gầy: Không khó như bạn vẫn nghĩ!",
                            Image = "/uploaded/images/20190724/t-shirt.jpg",
                            Status= Status.Active,
                            DateCreated=DateTime.Now
                        },
                        new Blog()
                        {
                            Name = "Chọn áo phông/áo thun nam giới dáng người gầy: Không khó như bạn vẫn nghĩ!",
                            Content = "Chọn áo phông/áo thun nam giới dáng người gầy: Không khó như bạn vẫn nghĩ!",
                            Image = "/uploaded/images/20190724/t-shirt.jpg",
                            Status= Status.Active,
                            DateCreated=DateTime.Now
                        }
                    };
                    _context.Blogs.AddRange(lstBlog);
                }

                if (!_context.SystemConfigs.Any(x => x.Id == "HomeTitle"))
                {
                    _context.SystemConfigs.Add(new SystemConfig()
                    {
                        Id = "HomeTitle",
                        Name = "Home's title",
                        Value1 = "Td Shop trang chủ",
                        Status = Status.Active
                    });
                }
                if (!_context.SystemConfigs.Any(x => x.Id == "HomeMetaKeyword"))
                {
                    _context.SystemConfigs.Add(new SystemConfig()
                    {
                        Id = "HomeMetaKeyword",
                        Name = "Home Keyword",
                        Value1 = "Bán hàng, khuyến mãi",
                        Status = Status.Active
                    });
                }
                if (!_context.SystemConfigs.Any(x => x.Id == "HomeMetaDescription"))
                {
                    _context.SystemConfigs.Add(new SystemConfig()
                    {
                        Id = "HomeMetaDescription",
                        Name = "Home Description",
                        Value1 = "Trang chủ Td shop",
                        Status = Status.Active
                    });
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
    }
}