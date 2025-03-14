using Listening.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listening.Domain
{
    public class ListeningDomainService
    {

        private readonly IListeningRepository _listeningRepository;

        public ListeningDomainService(IListeningRepository listeningRepository)
        {
            _listeningRepository = listeningRepository;
        }

        public async Task<Category> AddCategory(string categoryName)
        {
           var category= await  _listeningRepository.FindCategoryByNameAsync(categoryName);
            if (category != null) return category;
            Category newCategory=new Category(categoryName,1);
            return newCategory;

        }
        public async Task<Album> AddAlbum(string albumName,Guid categoryId)
        {
            var category= await _listeningRepository.FindCategoryByIdAsync(categoryId);
            if (category==null)
            {
                throw new Exception($"{categoryId}不存在");
            }
            var album = await _listeningRepository.FindAlbumByNameAsync(albumName);
            if (album != null) return album;
            Album newAlbum = new Album(albumName, 1, categoryId);
            return newAlbum;

        }
        public async Task<Episode> AddEpisode(string episodeName,Guid albumId,string sentenceContext,string setenceType)
        {

            var album=await _listeningRepository.FindAlbumByIdAsync(albumId);
            if (album == null)
            {
                throw new Exception($"{albumId}不存在");
            }
            var episode = await _listeningRepository.FindEpisodeByNameAsync(episodeName);
            if (episode != null) return episode;
            Episode newEpisode = new Episode(albumId, sentenceContext, setenceType);
            return newEpisode;

        }


        public async Task ChangeEpisodeShowIndex(string albumName, Guid episodeId,int index)
        {
            var episodes = await _listeningRepository.GetAllEpisodeByAlbumNameAsync(albumName);

            var hasEpisodeId=episodes.OrderBy(e=>e.Id).Select(e=>e.Id).ToArray();

            if (!hasEpisodeId.Contains(episodeId))
            {
                throw new Exception("非法ID");
            }

           var episode= await _listeningRepository.FindEpisodeByIdAsync(episodeId);
            episode.ChangeShowIndex(index);

        }
    }
}
