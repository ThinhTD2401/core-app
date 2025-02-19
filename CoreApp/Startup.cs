﻿using AutoMapper;
using CoreApp.Application.Implementation;
using CoreApp.Application.Interfaces;
using CoreApp.Authorization;
using CoreApp.Data.EF;
using CoreApp.Data.EF.Repositories;
using CoreApp.Data.Entities;
using CoreApp.Data.IRepositories;
using CoreApp.Helpers;
using CoreApp.Infrastructure.Interfaces;
using CoreApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using PaulMiami.AspNetCore.Mvc.Recaptcha;
using SqlAlias;
using System;

namespace CoreApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(Aliases.Map(Configuration.GetConnectionString("DefaultConnection")),
                //options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                o => o.MigrationsAssembly("CoreApp.Data.EF")));

            services.AddIdentity<AppUser, AppRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            // Configure Identity
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
 
                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;

                // User settings
                options.User.RequireUniqueEmail = true;
            });
            services.AddScoped<IUserClaimsPrincipalFactory<AppUser>, CustomClaimsPrincipalFactory>();

            // Add application services.
            services.AddScoped<UserManager<AppUser>, UserManager<AppUser>>();
            services.AddScoped<RoleManager<AppRole>, RoleManager<AppRole>>();

            //Config Mapper
            services.AddAutoMapper();
            services.AddSingleton(Mapper.Configuration);
            services.AddScoped<IMapper>(sp => new Mapper(sp.GetRequiredService<AutoMapper.IConfigurationProvider>(), sp.GetService));

            //Config Repository generic and Unit Of Work
            services.AddTransient<IUnitOfWork, EFUnitOfWork>();
            services.AddTransient(typeof(IRepository<,>), typeof(EFRepository<,>));

            //Config Service and Respository
            services.AddTransient<IProductCategoryRepository, ProductCategoryRepository>();
            services.AddTransient<IProductCategoryService, ProductCategoryService>();

            services.AddTransient<IFunctionRepository, FunctionRepository>();
            services.AddTransient<IFunctionService, FunctionService>();

            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IProductService, ProductService>();


            services.AddTransient<IProductTagRepository, ProductTagRepository>();
            services.AddTransient<ITagRepository, TagRepository>();

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IRoleService, RoleService>();

            services.AddTransient<IBillRepository, BillRepository>();
            services.AddTransient<IBillService, BillService>();

            services.AddTransient<IBlogRepository, BlogRepository>();
            services.AddTransient<IBlogService, BlogService>();

            services.AddTransient<IBlogTagRepository, BlogTagRepository>();
            services.AddTransient<ICommonService, CommonService>();

            services.AddTransient<IContactRepository, ContactRespository>();
            services.AddTransient<IContactService, ContactService>();

            services.AddTransient<IFeedbackRepository, FeedbackRepository>();
            services.AddTransient<IFeedbackService, FeedbackService>();

            services.AddTransient<IProductCategoryRepository, ProductCategoryRepository>();
            services.AddTransient<IProductCategoryService, ProductCategoryService>();


            services.AddTransient<IBillDetailRepository, BillDetailRepository>();

            services.AddTransient<IWholePriceRepository, WholePriceRepository>();

            services.AddTransient<IProductImageRepository, ProductImageRepository>();

            services.AddTransient<IProductQuantityRepository, ProductQuantityRepository>();

            services.AddTransient<IColorRepository, ColorRepository>();

            services.AddTransient<ISizeRepository, SizeRepository>();

            services.AddTransient<ISlideRepository, SlideRepository>();

            services.AddTransient<IFooterRepository, FooterRepository>();

            services.AddTransient<IPermissionRepository, PermissionRepository>();

            services.AddTransient<ISystemConfigRepository, SystemConfigRepository>();

            services.AddTransient<IAuthorizationHandler, BaseResourceAuthorizationHandler>();

            services.AddTransient<IViewRenderService, ViewRenderService>();

            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<DbInitializer>();
            services.AddMvc().AddJsonOptions
                (options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());
            
            //Setting captcha
            services.AddRecaptcha(new RecaptchaOptions()
            {
                SiteKey = Configuration["Recaptcha:SiteKey"],
                SecretKey = Configuration["Recaptcha:SecretKey"],
                ValidationMessage ="Bạn chưa xác thực mã captcha."
            });

            //Setting Sesssion 
            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromSeconds(60);
                options.Cookie.HttpOnly = true;
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, DbInitializer dbInitializer, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile("Logs/appcore-{Date}.txt");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(name: "areaRoute",
                    template: "{area:exists}/{controller=Login}/{action=Index}/{id?}");

            });
            
        }
    }
}