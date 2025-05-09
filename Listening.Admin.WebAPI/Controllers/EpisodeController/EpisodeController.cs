﻿using FileService.Domain.Entity;
using FileService.Infrastructure;
using Listening.Domain;
using Listening.Domain.Entity;
using Listening.Infrastructure;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using ZHZ.EventBus;
using ZHZ.UnitOkWork;

namespace Listening.Admin.WebAPI.Controllers.EpisodeController
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "Admin")]

    public class EpisodeController : ControllerBase
    {
        private readonly ListeningDbContext _dbCtx;
        private IListeningRepository _listeningRepository;
        private readonly ListeningDomainService _listeningDomainService;
        private readonly IMemoryCache _memoryCache;

        private readonly IEventBus _eventBus;
        private readonly EncodingEpisodeHelper _episodeHelper;

        private readonly MyDbContext _fileDbContext;

        public EpisodeController(ListeningDbContext dbCtx, IListeningRepository listeningRepository, ListeningDomainService listeningDomainService, IMemoryCache memoryCache, IEventBus eventBus, EncodingEpisodeHelper episodeHelper, MyDbContext fileDbContext)
        {
            _dbCtx = dbCtx;
            _listeningRepository = listeningRepository;
            _listeningDomainService = listeningDomainService;
            _memoryCache = memoryCache;
            _eventBus = eventBus;
            _episodeHelper = episodeHelper;
            _fileDbContext = fileDbContext;
        }


        [UnitOfWork(typeof(ListeningDbContext))]
        [HttpPost]
        public async Task<ActionResult<bool>> AddEpisode(AddEpisodeRequest request)
        {
            var album = await _listeningRepository.FindAlbumByNameAsync(request.albumName);

            if (album == null)
            {

                return false;
            }

            if (request.AudioUrl.ToString().EndsWith("m4a", StringComparison.OrdinalIgnoreCase))
            {

                Episode episode = new Episode(album.AlbumName, request.sentenceContext, request.sentenceType,
                request.episodeName,request.AudioUrl);

                _dbCtx.Episodes.Add(episode);

                return true;
            }
            else
            {
                Guid episodeId = Guid.NewGuid();

               var uploadItem= _fileDbContext.UploadItems.FirstOrDefault<UploadItem>(e => e.SourceUrl == request.AudioUrl);
                if (uploadItem == null) return BadRequest("文件上传失败");

                EncodingEpisodeInfo encodingEpisode = new EncodingEpisodeInfo(
                     episodeId,
                     _listeningRepository.FindAlbumByNameAsync(request.albumName).Result.Id,
                      request.albumName,
                      request.episodeName,
                    request.sentenceContext,
                    request.sentenceType,
                    "Created");
                await _episodeHelper.AddEncodingEpisodeAsync(request.episodeName, encodingEpisode);

                

                //启动转码
                _eventBus.Publish("MediaEncoding.Created", 
                new MediaEncodingData(episodeId, "Listening", encodingEpisode.EpisodeName,
                                              "m4a",uploadItem.FileSHA256Hash,uploadItem.FileByteSize,request.AudioUrl)
                ); 
                return true;            
            }



        }


        [UnitOfWork(typeof(ListeningDbContext))]
        [HttpPost]
        public async Task<ActionResult<bool>> DeleteByName(DeleteEpisodeRequest del)
        {
            var episodes=_listeningRepository.GetAllEpisodeByAlbumNameAsync(del.albumName).Result;

            var episode=episodes.Where(x=>x.EpisodeName==del.episodeName).FirstOrDefault();
            if (episode == null) return false;
            episode.SoftDelete();
            return true;

         
        }



        


        [AllowAnonymous]
        [HttpGet]
        public void TestEvent()
        {
            _eventBus.Publish("MediaEncoding.Created",

            new MediaEncodingData(Guid.NewGuid(), "Listening", "test",
                                              "m4a","1",1,new Uri("http://127.0.0.1"))
            );
        }


        [HttpGet]
        [Route("{name}")]
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


        //获取albumId下所有的转码任务
        [HttpGet]
        [Route("{name}")]
        public async Task<ActionResult<EncodingEpisodeInfo[]>> FindEncodingEpisodesByAlbumId( string name)
        {
            List<EncodingEpisodeInfo> list = new List<EncodingEpisodeInfo>();
            var episodeNames = await _episodeHelper.GetEncodingEpisodeIdsAsync(name);
            foreach (string episodeName in episodeNames)
            {
                var encodingEpisode = await _episodeHelper.GetEncodingEpisodeAsync(episodeName);
                if (!encodingEpisode.Status.Equals("Completed"))//不显示已经完成的
                {
                    list.Add(encodingEpisode);
                }
            }
            return list.ToArray();
        }
    }
}
