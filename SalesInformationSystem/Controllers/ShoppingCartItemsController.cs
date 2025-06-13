using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SalesInformationSystem.Data;  
using SalesInformationSystem.Models;


namespace SalesInformationSystem.Controllers
{

    //[Authorize("Customer")]
    public class ShoppingCartItemsController : Controller
    {
        public string ShoppingCartId { get; set; }
        public const string CartSessionKey = "CartId";

        private readonly ApplicationDbContext _context;


        public ShoppingCartItemsController (ApplicationDbContext context)
        {
            this.ShoppingCartId = "";
            _context = context;
        }




        public async Task<IActionResult> AddToCart(int id)
        {
            ShoppingCartId = GetCartId();
            CartItems cartItem = new CartItems();
            cartItem = _context.CartItems.SingleOrDefault(c => c.CartId == ShoppingCartId && c.ProductId == id);
            if (cartItem == null)
            {
                // Create a new cart item if no cart item exists.
                cartItem = new CartItems
                {
                    ItemId = id,
                    ProductId = id,
                    Price = (int)_context.Product.SingleOrDefault(p => p.ProductId == id).ProductPrice,
                    CartId = ShoppingCartId,
                    Product = _context.Product.SingleOrDefault(p => p.ProductId == id),
                    Quantity = 1,
                    DataCreated = DateTime.Now
                };
                _context.CartItems.Add(cartItem);
            }
            else
            {
                // If the iten does exist in the cart,
                // then add one to the quantity.
                cartItem.Quantity++;
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("DisplayCartItems");
        }

        public string GetCartId()
        {
            var session = HttpContext.Session.GetString(CartSessionKey);
            if (session == null)
            {
                if (!string.IsNullOrWhiteSpace(User.Identity.Name))
                {
                    session = User.Identity.Name;
                }
                else
                {
                    // Generate a new randon GUID using System.Guid class.
                    Guid tempCartId = Guid.NewGuid();
                    session = tempCartId.ToString();

                }
            }
            return session.ToString();
        }

        public List<CartItems> GetCartItems()
        {
            ShoppingCartId = GetCartId();
            return _context.CartItems.Where(
            c => c.CartId == ShoppingCartId).ToList();
        }
        public ActionResult DisplayCartItems()
        {
            var cartitems = GetCartItems();
            ViewBag.count = cartitems.Count;
            return View(cartitems);
        }

    }
}




