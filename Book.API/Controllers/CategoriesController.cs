using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Book.API.Application.Categories.Commands;
using Book.API.Application.Categories.Queries;
using Book.API.Dto;
using Book.API.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Book.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly CategoryService _categoryService;
        private readonly ILogger<CategoriesController> _logger;

        private readonly IMediator _mediator;
        public CategoriesController(CategoryService categoryService, ILogger<CategoriesController> logger, IMediator mediator)
        {
            _categoryService = categoryService;
            _logger = logger;
            _mediator = mediator;
        }

        // GET: api/categories
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Tüm kategoriler API'den isteniyor.");
            var result = await _mediator.Send(new GetAllCategoriesQuery());
            return Ok(result);
        }

        // GET: api/categories/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("Kategori detay API isteği: {CategoryId}", id);
            var result = await _mediator.Send(new GetCategoryByIdQuery(id));
            if (result == null)
            {
                _logger.LogWarning("Kategori bulunamadı: {CategoryId}", id);
                return NotFound();
            }

            return Ok(result); 
        }

        // POST: api/categories
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryDto dto)
        {
            _logger.LogInformation("Yeni kategori oluşturuluyor: {@Category}", dto);
            var result = await _mediator.Send(new CreateCategoryCommand(dto));
            return Ok(result);
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

            var result = await _mediator.Send(new UpdateCategoryCommand(dto));
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
            var result = await _mediator.Send(new DeleteCategoryCommand(id));
            if (!result)
            {
                _logger.LogWarning("Kategori silinemedi, bulunamadı: {CategoryId}", id);
                return NotFound();
            }

            return NoContent(); 
        }
    
    }
}