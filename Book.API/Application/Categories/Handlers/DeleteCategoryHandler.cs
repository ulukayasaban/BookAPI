using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Book.API.Application.Categories.Commands;
using Book.API.Repositories;
using MediatR;

namespace Book.API.Application.Categories.Handlers
{
    public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand, bool>
    {
        private readonly ICategoryRepository _repository;
        private readonly ILogger<DeleteCategoryHandler> _logger;

        public DeleteCategoryHandler(ICategoryRepository repository, ILogger<DeleteCategoryHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Kategori siliniyor: {CategoryId}", request.Id);

            var category = await _repository.GetByIdAsync(request.Id);
            if (category is null)
            {
                _logger.LogWarning("Silme başarısız, kategori bulunamadı: {CategoryId}", request.Id);
                return false;
            }

            _repository.Delete(category);
            await _repository.SaveAsync();
            return true;
        }
    }
}