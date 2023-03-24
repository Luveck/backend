﻿using AutoMapper;
using Luveck.Service.Administration.Data;
using Luveck.Service.Administration.Models.Dto;
using Luveck.Service.Administration.Repository.IRepository;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Luveck.Service.Administration.Models;
using System;
using Luveck.Service.Administration.UnitWork;
using Luveck.Service.Administration.DTO.Response;
using Luveck.Service.Administration.DTO;
using Luveck.Service.Administration.Utils.Exceptions;
using Luveck.Service.Administration.Utils.Resource;
using System.Xml.Linq;
using Microsoft.Extensions.Configuration;

namespace Luveck.Service.Administration.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration configuration;
        private readonly IRestService restService;

        public ProductRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork= unitOfWork;
        }

        public async Task<ProductResponseDto> CreateProduct(ProductRequestDto productDto, string user)
        {
            try
            {
                var category = await _unitOfWork.CategoryRepository.Find(x => x.Id == productDto.IdCategory);
                if(category == null) throw new BusinessException(GeneralMessage.CategoryNoExist);

                var product = await _unitOfWork.ProductRepository.Find(x => x.Name.ToLower() == productDto.Name.ToLower());
                if (product != null) throw new BusinessException(GeneralMessage.ProductExist);

                Product newProduct = new Product()
                {
                    Name = productDto.Name,
                    CreationDate = DateTime.Now,
                    CreateBy = user,
                    categoryId = category.Id,
                    Barcode = productDto.Barcode,
                    Cost = productDto.Cost,
                    Description = productDto.Description,
                    presentation = productDto.presentation,
                    Quantity = productDto.Quantity,
                    state = true,
                    TypeSell = productDto.TypeSell,
                    UpdateBy = user,
                    UpdateDate = DateTime.Now,
                    UrlOficial = productDto.urlOficial
                };
                await _unitOfWork.ProductRepository.InsertAsync(newProduct);
                await _unitOfWork.SaveAsync();

                product = await _unitOfWork.ProductRepository.Find(x => x.Name.ToLower() == productDto.Name.ToLower());

                return new ProductResponseDto()
                {
                    Cost= product.Cost,
                    CreateBy= product.CreateBy,
                    CreationDate= product.CreationDate,
                    Description = product.Description,
                    Id = product.Id,
                    Barcode = product.Barcode,
                    IdCategory = category.Id,
                    state = product.state,
                    Name= product.Name,
                    NameCategory= category.Name,
                    presentation= product.presentation,
                    Quantity= product.Quantity,
                    TypeSell= product.TypeSell,
                    UpdateBy = product.UpdateBy,
                    UpdateDate = product.UpdateDate
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ProductResponseDto> UpdateProduct(ProductRequestDto productDto, string user)
        {
            try
            {
                var category = await _unitOfWork.CategoryRepository.Find(x => x.Id == productDto.IdCategory);
                if (category == null) throw new BusinessException(GeneralMessage.CategoryNoExist);
                var productExist = await _unitOfWork.ProductRepository.Find(x => x.Id == productDto.Id);
                if (productExist == null) throw new BusinessException(GeneralMessage.ProductNoExist);
                if (!productExist.Name.ToLower().Equals(productDto.Name.ToLower()))
                {
                    var product = await _unitOfWork.ProductRepository.Find(x => x.Name.ToLower() == productDto.Name.ToLower());
                    if(product != null) throw new BusinessException(GeneralMessage.ProductExist);
                }

                productExist.presentation = productDto.presentation;
                productExist.Quantity = productDto.Quantity;
                productExist.TypeSell = productDto.TypeSell;
                productExist.UpdateBy = user;
                productExist.Barcode = productDto.Barcode;
                productExist.UpdateDate = DateTime.Now;
                productExist.Name = productDto.Name;
                productExist.Description = productDto.Description;
                productExist.category = category;
                productExist.Cost = productDto.Cost;
                productExist.state = productDto.state;
                productExist.UrlOficial = productDto.urlOficial;
                _unitOfWork.ProductRepository.Update(productExist);

                await _unitOfWork.SaveAsync();

                return new ProductResponseDto()
                {
                    Cost = productExist.Cost,
                    CreateBy = productExist.CreateBy,
                    CreationDate = productExist.CreationDate,
                    Description = productDto.Description,
                    Id = productDto.Id,
                    Barcode = productExist.Barcode,
                    IdCategory = category.Id,
                    state = productExist.state,
                    Name = productExist.Name,
                    NameCategory = category.Name,
                    presentation = productExist.presentation,
                    Quantity = productExist.Quantity,
                    TypeSell = productExist.TypeSell,
                    UpdateBy = productExist.UpdateBy,
                    UpdateDate = productExist.UpdateDate,
                    urlOficial = productExist.UrlOficial
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> deleteProduct(int id, string user)
        {
            try
            {
                var product = await _unitOfWork.ProductRepository.Find(x => x.Id == id);
                if(product == null) throw new BusinessException(GeneralMessage.CategoryNoExist);

                product.state = false;
                _unitOfWork.ProductRepository.Update(product);
                await _unitOfWork.SaveAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ProductResponseDto> GetProductById(int id)
        {
            var product = await (from prod in _unitOfWork.ProductRepository.AsQueryable()
                                 join cat in _unitOfWork.CategoryRepository.AsQueryable() on prod.category.Id equals cat.Id
                                 where prod.Id == id select new ProductResponseDto()
                                 {
                                     Cost = prod.Cost,
                                     CreateBy = prod.CreateBy,
                                     CreationDate = prod.CreationDate,
                                     Description = prod.Description,
                                     Id = prod.Id,
                                     Barcode = prod.Barcode,
                                     IdCategory = cat.Id,
                                     state = prod.state,
                                     Name = prod.Name,
                                     NameCategory = cat.Name,
                                     presentation = prod.presentation,
                                     Quantity = prod.Quantity,
                                     TypeSell = prod.TypeSell,
                                     UpdateBy = prod.UpdateBy,
                                     UpdateDate = prod.UpdateDate,
                                     urlOficial = prod.UrlOficial
                                 }).FirstOrDefaultAsync();
            return product;
        }

        public async Task<ProductResponseDto> GetProductByName(string name)
        {
            var product = await (from prod in _unitOfWork.ProductRepository.AsQueryable()
                                 join cat in _unitOfWork.CategoryRepository.AsQueryable() on prod.category.Id equals cat.Id
                                 where prod.Name.ToUpper().Equals(name.ToUpper())
                                 select new ProductResponseDto()
                                 {
                                     Cost = prod.Cost,
                                     CreateBy = prod.CreateBy,
                                     CreationDate = prod.CreationDate,
                                     Description = prod.Description,
                                     Id = prod.Id,
                                     Barcode = prod.Barcode,
                                     IdCategory = cat.Id,
                                     state = prod.state,
                                     Name = prod.Name,
                                     NameCategory = cat.Name,
                                     presentation = prod.presentation,
                                     Quantity = prod.Quantity,
                                     TypeSell = prod.TypeSell,
                                     UpdateBy = prod.UpdateBy,
                                     UpdateDate = prod.UpdateDate,
                                     urlOficial = prod.UrlOficial
                                 }).FirstOrDefaultAsync();
            return product;
        }
        public async Task<ProductResponseDto> GetProductByBarcode(string barcode)
        {
            var product = await (from prod in _unitOfWork.ProductRepository.AsQueryable()
                                 join cat in _unitOfWork.CategoryRepository.AsQueryable() on prod.category.Id equals cat.Id
                                 where prod.Barcode.ToUpper().Equals(barcode.ToUpper())
                                 select new ProductResponseDto()
                                 {
                                     Cost = prod.Cost,
                                     CreateBy = prod.CreateBy,
                                     CreationDate = prod.CreationDate,
                                     Description = prod.Description,
                                     Id = prod.Id,
                                     Barcode = prod.Barcode,
                                     IdCategory = cat.Id,
                                     state = prod.state,
                                     Name = prod.Name,
                                     NameCategory = cat.Name,
                                     presentation = prod.presentation,
                                     Quantity = prod.Quantity,
                                     TypeSell = prod.TypeSell,
                                     UpdateBy = prod.UpdateBy,
                                     UpdateDate = prod.UpdateDate,
                                     urlOficial = prod.UrlOficial
                                 }).FirstOrDefaultAsync();
            return product;
        }

        public async Task<List<ProductResponseDto>> GetProducts()
        {
            List < ProductResponseDto > lst = await (from prod in _unitOfWork.ProductRepository.AsQueryable()
                                                     join cat in _unitOfWork.CategoryRepository.AsQueryable() on prod.category.Id equals cat.Id
                                                     select new ProductResponseDto
                                                     {
                                                         Id= prod.Id,
                                                         Cost= prod.Cost,
                                                         CreateBy= prod.CreateBy,
                                                         CreationDate = prod.CreationDate,
                                                         Description = prod.Description,
                                                         Name= prod.Name,
                                                         presentation = prod.presentation,
                                                         Quantity= prod.Quantity,
                                                         state= prod.state,
                                                         Barcode = prod.Barcode,
                                                         TypeSell = prod.TypeSell,
                                                         UpdateBy= prod.UpdateBy,
                                                         UpdateDate = prod.UpdateDate,
                                                         IdCategory = cat.Id,
                                                         NameCategory = cat.Name,
                                                         urlOficial = prod.UrlOficial
                                                     } ).ToListAsync();
            return lst;
        }

        public async Task<List<ProductResponseDto>> GetProductsByCategory(int idCategory)
        {
            List<ProductResponseDto> lst = await(from prod in _unitOfWork.ProductRepository.AsQueryable()
                                                 join cat in _unitOfWork.CategoryRepository.AsQueryable() on prod.category.Id equals cat.Id
                                                 where cat.Id == idCategory
                                                 select new ProductResponseDto
                                                 {
                                                     Id = prod.Id,
                                                     Cost = prod.Cost,
                                                     CreateBy = prod.CreateBy,
                                                     CreationDate = prod.CreationDate,
                                                     Description = prod.Description,
                                                     Name = prod.Name,
                                                     presentation = prod.presentation,
                                                     Quantity = prod.Quantity,
                                                     state = prod.state,
                                                     Barcode = prod.Barcode,
                                                     TypeSell = prod.TypeSell,
                                                     UpdateBy = prod.UpdateBy,
                                                     UpdateDate = prod.UpdateDate,
                                                     IdCategory = cat.Id,
                                                     NameCategory = cat.Name,
                                                     urlOficial = prod.UrlOficial
                                                 }).ToListAsync();
            return lst;
        }

        /// <summary>
        /// Cargar imagen en blob storage
        /// </summary>
        /// <param name="requestFile"></param>
        /// <returns></returns>
        private string LoadImage(FileRequestDto requestFile)
        {

            string url = configuration.GetSection("AdminFiles:Url").Value;
            string controller = configuration.GetSection("AdminFiles:AzureBlobStore:Controller").Value;
            string container = configuration.GetSection("AdminFiles:AzureBlobStore:Params:Container").Value;
            string pathBlob = configuration.GetSection("AdminFiles:AzureBlobStore:Params:PathBlob").Value;

            Dictionary<string, string> headers = new Dictionary<string, string>
            {
            };

            BlobDto request = new BlobDto
            {
                ContentsBase64 = requestFile.FileBase64,
                Container = container,
                PathBlob = pathBlob,
                ContentType = ""
            };

            request.BlobName = requestFile.Name;



            ResponseFileAdminDto result = restService.PostRestServiceAsync<ResponseFileAdminDto>(url, controller, "Save", request, headers).Result;
            if (result != null)
            {
                return (result.IsSuccess ? result.Data.BlobName : string.Empty);
            }


            return string.Empty;


        }
    }
}
