using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PartyInvites.Models;

namespace PartyInvites.Controllers
{
    public class ProductsController : Controller
    {
        private readonly DataContext _context;
        private readonly int PageSize = 3;

        public ProductsController(DataContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            return View(new ProductBag { Products = await _context.Products.Skip(0).Take(PageSize).ToListAsync(), TotalProductCount = _context.Products.Count(), Page = 1, PageSize = PageSize });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ProductBag bag)
        {
            var products = _context.Products.AsQueryable();
            if (bag.SelectedCategory != null)
            {
                products = products.Where(p => p.Category == bag.SelectedCategory);
            }

            if (bag.SortBy == "Name")
            {
                if (bag.SortOrder == "desc")
                {
                    products = products.OrderByDescending(p => p.Name);
                }
                else
                {
                    products = products.OrderBy(p => p.Name);
                }
            }
            else if (bag.SortBy == "Price")
            {
                if (bag.SortOrder == "desc")
                {
                    products = products.OrderByDescending(p => p.Price);
                }
                else
                {
                    products = products.OrderBy(p => p.Price);
                }
            }

            bag.TotalProductCount = products.Count();
            bag.Products = await products.Skip((bag.Page - 1) * PageSize).Take(PageSize).ToListAsync();
            
            return View(bag);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,Name,Description,Price,Category,PictureLink,CreatedDate,ModifiedDate")] Product product)
        {
            product.CreatedDate = DateTime.Now;
            product.ModifiedDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,Name,Description,Price,Category,PictureLink,CreatedDate,ModifiedDate")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            product.ModifiedDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
