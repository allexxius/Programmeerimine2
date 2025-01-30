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

    public class InvoiceLinesController : Controller

    {

        private readonly IInvoiceLineService _invoiceLineService;

        public InvoiceLinesController(IInvoiceLineService invoiceLineService)

        {

            _invoiceLineService = invoiceLineService;

        }

        // GET: Doctors

        public async Task<IActionResult> Index(int page = 1)

        {

            var InvoiceLine = await _invoiceLineService.List(page, 10); // Use List method from the service

            return View(InvoiceLine);

        }

        // GET: Doctors/Details/5

        public async Task<IActionResult> Details(int? id)

        {

            if (id == null)

            {

                return NotFound();

            }

            var invoiceLine = await _invoiceLineService.Get(id.Value); // Use the Get method

            if (invoiceLine == null)

            {

                return NotFound();

            }

            return View(invoiceLine);

        }

        // GET: Doctors/Create

        public IActionResult Create()

        {

            return View();

        }

        // POST: Doctors/Create

        [HttpPost]

        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create([Bind("Id,Specialization,UserId")] InvoiceLine invoiceLine)

        {

            if (ModelState.IsValid)

            {

                await _invoiceLineService.Save(invoiceLine); // Use Save method from the service

                return RedirectToAction(nameof(Index));

            }

            return View(invoiceLine);

        }

        // GET: Doctors/Edit/5

        public async Task<IActionResult> Edit(int? id)

        {

            if (id == null)

            {

                return NotFound();

            }

            var invoiceLine = await _invoiceLineService.Get(id.Value); // Use the Get method

            if (invoiceLine == null)

            {

                return NotFound();

            }

            return View(invoiceLine);

        }

        // POST: Doctors/Edit/5

        [HttpPost]

        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(int id, [Bind("Id,Specialization,UserId")] InvoiceLine invoiceLine)

        {

            if (id != invoiceLine.Id)

            {

                return NotFound();

            }

            if (ModelState.IsValid)

            {

                try

                {

                    await _invoiceLineService.Save(invoiceLine); // Use Save method to update

                }

                catch (DbUpdateConcurrencyException)

                {

                    if (!DocumentExists(invoiceLine.Id))

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

            return View(invoiceLine);

        }

        // GET: Doctors/Delete/5

        public async Task<IActionResult> Delete(int? id)

        {

            if (id == null)

            {

                return NotFound();

            }

            var invoiceLine = await _invoiceLineService.Get(id.Value); // Use the Get method

            if (invoiceLine == null)

            {

                return NotFound();

            }

            return View(invoiceLine);

        }

        // POST: Doctors/Delete/5

        [HttpPost, ActionName("Delete")]

        [ValidateAntiForgeryToken]

        public async Task<IActionResult> DeleteConfirmed(int id)

        {

            await _invoiceLineService.Delete(id); // Use Delete method from the service

            return RedirectToAction(nameof(Index));

        }

        private bool DocumentExists(int id)

        {

            var invoiceLine = _invoiceLineService.Get(id).Result; // Check using the Get method

            return invoiceLine != null;

        }

    }

}

