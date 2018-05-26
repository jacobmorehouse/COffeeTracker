using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CoffeeTracker.Models
{
    public class CoffeeTrackerContext : DbContext
    {
        public CoffeeTrackerContext (DbContextOptions<CoffeeTrackerContext> options)
            : base(options)
        {
        }

        public DbSet<CoffeeTracker.Models.Coffee> Coffee { get; set; }
    }
}
