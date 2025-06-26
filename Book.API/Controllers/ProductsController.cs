using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Book.API.Application.Products.Commands;
using Book.API.Application.Products.Queries;
using Book.API.Dto;
using Book.API.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Book.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;
        private readonly ILogger<ProductsController> _logger; 
        private readonly IMediator _mediator;


        public ProductsController(ProductService productService, ILogger<ProductsController> logger, IMediator mediator)
        {
            _productService = productService;
            _logger = logger;
            _mediator = mediator;
        }
        // GET: api/products

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Tüm ürünler API üzerinden isteniyor.");
            var result = await _mediator.Send(new GetAllProductsQuery());
            return Ok(result);
        }

            // GET: api/products/{id}
            [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("Ürün detay isteniyor, ID: {ProductId}", id);
            var result = await _mediator.Send(new GetProductByIdQuery(id));
            if (result == null)
            {
                _logger.LogWarning("Ürün bulunamadı, ID: {ProductId}", id);
                return NotFound();
            }

            return Ok(result); 
        }
        // POST: api/products

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductDto dto)
        {
            _logger.LogInformation("Yeni ürün oluşturma isteği: {@Product}", dto);
            var result = await _mediator.Send(new CreateProductCommand(dto));
            return CreatedAtAction(nameof(GetById), new { id = result.ProductId }, result);
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

            var result = await _mediator.Send(new UpdateProductCommand(dto));
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
            var result = await _mediator.Send(new DeleteProductCommand(id));
            if (!result)
            {
                _logger.LogWarning("Silme işlemi başarısız, ürün yok: {ProductId}", id);
                return NotFound();
            }

            return NoContent(); 
        }
    }
}