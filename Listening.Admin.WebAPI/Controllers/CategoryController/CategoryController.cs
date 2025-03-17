
using Listening.Admin.WebAPI.Controllers.AlbumController;
using Listening.Domain;
using Listening.Domain.Entity;
using Listening.Infrastructure;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using ZHZ.UnitOkWork;

namespace Listening.Admin.WebAPI.Controllers.CategoryController
{
    [Route("api/[controller]/[action]")]
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

        [HttpPost]
        [UnitOfWork(typeof(ListeningDbContext))]
        public async Task<ActionResult> AddCategory([FromBody] AddCategoryRequest request)
        {
    


            Category category = new Category(request.CategoryName, request.ShowIndex);
            

            _dbCtx.Categories.Add(category);

         

            return Ok("添加成功");

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
