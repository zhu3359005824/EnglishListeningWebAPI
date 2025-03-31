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
        [Route("{name}")]
        public async Task<ActionResult<EpisodeModel>> FindEpisodeByName(string name)
        {

            var Model = await _memoryCache.GetOrCreateAsync($"EpisodeModel_FindEpisodeByName_{name}", async (e) =>
            {

                e.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(Random.Shared.Next(5, 10));
                e.SlidingExpiration = TimeSpan.FromMinutes(1);

                return EpisodeModel.Create(await _listeningRepository.FindEpisodeByNameAsync(name), true);


            });

            if (Model == null)
            {
                return NotFound();
            }

            return Model;
        }


        [HttpGet]
        [Route("{name}")]
        public async Task<ActionResult<EpisodeModel[]>> FindEpisodesByAlbumName(string name)
        {
            var Models = await _memoryCache.GetOrCreateAsync($"EpisodeModels_FindEpisodesByAlbumName_{name}", async (e) =>
            {

                e.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(Random.Shared.Next(5, 10));
                e.SlidingExpiration = TimeSpan.FromMinutes(1);

                return EpisodeModel.Create(await _listeningRepository.GetAllEpisodeByAlbumNameAsync(name), true);


            });

            if (Models == null)
            {
                return NotFound();
            }

            return Models;

        }
    }
}
