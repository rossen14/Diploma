using Diploma.Data;
using Diploma.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Diploma.Controllers
{
    public class ProductsController : Controller
    {
        private ApplicationDbContext _context;
        public ProductsController(ApplicationDbContext context)
        {
            this._context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            List<Product> products = _context.Products.ToList();
            return View(products);
        }
        // [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return this.View();
        }
        // [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult Create(Product model)
        {
            if (this.ModelState.IsValid)
            {
                var product = new Product
                {
                    Price = model.Price,
                    Name = model.Name,
                    Stock = model.Stock,
                    ImageUrl = model.ImageUrl
                };
                _context.Products.Add(product);
                _context.SaveChanges();
                return RedirectToAction("Index", "Products");
            }
            return this.View();
        }

       // [Authorize(Roles = "admin")]
        [HttpGet]
        [Route("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            return this.View(product);
        }

        // [Authorize(Roles = "admin")]
        [HttpPost]
        [Route("delete/{id}")]
        public IActionResult Delete(int id, Product model)
        {
            var product = _context.Products.Find(id);
            _context.Products.Remove(product);
            _context.SaveChanges();
            return RedirectToAction("Index", "Products");
        }

       // [Authorize(Roles = "admin")]
        [HttpGet]
        [Route("edit/{id}")]
        public IActionResult Edit(int id)
        {
            using (var _context = new ApplicationDbContext())
            {
                {
                    var product = this._context.Products.Find(id);
                    if (product == null)
                    {
                        return NotFound();
                    }
                    return View(product);
                }
            }
        }

        [HttpPost]
        [Route("edit/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Product model)
        {
            using (var _context = new ApplicationDbContext())
            {
                var existingProduct = _context.Products.Find(id);
                if (existingProduct == null)
                {
                    return NotFound();
                }

                if (this.ModelState.IsValid)
                {
                    existingProduct.Name = model.Name;
                    existingProduct.Stock = model.Stock;
                    existingProduct.Price = model.Price;
                    existingProduct.ImageUrl = model.ImageUrl;
                    _context.SaveChanges();

                    return Redirect("/");
                }
                return RedirectToAction("Index", "Products");
            }
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var product = _context.Products.Find(id);
            if(product != null)
            {
                var model = new Product
                {
                    Name = product.Name,
                    ImageUrl = product.ImageUrl,
                    Stock = product.Stock,
                    Price = product.Price
                };
            return View(model);
            }
            return NotFound();
        }
    }
}