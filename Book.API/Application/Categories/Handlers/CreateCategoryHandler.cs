using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Book.API.Application.Categories.Commands;
using Book.API.Dto;
using Book.API.Models;
using Book.API.Repositories;
using MediatR;

namespace Book.API.Application.Categories.Handlers
{
    public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, CategoryDto>
    {
        private readonly ICategoryRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateCategoryHandler> _logger;

        public CreateCategoryHandler(ICategoryRepository repository, IMapper mapper, ILogger<CreateCategoryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CategoryDto> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Yeni kategori ekleniyor: {@Category}", request.Category);

            var entity = _mapper.Map<Category>(request.Category);
            await _repository.AddAsync(entity);
            await _repository.SaveAsync();

            return _mapper.Map<CategoryDto>(entity);
        }
    }
}