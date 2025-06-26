using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Book.API.Application.Categories.Queries;
using Book.API.Dto;
using Book.API.Repositories;
using MediatR;

namespace Book.API.Application.Categories.Handlers
{
    public class GetCategoryByIdHandler : IRequestHandler<GetCategoryByIdQuery, CategoryDto?>
    {
        private readonly ICategoryRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetCategoryByIdHandler> _logger;

        public GetCategoryByIdHandler(ICategoryRepository repository, IMapper mapper, ILogger<GetCategoryByIdHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CategoryDto?> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Kategori ID ile getiriliyor: {CategoryId}", request.Id);

            var category = await _repository.GetByIdAsync(request.Id);
            return category is null ? null : _mapper.Map<CategoryDto>(category);
        }
    }
}