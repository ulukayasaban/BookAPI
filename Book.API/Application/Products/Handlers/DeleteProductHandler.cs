using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Book.API.Application.Products.Commands;
using Book.API.Repositories;
using MediatR;

namespace Book.API.Application.Products.Handlers
{
    public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, bool>
    {
        private readonly IProductRepository _repository;
        private readonly ILogger<DeleteProductHandler> _logger;

        public DeleteProductHandler(IProductRepository repository, ILogger<DeleteProductHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Ürün siliniyor: {ProductId}", request.Id);

            var product = await _repository.GetByIdAsync(request.Id);
            if (product is null)
            {
                _logger.LogWarning("Silme başarısız, ürün bulunamadı: {ProductId}", request.Id);
                return false;
            }

            _repository.Delete(product);
            await _repository.SaveAsync();
            return true;
        }
    }
}