using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models
{
    public class CartViewModels
    {
        public int Id { get; set; }
        public int ItemsCount { get; set; }
        public double Total { get; set; }

        public string Status { get; set; }

        public string TotalCurency()
        {
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");   // try with "en-US"
            string a = Total.ToString("#,### đ", cul.NumberFormat);
            return a;
        }
    }
}
