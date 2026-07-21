using Brawndo_Components.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WhatPlantsCrave.Infrastructure.Repositories;

namespace WhatPlantsCrave.Pages.Views
{
    public class SalesOrderDetailsModel : PageModel
    {
        private readonly ISalesOrderDetailRepository _repository;

        public List<SalesOrderDetail> SalesOrderDetails { get; set; } = new();

        public SalesOrderDetailsModel(ISalesOrderDetailRepository repository)
        {
            _repository = repository;
        }

        public void OnGet()
        {
            SalesOrderDetails = _repository.GetAll();
        }
    }
}
