using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Book.API.Application.Categories.Commands;
using Book.API.Infrastructure.Repositories;
using MediatR;

namespace Book.API.Application.Categories.Handlers
{
    public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, bool>
    {
        private readonly ICategoryRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateCategoryHandler> _logger;

        public UpdateCategoryHandler(ICategoryRepository repository, IMapper mapper, ILogger<UpdateCategoryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<bool> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Category;
            _logger.LogInformation("Kategori güncelleniyor: {@Category}", dto);

            var existing = await _repository.GetByIdAsync(dto.Id);
            if (existing is null)
            {
                _logger.LogWarning("Güncelleme başarısız, kategori bulunamadı: {CategoryId}", dto.Id);
                return false;
            }

            _mapper.Map(dto, existing);
            _repository.Update(existing);
            await _repository.SaveAsync();
            return true;
        }
    }
}