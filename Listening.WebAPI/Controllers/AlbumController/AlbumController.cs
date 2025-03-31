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
        [Route("{name}")]
        public async Task<ActionResult<AlbumModel[]>> FindAlbumsByCategoryName(string name)
        {
            var albumModels = await _memoryCache.GetOrCreateAsync($"AlbumModels_FindByCategoryName_{name}", async (e) =>
            {

                e.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(Random.Shared.Next(5, 10));
                e.SlidingExpiration = TimeSpan.FromMinutes(1);

                return AlbumModel.Create(await _listeningRepository.GetAllAlbumByCategoryNameAsync(name));


            });

            if (albumModels == null)
            {
                return NotFound();
            }

            return albumModels;

        }

    }
}
