﻿using System;
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
        public string Name { get; set; }

        public virtual IList<CategoryProduct> CategoryProducts { get; set; }
    }
}