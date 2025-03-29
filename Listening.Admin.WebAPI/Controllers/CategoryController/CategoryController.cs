
using Listening.Admin.WebAPI.Categories;
using Listening.Admin.WebAPI.Controllers.AlbumController;
using Listening.Domain;
using Listening.Domain.Entity;
using Listening.Infrastructure;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using ZHZ.UnitOkWork;

namespace Listening.Admin.WebAPI.Controllers.CategoryController
{
    [Route("[controller]/[action]")]
    [ApiController]
   // [Authorize(Roles = "Admin")]
   [ Authorize]
    
    public class CategoryController : ControllerBase
    {
        private readonly ListeningDbContext _dbCtx;
        private IListeningRepository _listeningRepository;
        private readonly ListeningDomainService _listeningDomainService;
        private readonly IMemoryCache _memoryCache;

        public CategoryController(ListeningDbContext dbCtx, IListeningRepository listeningRepository, ListeningDomainService listeningDomainService, IMemoryCache memoryCache)
        {
            _dbCtx = dbCtx;
            _listeningRepository = listeningRepository;
            _listeningDomainService = listeningDomainService;
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







        //-------------------
        [HttpGet]
        public async Task<Category[]> FindAll()
        {
            return await _listeningRepository.GetAllCategoryAsync();
        }

      

        [HttpPost]
        [UnitOfWork(typeof(ListeningDbContext))]
        public async Task<ActionResult<Guid>> Add(CategoryAddRequest req)
        {
            var category = await _listeningDomainService.AddCategory(req.Name,req.ShowIndex,req.CoverUrl);
            _dbCtx.Add(category);
            return category.Id;
        }

        

        [HttpGet]
        [Route("{categoryName}")]
        [UnitOfWork(typeof(ListeningDbContext))]
        public async Task<ActionResult> DeleteByName(string categoryName)
        {
            var cat = await _listeningRepository.FindCategoryByNameAsync(categoryName);
            if (cat == null)
            {
                //这样做仍然是幂等的，因为“调用N次，确保服务器处于与第一次调用相同的状态。”与响应无关
                return NotFound($"没有Id={categoryName}的Category");
            }
            cat.SoftDelete();//软删除
            return Ok();
        }


    }
}
