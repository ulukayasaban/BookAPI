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

        public CategoriesController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: api/categories
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAllAsync();
            return Ok(categories);
        }

        // GET: api/categories/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null)
                return NotFound();

            return Ok(category);
        }

        // POST: api/categories
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryDto dto)
        {
            await _categoryService.AddAsync(dto);
            return Ok(dto);
        }

        // PUT: api/categories/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CategoryDto dto)
        {
            if (id != dto.Id)
                return BadRequest("ID uyuşmazlığı.");

            var result = await _categoryService.UpdateAsync(dto);
            if (!result)
                return NotFound();

            return NoContent();
        }

        // DELETE: api/categories/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _categoryService.DeleteAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
    
    }
}