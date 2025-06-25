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

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var products = await _productRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            return product is null ? null : _mapper.Map<ProductDto>(product);
        }

        public async Task AddAsync(ProductDto dto)
        {
            var product = _mapper.Map<Product>(dto);
            await _productRepository.AddAsync(product);
            await _productRepository.SaveAsync();
        }

        public async Task<bool> UpdateAsync(ProductDto dto)
        {
            var existingProduct = await _productRepository.GetByIdAsync(dto.ProductId);
            if (existingProduct is null)
                return false;

            // Map updated values from dto to existing entity
            _mapper.Map(dto, existingProduct);
            _productRepository.Update(existingProduct);
            await _productRepository.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product is null)
                return false;

            _productRepository.Delete(product);
            await _productRepository.SaveAsync();
            return true;
        }

        
    }
}