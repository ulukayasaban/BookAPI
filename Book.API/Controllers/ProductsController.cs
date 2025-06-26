using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Book.API.Dto;
using Book.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Book.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;
        private readonly ILogger<ProductsController> _logger; // ✅ logger tanımı

        public ProductsController(ProductService productService, ILogger<ProductsController> logger)
        {
            _productService = productService;
            _logger = logger;
        }
        // GET: api/products

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Tüm ürünler API üzerinden isteniyor.");
            var products = await _productService.GetAllAsync();
            return Ok(products);
        }

            // GET: api/products/{id}
            [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("Ürün detay isteniyor, ID: {ProductId}", id);
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
            {
                _logger.LogWarning("Ürün bulunamadı, ID: {ProductId}", id);
                return NotFound();
            }

            return Ok(product);
        }
        // POST: api/products

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductDto dto)
        {
            _logger.LogInformation("Yeni ürün oluşturma isteği: {@Product}", dto);
            await _productService.AddAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = dto.ProductId }, dto);
        }
        // PUT: api/products/{id}

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductDto dto)
        {
            if (id != dto.ProductId)
            {
                _logger.LogWarning("ID uyuşmazlığı. Route ID: {RouteId}, Body ID: {BodyId}", id, dto.ProductId);
                return BadRequest("ID uyuşmazlıgı.");
            }

            var result = await _productService.UpdateAsync(dto);
            if (!result)
            {
                _logger.LogWarning("Güncelleme yapılamadı, ürün bulunamadı: {ProductId}", id);
                return NotFound();
            }

            return NoContent();
        }
        // DELETE: api/products/{id}

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Ürün silme isteği alındı, ID: {ProductId}", id);
            var result = await _productService.DeleteAsync(id);
            if (!result)
            {
                _logger.LogWarning("Silme işlemi başarısız, ürün yok: {ProductId}", id);
                return NotFound();
            }

            return NoContent();
        }
    }
}