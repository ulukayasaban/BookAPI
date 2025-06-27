using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Book.API.Application.Products.Queries;
using Book.API.Dto;
using Book.API.Infrastructure.Repositories;
using MediatR;

namespace Book.API.Application.Products.Handlers
{
    public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, ProductDto?>
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetProductByIdHandler> _logger;

        public GetProductByIdHandler(IProductRepository repository, IMapper mapper, ILogger<GetProductByIdHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Ürün detay isteniyor, ID: {ProductId}", request.Id);
            var product = await _repository.GetByIdAsync(request.Id);
            return product is null ? null : _mapper.Map<ProductDto>(product);
        }
    }
}