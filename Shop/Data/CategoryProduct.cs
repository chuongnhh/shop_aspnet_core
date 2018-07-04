using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Data {
    public class CategoryProduct {


        public CategoryProduct() {
            Products = new List<Product>();
        }
        [Key]
        public int Id { get; set; }
        [Display(Name = "Category Name")]
        public string Name { get; set; }
        [Display(Name = "Created Date")]

        public DateTime CreatedDate { get; set; }
        [Display(Name = "Modified Date")]
        public DateTime ModifiedDate { get; set; }

        [Display(Name = "Group Product")]

        public int GroupProductId { get; set; }
        [ForeignKey("GroupProductId")]
        public virtual GroupProduct GroupProduct { get; set; }

        public virtual IList<Product> Products { get; set; }
    }
}
