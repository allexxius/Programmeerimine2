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
    public class VisitsController : Controller
    {
        private readonly IVisitService _visitService;

        public VisitsController(IVisitService visitService)
        {
            _visitService = visitService;
        }

        // GET: Doctors
        public async Task<IActionResult> Index(int page = 1)
        {
            var Visit = await _visitService.List(page, 10); // Use List method from the service
            return View(Visit);
        }

        // GET: Doctors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var time = await _visitService.Get(id.Value); // Use the Get method
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
        public async Task<IActionResult> Create([Bind("Id,Specialization,UserId")] Visit visit)
        {
            if (ModelState.IsValid)
            {
                await _visitService.Save(visit); // Use Save method from the service
                return RedirectToAction(nameof(Index));
            }
            return View(visit);
        }

        // GET: Doctors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var visit = await _visitService.Get(id.Value); // Use the Get method
            if (visit == null)
            {
                return NotFound();
            }

            return View(visit);
        }

        // POST: Doctors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Specialization,UserId")] Visit visit)
        {
            if (id != visit.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _visitService.Save(visit); // Use Save method to update
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VisitExists(visit.Id))
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

            return View(visit);
        }

        // GET: Doctors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var visit = await _visitService.Get(id.Value); // Use the Get method
            if (visit == null)
            {
                return NotFound();
            }

            return View(visit);
        }

        // POST: Doctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _visitService.Delete(id); // Use Delete method from the service
            return RedirectToAction(nameof(Index));
        }

        private bool VisitExists(int id)
        {
            var visit = _visitService.Get(id).Result; // Check using the Get method
            return visit != null;
        }
    }
}
