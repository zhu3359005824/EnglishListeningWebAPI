using Listening.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Listening.Main.WebAPI.Controllers.AlbumController
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AlbumController : ControllerBase
    {
        private readonly IListeningRepository _listeningRepository;
        private readonly IMemoryCache _memoryCache;

        public AlbumController(IListeningRepository listeningRepository, IMemoryCache memoryCache)
        {
            _listeningRepository = listeningRepository;
            _memoryCache = memoryCache;
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
