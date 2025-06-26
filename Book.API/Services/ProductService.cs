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
    public class ProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductService> _logger; 

        public ProductService(IProductRepository productRepository, IMapper mapper, ILogger<ProductService> logger)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            _logger.LogInformation("Tüm ürünler getiriliyor.");
            var products = await _productRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            _logger.LogInformation("ID ile ürün aranıyor: {ProductId}", id);
            var product = await _productRepository.GetByIdAsync(id);
            return product is null ? null : _mapper.Map<ProductDto>(product);
        }

        public async Task AddAsync(ProductDto dto)
        {
            _logger.LogInformation("Yeni ürün ekleniyor: {@Product}", dto);
            var product = _mapper.Map<Product>(dto);
            await _productRepository.AddAsync(product);
            await _productRepository.SaveAsync();
        }

        public async Task<bool> UpdateAsync(ProductDto dto)
        {
            _logger.LogInformation("Ürün güncelleniyor: {@Product}", dto);
            var existingProduct = await _productRepository.GetByIdAsync(dto.ProductId);
            if (existingProduct is null)
            {
                _logger.LogWarning("Güncelleme başarısız, ürün bulunamadı: {ProductId}", dto.ProductId);
                return false;
            }

            _mapper.Map(dto, existingProduct);
            _productRepository.Update(existingProduct);
            await _productRepository.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            _logger.LogInformation("Ürün siliniyor: {ProductId}", id);
            var product = await _productRepository.GetByIdAsync(id);
            if (product is null)
            {
                _logger.LogWarning("Silme başarısız, ürün bulunamadı: {ProductId}", id);
                return false;
            }

            _productRepository.Delete(product);
            await _productRepository.SaveAsync();
            return true;
        }

        
    }
}