using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GIP.PRJ.TraiteurApp.Models;
using GIP.PRJ.TraiteurApp.Services.Interfaces;
namespace GIP.PRJ.TraiteurApp.Controllers
{
    public class BusinessHoursController : Controller
    {
        private readonly TraiteurAppDbContext _context;
        private readonly IBusinessHoursService _businessHoursService;
        public BusinessHoursController(TraiteurAppDbContext context, IBusinessHoursService hoursService)
        {
            _context = context;
            _businessHoursService = hoursService;
        }
        // GET: BusinessHours
        public async Task<IActionResult> Index()
        {
            return View(await _context.BusinessHours.ToListAsync());
        }
        // GET: BusinessHours/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.BusinessHours == null)
            {
                return NotFound();
            }
            var businessHours = await _context.BusinessHours
                .FirstOrDefaultAsync(m => m.Id == id);
            if (businessHours == null)
            {
                return NotFound();
            }
            return View(businessHours);
        }
        // GET: BusinessHours/Create
        public IActionResult Create()
        {
            return View();
        }
        // POST: BusinessHours/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DayOfWeek,ClosingDays,OpeningTime,ClosingTime,IsClosed")] BusinessHours businessHours)
        {
            if (ModelState.IsValid)
            {
                _context.Add(businessHours);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(businessHours);
        }
        // GET: BusinessHours/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.BusinessHours == null)
            {
                return NotFound();
            }
            var businessHours = await _context.BusinessHours.FindAsync(id);
            if (businessHours == null)
            {
                return NotFound();
            }
            return View(businessHours);
        }
        // POST: BusinessHours/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DayOfWeek,ClosingDays,OpeningTime,ClosingTime,IsClosed")] BusinessHours businessHours)
        {
            if (id != businessHours.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(businessHours);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BusinessHoursExists(businessHours.Id))
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
            return View(businessHours);
        }
        // GET: BusinessHours/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.BusinessHours == null)
            {
                return NotFound();
            }
            var businessHours = await _context.BusinessHours
                .FirstOrDefaultAsync(m => m.Id == id);
            if (businessHours == null)
            {
                return NotFound();
            }
            return View(businessHours);
        }
        // POST: BusinessHours/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.BusinessHours == null)
            {
                return Problem("Entity set 'TraiteurAppDbContext.BusinessHours'  is null.");
            }
            var businessHours = await _context.BusinessHours.FindAsync(id);
            if (businessHours != null)
            {
                _context.BusinessHours.Remove(businessHours);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool BusinessHoursExists(int id)
        {
            return _context.BusinessHours.Any(e => e.Id == id);
        }
    }
}