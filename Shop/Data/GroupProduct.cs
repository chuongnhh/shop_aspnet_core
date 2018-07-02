using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Data {
    public class GroupProduct {
        public GroupProduct() {
            CategoryProducts = new List<CategoryProduct>();
        }
        [Key]
        public int Id { get; set; }
        [Display(Name="Group Name")]
        public string Name { get; set; }
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }
        [Display(Name = "Modified Date")]
        public DateTime ModifiedDate { get; set; }

        public virtual IList<CategoryProduct> CategoryProducts { get; set; }
    }
}
