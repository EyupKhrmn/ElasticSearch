using ElasticSearch.API.DTOs;
using ElasticSearch.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ElasticSearch.API.Controllers
{
    public class ProductController : BaseController
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }
        

        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductCreateDto productCreateDto)
        {
            return CreateActionResult(await _productService.SaveAsync(productCreateDto));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return CreateActionResult(await _productService.GetAllAsync());
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAll(string id)
        {
            return CreateActionResult(await _productService.GetByIdAsync(id));
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> GetAll(ProductUpdateDto productUpdateDto)
        {
            return CreateActionResult(await _productService.UpdateAsync(productUpdateDto));
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            return CreateActionResult(await _productService.DeleteAsync(id));
        }
    }
}
