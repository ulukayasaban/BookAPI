using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Book.API.Application.Products.Commands;
using Book.API.Domain;
using Book.API.Dto;
using Book.API.Infrastructure.Repositories;
using Book.API.Models;
using MediatR;

namespace Book.API.Application.Products.Handlers
{
    public class CreateProductHandler : IRequestHandler<CreateProductCommand, ProductDto>
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateProductHandler> _logger;

        public CreateProductHandler(IProductRepository repository, IMapper mapper, ILogger<CreateProductHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Ürün oluşturuluyor: {@Product}", request.Product);

            var productEntity = _mapper.Map<Product>(request.Product);
            await _repository.AddAsync(productEntity);
            await _repository.SaveAsync();

            return _mapper.Map<ProductDto>(productEntity);
        }
    }
}