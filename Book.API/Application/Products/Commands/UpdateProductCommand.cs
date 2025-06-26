using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Book.API.Dto;
using MediatR;

namespace Book.API.Application.Products.Commands
{
    public class UpdateProductCommand : IRequest<bool>
    {
        public ProductDto Product { get; }

        public UpdateProductCommand(ProductDto product)
        {
            Product = product;
        }
    }
}