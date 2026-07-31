using FluentValidation;
using StoreApi.DTOs;
using StoreApi.Mappers;
using StoreApi.Repositories;

namespace StoreApi.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly IValidator<CreateProductDto> _validator;

        public ProductService(IProductRepository repository,IValidator<CreateProductDto> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<bool> Add(CreateProductDto dto)
        {
            var validationResult = await _validator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            var product = ProductMapper.ToEntity(dto);
            await _repository.AddAsync(product);
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            if(product == null)
                return false;
            await _repository.DeleteAsync(product);
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProducts()
        {
            var products = await _repository.GetAllAsync();
            return products.Select(ProductMapper.ToDto);
        }

        public async Task<ProductDto?> GetProductById(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null)
            {
                return null;
            }
            return ProductMapper.ToDto(product);
        }

        public async Task<bool> Update(int id,UpdateProductDto dto)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null)
                return false;
            
            ProductMapper.UpdateEntity(product, dto);
            await _repository.SaveChangesAsync();

            return true;
          
        }

    }
}
