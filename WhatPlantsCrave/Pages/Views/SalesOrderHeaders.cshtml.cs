using Brawndo_Components.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WhatPlantsCrave.Infrastructure.Repositories;

namespace WhatPlantsCrave.Pages.Views
{
    public class SalesOrderHeadersModel : PageModel
    {
        private readonly ISalesOrderHeaderRepository _repository;

        public List<SalesOrderHeader> SalesOrderHeaders { get; set; } = new();

        public SalesOrderHeadersModel(ISalesOrderHeaderRepository repository)
        {
            _repository = repository;
        }

        public void OnGet()
        {
            SalesOrderHeaders = _repository.GetAll();
        }
    }
}
