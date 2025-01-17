
﻿using KooliProjekt.Models;

using Microsoft.AspNetCore.Mvc;

using System.Diagnostics;

namespace KooliProjekt.Controllers

{

    public class HomeController : Controller

    {



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
