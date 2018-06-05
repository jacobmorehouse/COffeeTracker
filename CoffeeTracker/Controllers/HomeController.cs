using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoffeeTracker.Models;
using Microsoft.EntityFrameworkCore;
using System.Web;

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

            var allcoffee = (from cof in _context.Coffee
                          orderby cof.recorded descending
                          select cof);


            //Create line dataset 
            string lineset = "[";
            foreach (Coffee lsc in allcoffee)
            {
                lineset = lineset + @"{x: '" + lsc.consumed + "', ";
                lineset = lineset + @"y: " + lsc.CEU + "},";
            }
            lineset = lineset + "]";
            ViewBag.lineSet = lineset;

            //Create lineLabels
            string lineLabels = "[";
            foreach (Coffee lsc in allcoffee)
            {
                lineLabels = lineLabels + @"'" + lsc.consumed + "',";
            }
            lineLabels = lineLabels + "]";
            ViewBag.lineLabels = lineLabels;

            //get totals for the pie chart
            var icedTotal = (from icedCof in _context.Coffee
                             where icedCof.iced == true
                             select icedCof).Count();

            var hotTotal = (from hotCof in _context.Coffee
                             where hotCof.iced == false
                             select hotCof).Count();


            //get totals by day of week for the bar chart. 
            var barDays = (from d in _context.Coffee
                           select d);


            ViewBag.hotTotal = hotTotal;
            ViewBag.icedTotal = icedTotal;
            ViewBag.coffeeCount = allcoffee.Count();
            ViewBag.top10 = allcoffee.OrderByDescending(c => c.recorded).Take(10);
            ViewBag.top1000 = allcoffee.OrderByDescending(c => c.recorded).Take(1000);
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
