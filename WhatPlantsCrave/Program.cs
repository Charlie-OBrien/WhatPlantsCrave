using Brawndo_Components;
using WhatPlantsCrave.Infrastructure.Repositories;

namespace WhatPlantsCrave
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
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

            builder.Services.AddScoped<IAddressRepository, AddressRepository>();
            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
            builder.Services.AddScoped<ICustomerAddressRepository, CustomerAddressRepository>();

            // Example: Decorator pattern - ProductRepository wrapped with logging
            builder.Services.AddScoped<IProductRepository>(sp =>
                new LoggingProductRepository(
                    new ProductRepository(sp.GetRequiredService<IProductService>()),
                    sp.GetRequiredService<ILogger<LoggingProductRepository>>()
                )
            );

            builder.Services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
            builder.Services.AddScoped<IProductDescriptionRepository, ProductDescriptionRepository>();
            builder.Services.AddScoped<IProductModelRepository, ProductModelRepository>();
            builder.Services.AddScoped<IProductModelProductDescriptionRepository, ProductModelProductDescriptionRepository>();
            builder.Services.AddScoped<ISalesOrderDetailRepository, SalesOrderDetailRepository>();
            builder.Services.AddScoped<ISalesOrderHeaderRepository, SalesOrderHeaderRepository>();

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
