
using Listening.Admin.WebAPI.Controllers.CategoryController;
using Listening.Domain;
using Listening.Domain.Entity;
using Listening.Infrastructure;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using ZHZ.EventBus;
using ZHZ.UnitOkWork;

namespace Listening.Admin.WebAPI.Controllers.EpisodeController
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    [UnitOfWork(typeof(ListeningDbContext))]
    public class EpisodeController : ControllerBase
    {
        private readonly ListeningDbContext _dbCtx;
        private IListeningRepository _listeningRepository;
        private readonly ListeningDomainService _listeningDomainService;
        private readonly IMemoryCache _memoryCache;

        private readonly IEventBus _eventBus;
        private readonly EncodingEpisodeHelper _episodeHelper;

        public EpisodeController(ListeningDbContext dbCtx, IListeningRepository listeningRepository, ListeningDomainService listeningDomainService, IMemoryCache memoryCache, IEventBus eventBus, EncodingEpisodeHelper episodeHelper)
        {
            _dbCtx = dbCtx;
            _listeningRepository = listeningRepository;
            _listeningDomainService = listeningDomainService;
            _memoryCache = memoryCache;
            _eventBus = eventBus;
            _episodeHelper = episodeHelper;
        }



        [HttpPost]
        public async Task<ActionResult<Guid>> AddEpisode(AddEpisodeRequest request)
        {
            var album=await _listeningRepository.FindAlbumByNameAsync(request.albumName);

            if (album == null) 
            {
                return BadRequest($"Album_{request.albumName}不存在");
            }

            if(request.sentenceType.Equals("m4a",StringComparison.OrdinalIgnoreCase))
            {
                Episode episode = new Episode(album.Id, request.sentenceContext, request.sentenceType,
                request.episodeName);

                _dbCtx.Episodes.Add(episode);

                return episode.Id;
            }
            else
            {
                Guid episodeId = Guid.NewGuid();

                EncodingEpisodeInfo encodingEpisode = new EncodingEpisodeInfo(

                     episodeId,
                     _listeningRepository.FindAlbumByNameAsync(request.albumName).Result.Id,
                      request.albumName,
                      request.episodeName,
                    request.sentenceContext,
                    request.sentenceType,
                    "Created");
             await   _episodeHelper.AddEncodingEpisodeAsync(request.episodeName, encodingEpisode);

                //启动转码
                _eventBus.Publish("MediaEncoding.Created", new
                {
                    EpisodeName= encodingEpisode.EpisodeName,
                    OutputType="m4a",
                    SourceSystem="Listening"
                });

                return episodeId;
            }

            

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
