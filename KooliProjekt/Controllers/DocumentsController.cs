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
    public class DocumentsController : Controller
    {
        private readonly IDocumentService _documentService;

        public DocumentsController(IDocumentService documentService)
        {
            _documentService = documentService;
        }

        // GET: Doctors
        public async Task<IActionResult> Index(int page = 1)
        {
            var documents = await _documentService.List(page, 10); // Use List method from the service
            return View(documents);
        }

        // GET: Doctors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _documentService.Get(id.Value); // Use the Get method
            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        // GET: Doctors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Doctors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Specialization,UserId")] Document document)
        {
            if (ModelState.IsValid)
            {
                await _documentService.Save(document); // Use Save method from the service
                return RedirectToAction(nameof(Index));
            }
            return View(document);
        }

        // GET: Doctors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _documentService.Get(id.Value); // Use the Get method
            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        // POST: Doctors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Specialization,UserId")] Document document)
        {
            if (id != document.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _documentService.Save(document); // Use Save method to update
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocumentExists(document.ID))
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

            return View(document);
        }

        // GET: Doctors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _documentService.Get(id.Value); // Use the Get method
            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        // POST: Doctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _documentService.Delete(id); // Use Delete method from the service
            return RedirectToAction(nameof(Index));
        }

        private bool DocumentExists(int id)
        {
            var doctor = _documentService.Get(id).Result; // Check using the Get method
            return doctor != null;
        }
    }
}
