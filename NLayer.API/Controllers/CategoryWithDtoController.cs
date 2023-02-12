using MethodTimer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLayer.Core;
using NLayer.Core.DTOs;
using NLayer.Core.Services;

namespace NLayer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryWithDtoController : CustomBaseController
    {
        private readonly IServiceWithDto<Category, CategoryDto> _categoryService;

        public CategoryWithDtoController(IServiceWithDto<Category, CategoryDto> categoryService)
        {
            _categoryService = categoryService;
        }

        [Time]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return CreateActionResult(await _categoryService.GetAllAsync());
        }

        [Time]
        [HttpPost]
        public async Task<IActionResult> Save(CategoryDto categoryDto)
        {
            return CreateActionResult(await _categoryService.AddAsync(categoryDto));
        }
    }
}
