using Shop.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Data
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }
        public int ProductId { get; set; }
        public int CartId { get; set; }

        public bool Buy { get; set; }

        public int Quantity { get; set; }
      
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        [ForeignKey("CartId")]
        public virtual Cart Cart { get; set; }


        public double GetTotal()
        {
            return Product.Price * Quantity;
        }

        public string TotalPriceCurency()
        {
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");   // try with "en-US"
            string a = GetTotal().ToString("#,### đ", cul.NumberFormat);
            return a;
        }
    }
}
