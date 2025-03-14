using Listening.Domain;
using Listening.Domain.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Listening.Main.WebAPI.Controllers.AlbumController
{
    [Route("api/[controller]/[action]")]
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
        public async Task<ActionResult<AlbumModel>> FindById(Guid id)
        {

            var albumModel = await _memoryCache.GetOrCreateAsync($"AlbumModel_FindById_{id}", async (e) =>
            {

                e.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(Random.Shared.Next(5, 10));
                e.SlidingExpiration = TimeSpan.FromMinutes(1);

                return AlbumModel.Create(await _listeningRepository.FindAlbumByIdAsync(id));


            });

            if (albumModel == null)
            {
                return NotFound();
            }

            return albumModel;
        }


        //[HttpPost]
        //public async Task<ActionResult> AddAlbum()
        //{

        //}
    }
}
