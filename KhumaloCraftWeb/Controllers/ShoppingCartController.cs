using KhumaloCraftWeb.Data;
using KhumaloCraftWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace KhumaloCraftWeb.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ShoppingCartController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Action to display the shopping cart
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            var shoppingCart = await _context.ShoppingCart
                .Include(sc => sc.ShoppingCartItems)
                    .ThenInclude(sci => sci.Product) // Include product information
                .FirstOrDefaultAsync(sc => sc.UserId == user.Id);

            if (shoppingCart == null)
            {
                shoppingCart = new ShoppingCart
                {
                    UserId = user.Id,
                    ShoppingCartItems = new List<ShoppingCartItem>()
                };
                _context.ShoppingCart.Add(shoppingCart);
                shoppingCart.TotalPrice = shoppingCart.ShoppingCartItems.Sum(item => item.Quantity * item.Product.Price);
                await _context.SaveChangesAsync();
            }

            // Calculate total price
            decimal totalPrice = shoppingCart.ShoppingCartItems.Sum(item => item.Quantity * item.Product.Price);
            shoppingCart.TotalPrice = totalPrice;

            await _context.SaveChangesAsync();

            return View(shoppingCart);
        }



        // Action to add a product to the shopping cart
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            // Retrieve the current user
            var user = await _userManager.GetUserAsync(User);

            // Retrieve the product
            var product = await _context.Product.FindAsync(productId);

            if (product == null)
            {
                return NotFound(); // Return 404 Not Found if product is not found
            }

            // Retrieve the user's shopping cart
            var shoppingCart = await _context.ShoppingCart
                .Include(sc => sc.ShoppingCartItems)
                .FirstOrDefaultAsync(sc => sc.UserId == user.Id);

            // Add the product to the shopping cart or update the quantity if already exists
            if (shoppingCart == null)
            {
                // Create a new shopping cart if it doesn't exist
                shoppingCart = new ShoppingCart
                {
                    UserId = user.Id,
                    ShoppingCartItems = new System.Collections.Generic.List<ShoppingCartItem>()
                };
                _context.ShoppingCart.Add(shoppingCart);
            }

            var cartItem = shoppingCart.ShoppingCartItems.FirstOrDefault(item => item.ProductId == productId);
            if (cartItem != null)
            {
                // Update quantity if the product already exists in the cart
                cartItem.Quantity += quantity;
            }
            else
            {
                // Add the product to the cart with the specified quantity
                cartItem = new ShoppingCartItem
                {
                    ProductId = productId,
                    Quantity = quantity
                };
                shoppingCart.ShoppingCartItems.Add(cartItem);
            }

            // Save changes to the database
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Action to remove a product from the shopping cart
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RemoveFromCart(int shoppingCartItemId)
        {
            // Retrieve the shopping cart item
            var shoppingCartItem = await _context.ShoppingCartItem.FindAsync(shoppingCartItemId);

            if (shoppingCartItem != null)
            {
                // Remove the shopping cart item
                _context.ShoppingCartItem.Remove(shoppingCartItem);

                // Save changes to the database
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
