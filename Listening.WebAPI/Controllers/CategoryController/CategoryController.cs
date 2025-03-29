﻿using Listening.Domain;
using Listening.Main.WebAPI.Controllers.AlbumController;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Listening.Main.WebAPI.Controllers.CategoryController
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IListeningRepository _listeningRepository;
        private readonly IMemoryCache _memoryCache;

        public CategoryController(IListeningRepository listeningRepository, IMemoryCache memoryCache)
        {
            _listeningRepository = listeningRepository;
            _memoryCache = memoryCache;
        }


        [HttpGet]
        public async Task<ActionResult<CategoryModel>> FindByCategoryName(string Category)
        {

            var Model = await _memoryCache.GetOrCreateAsync($"CategoryModel_FindByCategoryName_{Category}", async (e) =>
            {

                e.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(Random.Shared.Next(5, 10));
                e.SlidingExpiration = TimeSpan.FromMinutes(1);

                return CategoryModel.Create(await _listeningRepository.FindCategoryByNameAsync(Category));


            });

            if (Model == null)
            {
                return NotFound();
            }

            return Model;
        }


        [HttpGet]
        public async Task<ActionResult<CategoryModel[]>> FindAllCategory()
        {
            var Models = await _memoryCache.GetOrCreateAsync($"CategoryModels_FindAllCategory", async (e) =>
            {

                e.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(Random.Shared.Next(5, 10));
                e.SlidingExpiration = TimeSpan.FromMinutes(1);

                return CategoryModel.Create(await _listeningRepository.GetAllCategoryAsync());


            });

            if (Models == null)
            {
                return NotFound();
            }

            return Models;

        }
    }
}
