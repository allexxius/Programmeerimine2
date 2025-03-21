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

    public class DocumentsController : Controller

    {

        private readonly IDocumentService _documentService;

        public DocumentsController(IDocumentService documentService)

        {

            _documentService = documentService;

        }

        // GET: Documents

        public async Task<IActionResult> Index(int page = 1)

        {

            var documents = await _documentService.List(page, 10);

            return View(documents);

        }

        // GET: Documents/Details/5

        public async Task<IActionResult> Details(int? id)

        {

            if (id == null)

            {

                return NotFound();

            }

            var document = await _documentService.Get(id.Value);

            if (document == null)

            {

                return NotFound();

            }

            return View(document);

        }

        // GET: Documents/Create

        public IActionResult Create()

        {

            return View();

        }

        // POST: Documents/Create

        [HttpPost]

        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create([Bind("Id,Type")] Document document)

        {

            if (ModelState.IsValid)

            {

                await _documentService.Save(document);

                return RedirectToAction(nameof(Index));

            }

            return View(document);

        }

        // GET: Documents/Edit/5

        public async Task<IActionResult> Edit(int? id)

        {

            if (id == null)

            {

                return NotFound();

            }

            var document = await _documentService.Get(id.Value);

            if (document == null)

            {

                return NotFound();

            }

            return View(document);

        }

        // POST: Documents/Edit/5

        [HttpPost]

        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(int id, [Bind("Id,Type")] Document document)

        {

            if (id != document.Id)

            {

                return NotFound();

            }

            if (ModelState.IsValid)

            {

                try

                {

                    await _documentService.Save(document);

                }

                catch (DbUpdateConcurrencyException)

                {

                    if (!DocumentExists(document.Id))

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

        // GET: Documents/Delete/5

        public async Task<IActionResult> Delete(int? id)

        {

            if (id == null)

            {

                return NotFound();

            }

            var document = await _documentService.Get(id.Value);

            if (document == null)

            {

                return NotFound();

            }

            return View(document);

        }

        // POST: Documents/Delete/5

        [HttpPost, ActionName("Delete")]

        [ValidateAntiForgeryToken]

        public async Task<IActionResult> DeleteConfirmed(int id)

        {

            await _documentService.Delete(id);

            return RedirectToAction(nameof(Index));

        }

        public bool DocumentExists(int id)

        {

            var document = _documentService.Get(id).Result;

            return document != null;

        }

    }

}
