using KhumaloCraftWeb.Data;
using KhumaloCraftWeb.Models;
using KhumaloCraftWeb.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace KhumaloCraftWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ProductRepository _productRepository;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, ProductRepository productRepository)
        {
            _context = context;
            _productRepository = productRepository;
            _logger = logger;
        }


        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AboutUs()
        {
            return View();
        }
        public IActionResult ContactUs()
        {
            return View();
        }
        public async Task<IActionResult> MyWork()
        {
            var Products = await _context.Product.ToListAsync();
            return View(Products);
        }

        // Other action methods...
        public IActionResult SeedProducts()
        {
            _productRepository.InsertProducts();
            return RedirectToAction("MyWork"); // Redirect to MyWork action after seeding products
        }
        public IActionResult Privacy()
        {
            return View();
        }


    }
}
