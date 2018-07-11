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

    public struct trendSctruct
    {
        public int CEUsum;
        public DateTime consumed;
    }

    public class HomeController : Controller
    {
        //private CoffeeTrackerContext coffeeContext;

        private readonly CoffeeTrackerContext _context; //Instantiates the context
        public HomeController(CoffeeTrackerContext context) //assigns the context to this controller.
        {
            this._context = context;
        }

        //The class the line dataset is made from
        public class lineDataPair {
            public decimal CEUsum;
            public DateTime ConsumedDate;
        }

        public IActionResult Index()
        {

            var allcoffee = (from cof in _context.Coffee
                          orderby cof.recorded descending
                          select cof);


            //Create line dataset trend
            int lineDaysBack = 100;
            ViewBag.trendDays = lineDaysBack;
            List<lineDataPair> lineDataSet = new List<lineDataPair>();


            var CEUandDays = from c in allcoffee
                             where c.consumed.Date > DateTime.Now.AddDays(-lineDaysBack)
                             select new
                             {
                                 ID = c.ID,
                                 CEU = c.CEU,
                                 Consumed = c.consumed.Date
                             };

            var distinctDates = (from c in CEUandDays
                                 where c.Consumed.Date > DateTime.Now.AddDays(-lineDaysBack)
                                 select c.Consumed.Date).Distinct();

            //stack the data into pairs, then into the list
            foreach (var DD in distinctDates)
            {
                var thisSum = (from c in CEUandDays
                               where c.Consumed.Date == DD
                               select c.CEU).Sum(); 


                lineDataPair thisPair = new lineDataPair();
                thisPair.CEUsum = thisSum;
                thisPair.ConsumedDate = DD;
                lineDataSet.Add(thisPair);
            }

            string lineset = "[";
            foreach (var vds in lineDataSet)
            {
                lineset = lineset + @"{x: '" + vds.ConsumedDate + "', ";
                lineset = lineset + @"y: " + vds.CEUsum + "},";
            }
            lineset = lineset + "]";
            ViewBag.lineSet = lineset;


            //Create lineLabels
            string lineLabels = "[";
            foreach (var lsc in lineDataSet)
            {
                lineLabels = lineLabels + @"'" + lsc.ConsumedDate.ToShortDateString() + "',";
            }
            lineLabels = lineLabels + "]";
            ViewBag.lineLabels = lineLabels;


            //----------------------------------------

            //get totals for the pie chart
            var icedTotal = (from icedCof in _context.Coffee
                             where icedCof.iced == true
                             select icedCof).Count();

            var hotTotal = (from hotCof in _context.Coffee
                             where hotCof.iced == false
                             select hotCof).Count();

            //----------------------------------------


            //get totals by day of week for the bar chart. 
            //int[] CountsByDay = new int[7];
            //foreach(Coffee c in allcoffee)
            //{
            //    CountsByDay[(int)c.consumed.DayOfWeek] += 1;
            //    //Console.WriteLine(c.consumed.DayOfWeek);
            //}

            decimal[] CEUCountsByDay = new decimal[7];
            
            foreach (Coffee c in allcoffee)
            {
                CEUCountsByDay[(int)c.consumed.DayOfWeek] += c.CEU;
            }
            string CEUCountsByDayJSON2 = "[";
            for (int c = 0; c < CEUCountsByDay.Count(); c++)
            {
                CEUCountsByDayJSON2 = CEUCountsByDayJSON2 +  +CEUCountsByDay[c] + ",";
            }
            CEUCountsByDayJSON2 = CEUCountsByDayJSON2 + "]";

            //----------------------------------------


            ViewBag.CEUCountsByDayJSON2 = CEUCountsByDayJSON2;
            //ViewBag.CountsByDay = CountsByDay;
            //ViewBag.CEUCountsByDay = CEUCountsByDay;
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
