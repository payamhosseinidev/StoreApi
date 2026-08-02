using AutoMapper;
using FluentValidation;
using StoreApi.DTOs;
using StoreApi.Mapping;
using StoreApi.Models;
using StoreApi.Repositories;

namespace StoreApi.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly IValidator<CreateProductDto> _validator;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository repository,IValidator<CreateProductDto> validator,IMapper mapper)
        {
            _repository = repository;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task<bool> Add(CreateProductDto dto)
        {
            var validationResult = await _validator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            var product = _mapper.Map<Product>(dto);
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
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<ProductDto?> GetProductById(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null)
            {
                return null;
            }
            return _mapper.Map<ProductDto>(product); ;
        }

        public async Task<bool> Update(int id,UpdateProductDto dto)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null)
                return false;
            
           _mapper.Map(dto, product);
            await _repository.SaveChangesAsync();

            return true;
          
        }

    }
}
