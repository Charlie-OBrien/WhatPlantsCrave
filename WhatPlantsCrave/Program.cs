using Brawndo_Components;
using Brawndo_Components.Data;
using Microsoft.EntityFrameworkCore;
using WhatPlantsCrave.Infrastructure.Repositories;

namespace WhatPlantsCrave
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("AdventureWorksConnection");
            builder.Services.AddDbContext<AdventureWorksContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddScoped<IAddressService, AddressService>();
            builder.Services.AddScoped<ICustomerService, CustomerService>();
            builder.Services.AddScoped<ICustomerAddressService, CustomerAddressService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IProductCategoryService, ProductCategoryService>();
            builder.Services.AddScoped<IProductDescriptionService, ProductDescriptionService>();
            builder.Services.AddScoped<IProductModelService, ProductModelService>();
            builder.Services.AddScoped<IProductModelProductDescriptionService, ProductModelProductDescriptionService>();
            builder.Services.AddScoped<ISalesOrderDetailService, SalesOrderDetailService>();
            builder.Services.AddScoped<ISalesOrderHeaderService, SalesOrderHeaderService>();

            // Repository registrations using SQL (production) implementations
            // Each can be swapped with InMemoryXxxRepository for testing without database
            builder.Services.AddScoped<IAddressRepository, SqlAddressRepository>();
            builder.Services.AddScoped<ICustomerRepository, SqlCustomerRepository>();
            builder.Services.AddScoped<ICustomerAddressRepository, SqlCustomerAddressRepository>();
            builder.Services.AddScoped<IProductRepository, SqlProductRepository>();
            builder.Services.AddScoped<IProductCategoryRepository, SqlProductCategoryRepository>();
            builder.Services.AddScoped<IProductDescriptionRepository, SqlProductDescriptionRepository>();
            builder.Services.AddScoped<IProductModelRepository, SqlProductModelRepository>();
            builder.Services.AddScoped<IProductModelProductDescriptionRepository, SqlProductModelProductDescriptionRepository>();
            builder.Services.AddScoped<ISalesOrderDetailRepository, SqlSalesOrderDetailRepository>();
            builder.Services.AddScoped<ISalesOrderHeaderRepository, SqlSalesOrderHeaderRepository>();

            builder.Services.AddRazorPages();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}
