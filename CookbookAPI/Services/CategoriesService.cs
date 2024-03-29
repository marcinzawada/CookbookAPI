﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CookbookAPI.Data;
using CookbookAPI.DTOs;
using CookbookAPI.Entities;
using CookbookAPI.Exceptions;
using CookbookAPI.Repositories.Interfaces;
using CookbookAPI.Services.Interfaces;
using CookbookAPI.ViewModels.Areas;
using CookbookAPI.ViewModels.Categories;
using Microsoft.AspNetCore.Authorization;

namespace CookbookAPI.Services
{
    public class CategoriesService : BaseService, ICategoriesService
    {
        private readonly ICategoriesRepository<Category> _categoriesRepository;

        public CategoriesService(CookbookDbContext context, IMapper mapper,
            IUserContextService userContextService, IAuthorizationService authorizationService,
            ICategoriesRepository<Category> categoriesRepository) 
            : base(context, mapper, userContextService, authorizationService)
        {
            _categoriesRepository = categoriesRepository;
        }

        public async Task<GetAllCategoriesVm> GetAll()
        {
            var categories = await _categoriesRepository.GetAll();

            var categoryDtos = _mapper.Map<List<CategoryDto>>(categories);

            return new GetAllCategoriesVm
            {
                Categories = categoryDtos
            };
        }

        public async Task<GetCategoryVm> GetById(int id)
        {
            var area = await _categoriesRepository.GetByIdWithRecipes(id);

            if (area == null)
                throw new NotFoundException($"Category with id:{id} not found");

            var recipeDtos = _mapper.Map<List<BaseRecipeDto>>(area.Recipes);

            return new GetCategoryVm
            {
                Id = area.Id,
                Name = area.Name,
                Recipes = recipeDtos
            };
        }
    }
}
