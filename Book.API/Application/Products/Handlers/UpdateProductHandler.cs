using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Book.API.Application.Products.Commands;
using Book.API.Repositories;
using MediatR;

namespace Book.API.Application.Products.Handlers
{
    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, bool>
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateProductHandler> _logger;

        public UpdateProductHandler(IProductRepository repository, IMapper mapper, ILogger<UpdateProductHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Product;
            _logger.LogInformation("Ürün güncelleniyor: {@Product}", dto);

            var existing = await _repository.GetByIdAsync(dto.ProductId);
            if (existing is null)
            {
                _logger.LogWarning("Güncelleme başarısız, ürün bulunamadı: {ProductId}", dto.ProductId);
                return false;
            }

            _mapper.Map(dto, existing);
            _repository.Update(existing);
            await _repository.SaveAsync();
            return true;
        }
    }
}