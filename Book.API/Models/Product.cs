using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Book.API.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }=null!;
        public decimal Price { get; set; }

        public int CategoryId { get; set; }

        public Category? Category { get; set; }
    }
}