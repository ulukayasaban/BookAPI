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
    public class CategoriesController : ControllerBase
    {
        private readonly CategoryService _categoryService;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(CategoryService categoryService, ILogger<CategoriesController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        // GET: api/categories
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Tüm kategoriler API'den isteniyor.");
            var categories = await _categoryService.GetAllAsync();
            return Ok(categories);
        }

        // GET: api/categories/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("Kategori detay API isteği: {CategoryId}", id);
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null)
            {
                _logger.LogWarning("Kategori bulunamadı: {CategoryId}", id);
                return NotFound();
            }

            return Ok(category);
        }

        // POST: api/categories
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryDto dto)
        {
            _logger.LogInformation("Yeni kategori oluşturuluyor: {@Category}", dto);
            await _categoryService.AddAsync(dto);
            return Ok(dto);
        }

        // PUT: api/categories/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CategoryDto dto)
        {
            if (id != dto.Id)
            {
                _logger.LogWarning("ID uyuşmazlığı. Route: {RouteId}, Body: {BodyId}", id, dto.Id);
                return BadRequest("ID uyuşmazlığı.");
            }

            var result = await _categoryService.UpdateAsync(dto);
            if (!result)
            {
                _logger.LogWarning("Kategori bulunamadı, güncellenemedi: {CategoryId}", id);
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/categories/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Kategori silme isteği alındı: {CategoryId}", id);
            var result = await _categoryService.DeleteAsync(id);
            if (!result)
            {
                _logger.LogWarning("Kategori silinemedi, bulunamadı: {CategoryId}", id);
                return NotFound();
            }

            return NoContent();
        }
    
    }
}