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
using Luveck.Service.Administration.UnitWork;
using Luveck.Service.Administration.DTO.Response;
using Luveck.Service.Administration.DTO;
using Luveck.Service.Administration.Utils.Exceptions;
using Luveck.Service.Administration.Utils.Resource;
using System.Xml.Linq;

namespace Luveck.Service.Administration.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CategoryResponseDto> CreateUpdateCategory(CategoryRequestDto categoryDto, string user)
        {
            var catExist = await _unitOfWork.CategoryRepository.Find(x => x.Name.ToLower().Equals(categoryDto.Name.Trim().ToLower()));
            if (catExist != null) throw new BusinessException(GeneralMessage.CategoryExist);

            try
            {
                if(categoryDto.Id == 0)
                {
                    Category cate = new Category()
                    {
                        Name = categoryDto.Name,
                        CreateBy = user,
                        CreationDate = DateTime.Now,
                        IsDeleted = false,
                        UpdateBy = user,
                        UpdateDate = DateTime.Now,
                    };
                    await _unitOfWork.CategoryRepository.InsertAsync(cate);
                }
                else
                {
                    var category = await _unitOfWork.CategoryRepository.Find(x => x.Id == categoryDto.Id);
                    if(category != null)
                    {
                        if (!category.Name.ToUpper().Equals(categoryDto.Name.ToUpper()))
                        {
                            var name = await _unitOfWork.CategoryRepository.Find(x => x.Name.ToUpper().Equals(categoryDto.Name.ToUpper()));

                            if (name != null) throw new BusinessException(GeneralMessage.CategoryExist);
                        }

                        category.UpdateDate = DateTime.Now;
                        category.UpdateBy = user;
                        category.Name = categoryDto.Name;
                        category.IsDeleted = categoryDto.IsDeleted;
                        
                        _unitOfWork.CategoryRepository.Update(category);                        
                    }

                    else throw new BusinessException(GeneralMessage.CategoryNoExist);
                }

                await _unitOfWork.SaveAsync();

                var cat = await _unitOfWork.CategoryRepository.Find(x => x.Name.ToLower() == categoryDto.Name.ToLower());

                return new CategoryResponseDto()
                {
                    Name = cat.Name,
                    CreateBy = cat.CreateBy,
                    CreationDate = cat.CreationDate,
                    IsDeleted = cat.IsDeleted,
                    Id = cat.Id,
                    UpdateBy = cat.UpdateBy,
                    UpdateDate = cat.UpdateDate
                };

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> deleteCategory(int id, string user)
        {
            try
            {
                var category = await _unitOfWork.CategoryRepository.Find(x => x.Id == id);
                if (category == null) throw new BusinessException(GeneralMessage.CategoryNoExist);

                category.UpdateDate = DateTime.Now;
                category.UpdateBy = user;
                category.IsDeleted = true;

                _unitOfWork.CategoryRepository.Update(category);
                await _unitOfWork.SaveAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<CategoryResponseDto>> GetCategories()
        {
            List<CategoryResponseDto> lst = await _unitOfWork.CategoryRepository.AsQueryable().Select(x => new CategoryResponseDto
            {
                Id = x.Id,
                CreateBy = x.CreateBy,
                CreationDate= x.CreationDate,
                IsDeleted = x.IsDeleted,
                Name = x.Name,
                UpdateBy = x.UpdateBy,
                UpdateDate = x.UpdateDate 
            }).ToListAsync();

            return lst;
        }

        public async Task<CategoryResponseDto> GetCategoryById(int id)
        {
            var category = _unitOfWork.CategoryRepository.Find(x => x.Id == id).Result;

            return new CategoryResponseDto()
            {
                Name = category.Name,
                CreateBy = category.CreateBy,
                CreationDate = category.CreationDate,
                IsDeleted = category.IsDeleted,
                Id = category.Id,
                UpdateBy = category.UpdateBy,
                UpdateDate = category.UpdateDate
            };
        }

        public async Task<CategoryResponseDto> GetCategoryByName(string name)
        {
            var category = _unitOfWork.CategoryRepository.Find(x => x.Name.ToLower() == name.ToLower()).Result;

            return new CategoryResponseDto()
            {
                Name = category.Name,
                CreateBy = category.CreateBy,
                CreationDate = category.CreationDate,
                IsDeleted = category.IsDeleted,
                Id = category.Id,
                UpdateBy = category.UpdateBy,
                UpdateDate = category.UpdateDate
            };
        }
    }
}
