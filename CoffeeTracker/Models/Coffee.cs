using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace CoffeeTracker.Models
{
    public class Coffee
    {
        [Key]
        public int ID { get; set; }
        public DateTime recorded { get; set; }
        public DateTime consumed { get; set; }
        [Required]
        public Decimal CEU { get; set; }
        public Boolean iced { get; set; }
    }
}
