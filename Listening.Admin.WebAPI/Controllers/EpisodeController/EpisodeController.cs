using IDentity.WebAPI;
using Listening.Admin.WebAPI.Controllers.CategoryController;
using Listening.Domain;
using Listening.Domain.Entity;
using Listening.Infrastructure;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Listening.Admin.WebAPI.Controllers.EpisodeController
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[Authorize(Roles = "Admin")]
    [UnitOfWork(typeof(ListeningDbContext))]
    public class EpisodeController : ControllerBase
    {
        private readonly ListeningDbContext _dbCtx;
        private IListeningRepository _listeningRepository;
        private readonly ListeningDomainService _listeningDomainService;
        private readonly IMemoryCache _memoryCache;

        public EpisodeController(ListeningDbContext dbCtx, IListeningRepository listeningRepository, ListeningDomainService listeningDomainService, IMemoryCache memoryCache)
        {
            _dbCtx = dbCtx;
            _listeningRepository = listeningRepository;
            _listeningDomainService = listeningDomainService;
            _memoryCache = memoryCache;
        }



        [HttpPost]
        public async Task<ActionResult> AddEpisode(AddEpisodeRequest request)
        {
            var album=await _listeningRepository.FindAlbumByNameAsync(request.albumName);
            Episode episode = new Episode(album.Id, request.sentenceContext, request.sentenceType,
                request.episodeName);
 
            _dbCtx.Episodes.Add(episode);

            return Ok("添加成功");

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
