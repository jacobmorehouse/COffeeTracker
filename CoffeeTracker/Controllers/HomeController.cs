using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoffeeTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace CoffeeTracker.Controllers
{
    public class HomeController : Controller
    {
        //private CoffeeTrackerContext coffeeContext;

        private readonly CoffeeTrackerContext _context; //Instantiates the context
        public HomeController(CoffeeTrackerContext context) //assigns the context to this controller.
        {
            this._context = context;
        }

        public IActionResult Index()
        {

            //var context = new CoffeeTrackerContext();
            var topten = (from cof in _context.Coffee
                            orderby cof.recorded descending
                            select cof).Take(10);

            ViewBag.topten = topten;
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
