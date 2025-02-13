using System;

using System.Collections.Generic;

using System.Linq;

using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using KooliProjekt.Services;

using Microsoft.EntityFrameworkCore;

using KooliProjekt.Data;

using KooliProjekt.Models;

using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("KooliProjekt.UnitTests")]

namespace KooliProjekt.Controllers

{

    public class TimesController : Controller

    {

        private readonly ITimeService _timeService;

        public TimesController(ITimeService timeService)

        {

            _timeService = timeService;

        }

        // GET: Times

        public async Task<IActionResult> Index(int page = 1)

        {

            var times = await _timeService.List(page, 10);

            return View(times);

        }

        // GET: Times/Details/5

        public async Task<IActionResult> Details(int? id)

        {

            if (id == null)

            {

                return NotFound();

            }

            var time = await _timeService.Get(id.Value);

            if (time == null)

            {

                return NotFound();

            }

            return View(time);

        }

        // GET: Times/Create

        public IActionResult Create()

        {

            return View();

        }

        // POST: Times/Create

        [HttpPost]

        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create([Bind("Id,DoctorId")] Time time)

        {

            if (ModelState.IsValid)

            {

                await _timeService.Save(time);

                return RedirectToAction(nameof(Index));

            }

            return View(time);

        }

        // GET: Times/Edit/5

        public async Task<IActionResult> Edit(int? id)

        {

            if (id == null)

            {

                return NotFound();

            }

            var time = await _timeService.Get(id.Value);

            if (time == null)

            {

                return NotFound();

            }

            return View(time);

        }

        // POST: Times/Edit/5

        [HttpPost]

        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(int id, [Bind("Id,DoctorId")] Time time)

        {

            if (id != time.Id)

            {

                return NotFound();

            }

            if (ModelState.IsValid)

            {

                try

                {

                    await _timeService.Save(time);

                }

                catch (DbUpdateConcurrencyException)

                {

                    if (!TimeExists(time.Id))

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

        // GET: Times/Delete/5

        public async Task<IActionResult> Delete(int? id)

        {

            if (id == null)

            {

                return NotFound();

            }

            var time = await _timeService.Get(id.Value);

            if (time == null)

            {

                return NotFound();

            }

            return View(time);

        }

        // POST: Times/Delete/5

        [HttpPost, ActionName("Delete")]

        [ValidateAntiForgeryToken]

        public async Task<IActionResult> DeleteConfirmed(int id)

        {

            await _timeService.Delete(id);

            return RedirectToAction(nameof(Index));

        }

        internal bool TimeExists(int id)

        {

            var time = _timeService.Get(id).Result;

            return time != null;

        }

    }

}
