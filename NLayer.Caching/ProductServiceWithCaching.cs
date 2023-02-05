using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using NLayer.Core;
using NLayer.Core.DTOs;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using System.Linq.Expressions;

namespace NLayer.Caching
{
    public class ProductServiceWithCaching : IProductService
    {
        private const string CacheProductKey = "ProductsCache";
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly IProductRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public ProductServiceWithCaching(IMapper mapper, IMemoryCache cache, IProductRepository repository, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _cache = cache;
            _repository = repository;
            _unitOfWork = unitOfWork;
            if (!cache.TryGetValue(CacheProductKey, out _))
            {
                cache.Set(CacheProductKey, _repository.GetProductsWithCategoryAsync().Result);
            }
        }

        public async Task<Product> AddAsync(Product entity)
        {
            await _repository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            await CacheAllProductsAsync();
            return entity;
        }

        public async Task<IEnumerable<Product>> AddRangeAsync(IEnumerable<Product> entities)
        {
            await _repository.AddRangeAsync(entities);
            await _unitOfWork.CommitAsync();
            await CacheAllProductsAsync();
            return entities;
        }

        public async Task<bool> AnyAsync(Expression<Func<Product, bool>> expression)
        {
            return await Task.FromResult(_cache.Get<List<Product>>(CacheProductKey).Any(expression.Compile()));
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await Task.FromResult(_cache.Get<IEnumerable<Product>>(CacheProductKey));
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            Product? result = await Task.FromResult(_cache.Get<List<Product>>(CacheProductKey).SingleOrDefault(x => x.Id == id));
            return result;
        }

        public async Task<CustomResponseDto<List<ProductWithCategoryDto>>> GetProductsWithCategoryAsync()
        {
            var products = _cache.Get<IEnumerable<Product>>(CacheProductKey);
            var productsWithCategory = _mapper.Map<List<ProductWithCategoryDto>>(products);
            return await Task.FromResult(CustomResponseDto<List<ProductWithCategoryDto>>.Success(200, productsWithCategory));
        }

        public async Task RemoveAsync(Product entity)
        {
            _repository.Remove(entity);
            await _unitOfWork.CommitAsync();
            await CacheAllProductsAsync();
        }

        public async Task RemoveRangeAsync(IEnumerable<Product> entities)
        {
            _repository.RemoveRange(entities);
            await _unitOfWork.CommitAsync();
            await CacheAllProductsAsync();
        }

        public async Task UpdateAsync(Product entity)
        {
            _repository.Update(entity);
            await _unitOfWork.CommitAsync();
            await CacheAllProductsAsync();
        }

        public IQueryable<Product> Where(Expression<Func<Product, bool>> expression)
        {
            return _cache.Get<List<Product>>(CacheProductKey).Where(expression.Compile()).AsQueryable();
        }

        public async Task CacheAllProductsAsync()
        {
            _cache.Set(CacheProductKey, await _repository.GetProductsWithCategoryAsync());
        }
    }
}
