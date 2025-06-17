using AutoMapper;
using Contracts.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Product.API.Entities;
using Product.API.Persistence;
using Product.API.Repositories.Intefaces;
using Shared.DTOs.Product;
using System.ComponentModel.DataAnnotations;

namespace Product.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductsController(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var product = await _productRepository.FindAll().ToListAsync();
            var result = _mapper.Map<IEnumerable<ProductDto>>(product);

            return Ok(result);
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetProduct([FromRoute] long id)
        {
            var product = await _productRepository.GetProduct(id);
            if (product == null) return NotFound();

            var result = _mapper.Map<ProductDto>(product);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto createDto)
        {
            var product = _mapper.Map<CatalogProduct>(createDto);
            await _productRepository.CreateProduct(product);
            await _productRepository.SaveChangeAsync();

            var result = _mapper.Map<ProductDto>(product);
            return Ok(result);
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> UpdateProduct([FromRoute] long id, [FromBody] UpdateProductDto updateDto)
        {
            var product = await _productRepository.GetProduct(id);
            if (product == null) return NotFound();

            var updateProduct = _mapper.Map(updateDto, product);
            //var updateProduct = _mapper.Map<CatalogProduct>(updateDto); //Miss data if use this way

            await _productRepository.UpdateProduct(updateProduct);
            await _productRepository.SaveChangeAsync();

            var result = _mapper.Map<ProductDto>(updateProduct);
            return Ok(result);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] long id)
        {
            var product = await _productRepository.GetProduct(id);
            if (product == null) return NotFound();

            await _productRepository.DeleteProduct(id);
            await _productRepository.SaveChangeAsync();
            return NoContent();
        }

        [HttpGet("GetProductByNo/{no}")]
        public async Task<IActionResult> GetProductByNo([FromRoute] string no)
        {
            var product = await _productRepository.GetProductByNo(no);
            if (product == null) return NotFound();

            var result = _mapper.Map<ProductDto>(product);
            return Ok(result);
        }
    }
}
