using AutoMapper;
using Luveck.Service.Administration.Data;
using Luveck.Service.Administration.Models;
using Luveck.Service.Administration.Models.Dto;
using Luveck.Service.Administration.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Luveck.Service.Administration.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _appDbContext;
        private IMapper _mapper;

        public CategoryRepository(AppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }
        public async Task<CategoryDto> CreateUpdateCategory(CategoryDto categoryDto)
        {
            Category category = _mapper.Map<Category>(categoryDto);

            if (category.Id > 0)
            {
                _appDbContext.Category.Update(category);
            }
            else
            {
                _appDbContext.Category.Add(category);
            }

            await _appDbContext.SaveChangesAsync();

            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<bool> deleteCategory(int id)
        {
            try
            {
                Category category = await _appDbContext.Category.FirstOrDefaultAsync(c => c.Id == id);

                if (category == null) return false;

                category.IsDeleted = true;
                _appDbContext.Category.Update(category);
                await _appDbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<CategoryDto>> GetCategories()
        {
            return await (from Category in _appDbContext.Category
                          select (new CategoryDto
                          {
                              Id = Category.Id,
                              Name = Category.Name, 
                              IsDeleted = Category.IsDeleted,
                              CreateBy = Category.CreateBy,
                              CreationDate = Category.CreationDate,
                              UpdateBy = Category.UpdateBy,
                              UpdateDate = Category.UpdateDate
                          })).ToListAsync();
        }

        public async Task<CategoryDto> GetCategory(int id)
        {
            Category category = await _appDbContext.Category.FirstOrDefaultAsync(c => c.Id == id);
            return _mapper.Map<CategoryDto>(category);
        }
    }
}
