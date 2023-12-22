using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Expense_Tracker.Models;

namespace Expense_Tracker.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TransactionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Transaction
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.transactions.Include(t => t.Category);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Transaction/CreateAndUpdate
        public IActionResult CreateAndUpdate(int id=0)
        {
            populateCategory();
            if(id == 0)
                return View(new Transaction());
            else 
                return View(_context.transactions.Find(id));
        }

		// POST: Transaction/CreateAndUpdate
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAndUpdate([Bind("TransactionId,CategoryId,Amount,Note,Date")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                if(transaction.TransactionId == 0) 
                    _context.Add(transaction);
                else
                    _context.Update(transaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            populateCategory();
			return View(transaction);
        }

        // POST: Transaction/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transaction = await _context.transactions.FindAsync(id);
            if (transaction != null)
            {
                _context.transactions.Remove(transaction);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [NonAction]
        public void populateCategory()
        {
            var categoryCollection = _context.Categories.ToList();
            Category defaultCategories = new Category() { CategoryId = 0 , Title="Choose a Category"};
            categoryCollection.Insert(0, defaultCategories);
            ViewBag.Categories = categoryCollection;

		}
    }
}
