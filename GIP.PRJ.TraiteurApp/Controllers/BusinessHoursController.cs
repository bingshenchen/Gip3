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
        private readonly IBusinessHoursService _businessHoursService;

        public BusinessHoursController(IBusinessHoursService businessHoursService)
        {
            _businessHoursService = businessHoursService;
        }

        public async Task<IActionResult> Index()
        {
            var businessHours = await _businessHoursService.GetBusinessHours();
            return View(new List<BusinessHours> { businessHours });
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var businessHours = await _businessHoursService.GetBusinessHours();
            if (businessHours == null)
            {
                return NotFound();
            }
            return View(businessHours);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DayOfWeek,ClosingDays,OpeningTime,ClosingTime,Holidays,IsClosed")] BusinessHours businessHours)
        {
            if (id != businessHours.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _businessHoursService.UpdateBusinessHours(businessHours);
                return RedirectToAction(nameof(Index));
            }
            return View(businessHours);
        }

        [HttpPost]
        public async Task<IActionResult> EditOpeningTime(TimeSpan openingTime)
        {
            await _businessHoursService.SetOpeningTime(openingTime);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> EditClosingTime(TimeSpan closingTime)
        {
            await _businessHoursService.SetClosingTime(closingTime);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> AddHoliday(DateTime holidayDate)
        {
            await _businessHoursService.AddHoliday(holidayDate);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveHoliday(DateTime holidayDate)
        {
            await _businessHoursService.RemoveHoliday(holidayDate);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> SetIsClosed(bool isClosed)
        {
            await _businessHoursService.SetIsClosed(isClosed);
            return RedirectToAction(nameof(Index));
        }
    }
}


