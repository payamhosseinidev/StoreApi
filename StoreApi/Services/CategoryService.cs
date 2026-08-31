using AutoMapper;
using FluentValidation;
using StoreApi.Common;
using StoreApi.DTOs;
using StoreApi.Models;
using StoreApi.Repositories;

namespace StoreApi.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;
        private readonly IValidator<CreateCategoryDto> _validator;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository repository, IValidator<CreateCategoryDto> validator, IMapper mapper)
        {
            _repository = repository;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task<Result<CategoryDto>> CreateAsync(CreateCategoryDto dto)
        {
            var validationResult = await _validator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            var category = _mapper.Map<Category>(dto);
            await _repository.AddAsync(category);
            await _repository.SaveChangesAsync();

            var categoryDto = _mapper.Map<CategoryDto>(category);
            return Result<CategoryDto>.SuccessResult(
                categoryDto,
                "دسته بندی با موفقیت ایجاد شد"
            );
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            var category = await _repository.GetByIdAsync(id);

            if (category is null)
            {
                return Result<bool>.Failure(
                    "دسته‌بندی مورد نظر پیدا نشد"
                );
            }

            try
            {
                await _repository.DeleteAsync(category);
                await _repository.SaveChangesAsync();

                return Result<bool>.SuccessResult(
                    true,
                    "دسته‌بندی با موفقیت حذف شد"
                );
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException)
            {
                return Result<bool>.Failure(
                    "امکان حذف این دسته‌بندی وجود ندارد؛ " +
                    "ابتدا محصولات آن را منتقل کنید."
                );
            }
        }

        public async Task<Result<IEnumerable<CategoryDto>>> GetAllAsync()
        {
            var categories = await _repository.GetAllAsync();
            var categoryDtos = _mapper.Map<IEnumerable<CategoryDto>>(categories);
            return Result<IEnumerable<CategoryDto>>.SuccessResult(
                categoryDtos,
                "دسته بندی ها با موفقیت دریافت شدند"
            );
        }

        public async Task<Result<CategoryDto>> GetByIdAsync(int id)
        {
            var category = await _repository.GetByIdAsync(id);
            if(category is null)
            {
                return Result<CategoryDto>.Failure(
                    "دسته بندی مورد نظر پیدا نشد"
                );
            }
            var categoryDto = _mapper.Map<CategoryDto>(category);
            return Result<CategoryDto>.SuccessResult(
                categoryDto,
                "دسته بندی با موفقیت دریافت شد"
            );
        }

        public async Task<Result<CategoryDto>> UpdateAsync(int id, UpdateCategoryDto dto)
        {
            var category = await _repository.GetByIdAsync(id);
            if (category is null)
                return Result<CategoryDto>.Failure("دسته بندی مورد نظر یافت نشد");

            _mapper.Map(dto, category);
            await _repository.SaveChangesAsync();

            var categoryDto = _mapper.Map<CategoryDto>(category);

            return Result<CategoryDto>.SuccessResult(
                categoryDto,
                "دسته‌بندی با موفقیت بروزرسانی شد"
            );
        }
    }
}
