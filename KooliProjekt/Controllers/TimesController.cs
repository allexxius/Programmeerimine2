using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KooliProjekt.Services;
using Microsoft.EntityFrameworkCore;
using KooliProjekt.Data;

namespace KooliProjekt.Controllers
{
    public class TimesController : Controller
    {
        private readonly ITimeService _timeService;

        public TimesController(ITimeService timeService)
        {
            _timeService = timeService;
        }

        // GET: Doctors
        public async Task<IActionResult> Index(int page = 1)
        {
            var Time = await _timeService.List(page, 10); // Use List method from the service
            return View(Time);
        }

        // GET: Doctors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var time = await _timeService.Get(id.Value); // Use the Get method
            if (time == null)
            {
                return NotFound();
            }

            return View(time);
        }

        // GET: Doctors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Doctors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Specialization,UserId")] Time time)
        {
            if (ModelState.IsValid)
            {
                await _timeService.Save(time); // Use Save method from the service
                return RedirectToAction(nameof(Index));
            }
            return View(time);
        }

        // GET: Doctors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var time = await _timeService.Get(id.Value); // Use the Get method
            if (time == null)
            {
                return NotFound();
            }

            return View(time);
        }

        // POST: Doctors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Specialization,UserId")] Time time)
        {
            if (id != time.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _timeService.Save(time); // Use Save method to update
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocumentExists(time.Id))
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

            return View(time);
        }

        // GET: Doctors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var time = await _timeService.Get(id.Value); // Use the Get method
            if (time == null)
            {
                return NotFound();
            }

            return View(time);
        }

        // POST: Doctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _timeService.Delete(id); // Use Delete method from the service
            return RedirectToAction(nameof(Index));
        }

        private bool DocumentExists(int id)
        {
            var time = _timeService.Get(id).Result; // Check using the Get method
            return time != null;
        }
    }
}
