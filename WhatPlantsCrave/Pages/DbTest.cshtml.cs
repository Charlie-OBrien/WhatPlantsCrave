using Brawndo_Components.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WhatPlantsCrave.Pages
{
    public class DbTestModel : PageModel
    {
        private readonly AdventureWorksContext _context;
        public bool IsConnected { get; set; }
        public string ConnectionMessage { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;

        public DbTestModel(AdventureWorksContext context)
        {
            _context = context;
        }

        public async Task OnGetAsync()
        {
            try
            {
                var canConnect = await _context.Database.CanConnectAsync();
                if (canConnect)
                {
                    IsConnected = true;
                    ConnectionMessage = "Connected to AdventureWorksLT2016 database on DESKTOP-OAJVK2J";
                }
                else
                {
                    IsConnected = false;
                    ErrorMessage = "Connection string exists but unable to connect to database.";
                }
            }
            catch (Exception ex)
            {
                IsConnected = false;
                ErrorMessage = $"Error: {ex.Message}";
            }
        }
    }
}
