using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Data {
    public class CategoryProduct {
        public int Id { get; set; }
        public string Name { get; set; }

        public int GroupProductId { get; set; }
        [ForeignKey("GroupProductId")]
        public virtual GroupProduct GroupProduct { get; set; }
    }
}
