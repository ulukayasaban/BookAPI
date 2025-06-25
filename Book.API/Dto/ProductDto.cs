using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Book.API.Dto
{
    public class ProductDto
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }=null!;
        public decimal Price { get; set; }
    }
}