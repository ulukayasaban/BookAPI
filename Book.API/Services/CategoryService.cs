using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Book.API.Dto;
using Book.API.Models;
using Book.API.Repositories;

namespace Book.API.Services
{
    public class CategoryService
    {
        private readonly ICategoryRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryService> _logger; 

        public CategoryService(ICategoryRepository repository, IMapper mapper, ILogger<CategoryService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            _logger.LogInformation("Tüm kategoriler getiriliyor.");
            var categories = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }

        public async Task<CategoryDto?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Kategori ID ile aranıyor: {CategoryId}", id);
            var category = await _repository.GetByIdAsync(id);
            return category is null ? null : _mapper.Map<CategoryDto>(category);
        }

        public async Task AddAsync(CategoryDto dto)
        {
            _logger.LogInformation("Yeni kategori ekleniyor: {@Category}", dto);
            var category = _mapper.Map<Category>(dto);
            await _repository.AddAsync(category);
            await _repository.SaveAsync();
        }

        public async Task<bool> UpdateAsync(CategoryDto dto)
        {
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

        public async Task<bool> DeleteAsync(int id)
        {
            _logger.LogInformation("Kategori siliniyor: {CategoryId}", id);
            var category = await _repository.GetByIdAsync(id);
            if (category is null)
            {
                _logger.LogWarning("Silme başarısız, kategori bulunamadı: {CategoryId}", id);
                return false;
            }

            _repository.Delete(category);
            await _repository.SaveAsync();
            return true;
        }
    }
}