using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Data
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Product Name")]
        public string Name { get; set; }
        [Display(Name = "Price")]
        public double Price { get; set; }
        [Display(Name = "Qantity")]
        public int Quantity { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Display")]
        public string Display { get; set; }

        [Display(Name = "Image Url")]
        public string ImageUrl { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }
        [Display(Name = "Modified Date")]
        public DateTime ModifiedDate { get; set; }

        [Display(Name="Category Product")]
        public int CategoryProductId { get; set; }
        public virtual CategoryProduct CategoryProduct { get; set; }
    }
}
