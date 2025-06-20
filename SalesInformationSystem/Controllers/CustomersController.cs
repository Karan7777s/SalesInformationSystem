﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using SalesInformationSystem.Data;
using SalesInformationSystem.Models;

namespace SalesInformationSystem.Controllers
{
    // GET: Customers
  

    
    public class CustomersController : Controller
    {
        //To instantiate an object to the class Compute
       

        //To instantiate an object to the class ApplicationDbContext
        private readonly ApplicationDbContext _context;

        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public CustomersController(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: Customers

        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> myProfile()
        {
            string username = User.Identity.Name;
            var getProfile =_context.Customer.Where(c => c.Email == username).FirstOrDefault();
            return View(getProfile);

        }

        public async Task<IActionResult> GetAllCustomers()
        {
            return View(await _context.Customer.ToListAsync());
        }
        public async Task<IActionResult> Index()
        {
        
            
            return View(await _context.Customer.ToListAsync());
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerId,CustLname,CustFname,Email,Address,Phone")] Customer customer)
        {
         
            _context.Add(customer);
            await _context.SaveChangesAsync();

            var custUserId = _context.Users.Where(u => u.Email == customer.Email).FirstOrDefault().Id;
            var custRoleId = _context.Roles.Where(r => r.Name == "Customer").FirstOrDefault().Id;
            var roleName = _context.Roles.Where(r => r.Name == "Customer").FirstOrDefault().Name;

            var userRole = new IdentityUserRole<string>
            {
                UserId = custUserId,
                RoleId = custRoleId,
            };
            _context.UserRoles.Add(userRole);
            await _context.SaveChangesAsync();
            return Redirect("/Identity/Account/Login");
           
            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerId,CustLname,CustFname,Email,Address,Phone")] Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return NotFound();
            }

          //  if (ModelState.IsValid)
           // {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.CustomerId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
          //  }
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customer.FindAsync(id);
            if (customer != null)
            {
                _context.Customer.Remove(customer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customer.Any(e => e.CustomerId == id);
        }
    }
}
