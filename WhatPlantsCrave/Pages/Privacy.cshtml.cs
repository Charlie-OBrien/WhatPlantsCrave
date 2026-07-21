using Brawndo_Components.Data;
using Brawndo_Components.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WhatPlantsCrave.Pages
{
    public class PrivacyModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;
        private readonly AdventureWorksContext _context;

        public List<Product> Products { get; set; } = new List<Product>();

        public PrivacyModel(ILogger<PrivacyModel> logger, AdventureWorksContext context)
        {
            _logger = logger;
            _context = context;
        }

        public void OnGet()
        {
            Products = _context.Products.ToList();
        }
    }

}
