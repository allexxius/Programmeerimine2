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
    public class InvoicesController : Controller
    {
        private readonly IInvoiceService _invoiceService;

        public InvoicesController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        // GET: Doctors
        public async Task<IActionResult> Index(int page = 1)
        {
            var Invoice = await _invoiceService.List(page, 10); // Use List method from the service
            return View(Invoice);
        }

        // GET: Doctors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _invoiceService.Get(id.Value); // Use the Get method
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // GET: Doctors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Doctors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Specialization,UserId")] Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                await _invoiceService.Save(invoice); // Use Save method from the service
                return RedirectToAction(nameof(Index));
            }
            return View(invoice);
        }

        // GET: Doctors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _invoiceService.Get(id.Value); // Use the Get method
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // POST: Doctors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Specialization,UserId")] Invoice invoice)
        {
            if (id != invoice.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _invoiceService.Save(invoice); // Use Save method to update
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocumentExists(invoice.Id))
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

            return View(invoice);
        }

        // GET: Doctors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _invoiceService.Get(id.Value); // Use the Get method
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // POST: Doctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _invoiceService.Delete(id); // Use Delete method from the service
            return RedirectToAction(nameof(Index));
        }

        private bool DocumentExists(int id)
        {
            var invoice = _invoiceService.Get(id).Result; // Check using the Get method
            return invoice != null;
        }
    }
}
