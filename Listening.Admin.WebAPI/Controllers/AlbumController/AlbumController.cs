

using Listening.Domain;
using Listening.Domain.Entity;
using Listening.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using ZHZ.UnitOkWork;

namespace Listening.Admin.WebAPI.Controllers.AlbumController
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "Admin")]

    public class AlbumController : ControllerBase
    {
        private readonly ListeningDbContext _dbCtx;
        private IListeningRepository _listeningRepository;
        private readonly ListeningDomainService _listeningDomainService;
        private readonly IMemoryCache _memoryCache;

        public AlbumController(ListeningDbContext dbCtx, IListeningRepository listeningRepository, ListeningDomainService listeningDomainService, IMemoryCache memoryCache)
        {
            _dbCtx = dbCtx;
            _listeningRepository = listeningRepository;
            _listeningDomainService = listeningDomainService;
            _memoryCache = memoryCache;
        }

        [UnitOfWork(typeof(ListeningDbContext))]
        [HttpPost]
        public async Task<ActionResult> AddAlbum(AddAlbumRequest request)
        {
            var category = await _listeningRepository.FindCategoryByNameAsync(request.CategoryName);
            if (category == null)
            {
                return BadRequest($"Catrgory_{request.CategoryName}不存在");
            }
            Album album = new Album(request.AlbumName, request.ShowIndex, category!.CategoryName);

            _dbCtx.Albums.Add(album);

            return Ok("添加成功");

        }


        [HttpGet]
        public async Task<ActionResult<AlbumModel>> FindByAlbumName(string albumName)
        {

            var albumModel = await _memoryCache.GetOrCreateAsync($"AlbumModel_FindByAlbumName_{albumName}", async (e) =>
            {

                e.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(Random.Shared.Next(5, 10));
                e.SlidingExpiration = TimeSpan.FromMinutes(1);

                return AlbumModel.Create(await _listeningRepository.FindAlbumByNameAsync(albumName));


            });

            if (albumModel == null)
            {
                return NotFound();
            }

            return albumModel;
        }

        [HttpGet]
        public async Task<ActionResult<AlbumModel[]>> FindAlbumsByCategoryName(string categoryName)
        {
            var albumModels = await _memoryCache.GetOrCreateAsync($"AlbumModels_FindByCategoryName_{categoryName}", async (e) =>
            {

                e.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(Random.Shared.Next(5, 10));
                e.SlidingExpiration = TimeSpan.FromMinutes(1);

                return AlbumModel.Create(await _listeningRepository.GetAllAlbumByCategoryNameAsync(categoryName));


            });

            if (albumModels == null)
            {
                return NotFound();
            }

            return albumModels;

        }


    }
}
