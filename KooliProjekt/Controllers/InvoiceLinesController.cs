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

    public class InvoiceLinesController : Controller

    {

        private readonly IInvoiceLineService _invoiceLineService;

        public InvoiceLinesController(IInvoiceLineService invoiceLineService)

        {

            _invoiceLineService = invoiceLineService;

        }

        // GET: InvoiceLines

        public async Task<IActionResult> Index(int page = 1)

        {

            var invoiceLines = await _invoiceLineService.List(page, 10);

            return View(invoiceLines);

        }

        // GET: InvoiceLines/Details/5

        public async Task<IActionResult> Details(int? id)

        {

            if (id == null)

            {

                return NotFound();

            }

            var invoiceLine = await _invoiceLineService.Get(id.Value);

            if (invoiceLine == null)

            {

                return NotFound();

            }

            return View(invoiceLine);

        }

        // GET: InvoiceLines/Create

        public IActionResult Create()

        {

            return View();

        }

        // POST: InvoiceLines/Create

        [HttpPost]

        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create([Bind("Id,Service")] InvoiceLine invoiceLine)

        {

            if (ModelState.IsValid)

            {

                await _invoiceLineService.Save(invoiceLine);

                return RedirectToAction(nameof(Index));

            }

            return View(invoiceLine);

        }

        // GET: InvoiceLines/Edit/5

        public async Task<IActionResult> Edit(int? id)

        {

            if (id == null)

            {

                return NotFound();

            }

            var invoiceLine = await _invoiceLineService.Get(id.Value);

            if (invoiceLine == null)

            {

                return NotFound();

            }

            return View(invoiceLine);

        }

        // POST: InvoiceLines/Edit/5

        [HttpPost]

        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(int id, [Bind("Id,Service")] InvoiceLine invoiceLine)

        {

            if (id != invoiceLine.Id)

            {

                return NotFound();

            }

            if (ModelState.IsValid)

            {

                try

                {

                    await _invoiceLineService.Save(invoiceLine);

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

        // GET: InvoiceLines/Delete/5

        public async Task<IActionResult> Delete(int? id)

        {

            if (id == null)

            {

                return NotFound();

            }

            var invoiceLine = await _invoiceLineService.Get(id.Value);

            if (invoiceLine == null)

            {

                return NotFound();

            }

            return View(invoiceLine);

        }

        // POST: InvoiceLines/Delete/5

        [HttpPost, ActionName("Delete")]

        [ValidateAntiForgeryToken]

        public async Task<IActionResult> DeleteConfirmed(int id)

        {

            await _invoiceLineService.Delete(id);

            return RedirectToAction(nameof(Index));

        }

        public bool DocumentExists(int id)

        {

            var invoiceLine = _invoiceLineService.Get(id).Result;

            return invoiceLine != null;

        }

    }

}
