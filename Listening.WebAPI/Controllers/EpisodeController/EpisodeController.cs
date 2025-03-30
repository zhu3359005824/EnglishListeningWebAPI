using Listening.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Listening.Main.WebAPI.Controllers.EpisodeController
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class EpisodeController : ControllerBase
    {
        private readonly IListeningRepository _listeningRepository;
        private readonly IMemoryCache _memoryCache;

        public EpisodeController(IListeningRepository listeningRepository, IMemoryCache memoryCache)
        {
            _listeningRepository = listeningRepository;
            _memoryCache = memoryCache;
        }


        [HttpGet]
        public async Task<ActionResult<EpisodeModel>> FindEpisodeByName(string Name)
        {

            var Model = await _memoryCache.GetOrCreateAsync($"EpisodeModel_FindEpisodeByName_{Name}", async (e) =>
            {

                e.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(Random.Shared.Next(5, 10));
                e.SlidingExpiration = TimeSpan.FromMinutes(1);

                return EpisodeModel.Create(await _listeningRepository.FindEpisodeByNameAsync(Name), true);


            });

            if (Model == null)
            {
                return NotFound();
            }

            return Model;
        }


        [HttpGet]
        public async Task<ActionResult<EpisodeModel[]>> FindEpisodesByAlbumName(string albumName)
        {
            var Models = await _memoryCache.GetOrCreateAsync($"EpisodeModels_FindEpisodesByAlbumName_{albumName}", async (e) =>
            {

                e.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(Random.Shared.Next(5, 10));
                e.SlidingExpiration = TimeSpan.FromMinutes(1);

                return EpisodeModel.Create(await _listeningRepository.GetAllEpisodeByAlbumNameAsync(albumName), true);


            });

            if (Models == null)
            {
                return NotFound();
            }

            return Models;

        }
    }
}
