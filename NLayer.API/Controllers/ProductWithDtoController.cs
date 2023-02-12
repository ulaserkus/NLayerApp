using MethodTimer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLayer.API.Filters;
using NLayer.Core.DTOs;
using NLayer.Core;
using NLayer.Core.Services;

namespace NLayer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductWithDtoController : CustomBaseController
    {
        private readonly IProductServiceWithDto _productService;

        public ProductWithDtoController(IProductServiceWithDto productService)
        {
            _productService = productService;
        }

        [Time]
        [HttpGet]
        public async Task<IActionResult> All()
        {
            return CreateActionResult(await _productService.GetAllAsync());
        }

        [Time]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetProductsWithCategory()
        {
            return CreateActionResult(await _productService.GetProductsWithCategoryAsync());
        }

        [Time]
        [ServiceFilter(typeof(NotFoundFilterAttribute<Product>))]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return CreateActionResult(await _productService.GetByIdAsync(id));
        }

        [Time]
        [HttpPost]
        public async Task<IActionResult> Save(ProductDto productDto)
        {
            return CreateActionResult(await _productService.AddAsync(productDto));
        }

        [Time]
        [HttpPut]
        public async Task<IActionResult> Update(ProductUpdateDto productDto)
        {
            return CreateActionResult(await _productService.UpdateAsync(productDto));
        }

        [Time]
        [ServiceFilter(typeof(NotFoundFilterAttribute<Product>))]
        [HttpDelete]
        public async Task<IActionResult> Remove(int id)
        {
            return CreateActionResult(await _productService.RemoveAsync(id));
        }

        [Time]
        [HttpPost("SaveAll")]
        public async Task<IActionResult> Save(List<ProductDto> productDtos)
        {
            return CreateActionResult(await _productService.AddRangeAsync(productDtos));
        }

        [Time]
        [HttpDelete("RemoveAll")]
        public async Task<IActionResult> Save(List<int> ids)
        {
            return CreateActionResult(await _productService.RemoveRangeAsync(ids));
        }


        [Time]
        [HttpGet("Any/{id}")]
        public async Task<IActionResult> Any(int id)
        {
            return CreateActionResult(await _productService.AnyAsync(x => x.Id == id));
        }
    }
}
