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
    public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, List<ProductDto>>
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllProductsHandler> _logger;

        public GetAllProductsHandler(IProductRepository repository, IMapper mapper, ILogger<GetAllProductsHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Tüm ürünler getiriliyor.");
            var products = await _repository.GetAllAsync();
            return _mapper.Map<List<ProductDto>>(products);
        }
    }
}