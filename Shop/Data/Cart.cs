using Shop.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Data
{
    public class Cart
    {
        public Cart()
        {
            Orders = new List<Order>();
        }
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

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public string TotalPriceCurency()
        {
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");   // try with "en-US"
            string a = this.Orders.Where(x=>x.Buy==true)
                .Sum(x=>x.GetTotal())
                .ToString("#,### đ", cul.NumberFormat);
            return a;
        }
    }
}
