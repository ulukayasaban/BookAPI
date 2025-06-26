using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Book.API.Dto;
using MediatR;

namespace Book.API.Application.Products.Commands
{
    public class CreateProductCommand : IRequest<ProductDto>
    {
        public ProductDto Product { get; }

        public CreateProductCommand(ProductDto product)
        {
            Product = product;
        }
    }
}