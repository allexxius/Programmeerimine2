<<<<<<< HEAD
﻿using KooliProjekt.Models;

=======
﻿ wdusing KooliProjekt.Models;
>>>>>>> 3ab08cc95858c0f3d4ab2d2123111f2da03c6471
using Microsoft.AspNetCore.Mvc;

using System.Diagnostics;

namespace KooliProjekt.Controllers

{

    public class HomeController : Controller

    {

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)

        {

            _logger = logger;

        }

        public IActionResult Index()

        {

            return View();

        }

        [HttpPost]

        public IActionResult Index(IFormFile myFile)

        {

            using (var stream = myFile.OpenReadStream())

            {

                stream.Seek(0, SeekOrigin.End);

            }

            return View();

        }

        public IActionResult Privacy()

        {

            return View();

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]

        public IActionResult Error()

        {

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

        }

    }

}
