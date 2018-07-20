using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Data
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }

        public decimal Total { get; set; }
        public string Status { get; set; }

        // User info
        public string CustomerAddress { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhoneNumber { get; set; }

        public virtual List<Order> Orders { get; set; }
    }
}
