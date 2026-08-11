using AutoMapper;
using FluentValidation;
using StoreApi.Common;
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

        public async Task<Result<ProductDto>> Add(CreateProductDto dto)
        {
            var validationResult = await _validator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            var product = _mapper.Map<Product>(dto);
            product.CreatedAt = DateTime.UtcNow;
            await _repository.AddAsync(product);
            await _repository.SaveChangesAsync();

            var productDto = _mapper.Map<ProductDto>(product);
            return Result<ProductDto>.SuccessResult(
                    productDto,
                    "محصول با موفقیت ایجاد شد"
            );
        }

        public async Task<Result<bool>> Delete(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            if(product == null)
                return Result<bool>.Failure(
                    "محصول مورد نظر پیدا نشد"
                    );
            await _repository.DeleteAsync(product);
            await _repository.SaveChangesAsync();
            return Result<bool>.SuccessResult(
                true,
                "محصول با موفقیت حذف شد"
                );
        }

        public async Task<Result<IEnumerable<ProductDto>>> GetAllProducts()
        {
            var products = await _repository.GetAllAsync();
            var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products);
            return Result<IEnumerable<ProductDto>>.SuccessResult(
                productDtos,
                "محصولات با موفقیت دریافت شدند"
                );
        }

        public async Task<Result<ProductDto>> GetProductById(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null)
            {
                return Result<ProductDto>.Failure(
                    "محصول پیدا نشد"
                );
            }
            var productDto = _mapper.Map<ProductDto>(product);
            return Result<ProductDto>.SuccessResult(
                productDto,
                "محصول با موفقیت دریافت شد"
            );
        }

        public async Task<Result<ProductDto>> Update(int id,UpdateProductDto dto)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null)
                return Result<ProductDto>.Failure(
                    "محصول مورد نظر پیدا نشد"
                    );
            
            //save new data on existing DTO, Not create new Product
           _mapper.Map(dto, product);
            await _repository.SaveChangesAsync();

            var productDto = _mapper.Map<ProductDto>(product);
            return Result<ProductDto>.SuccessResult(
                productDto,
                "محصول با موفقیت بروزرسانی شد"
                );
          
        }

    }
}
