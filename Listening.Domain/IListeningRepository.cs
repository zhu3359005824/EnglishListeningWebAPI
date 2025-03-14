﻿using Listening.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listening.Domain
{
    public interface IListeningRepository
    {
        Task<Category?> FindCategoryByNameAsync(string categoryName);
        Task<Category?> FindCategoryByIdAsync(Guid id);

        public Task<Category[]?> GetAllCategoryAsync();

        Task<Album?> FindAlbumByNameAsync(string albumName);
        Task<Album?> FindAlbumByIdAsync(Guid id);

        public Task<Album[]?> GetAllAlbumByCategoryNameAsync(string categoryName);

        Task<Episode?> FindEpisodeByNameAsync(string episodeName);
        Task<Episode?> FindEpisodeByIdAsync(Guid id);

        public Task<Episode[]?> GetAllEpisodeByAlbumNameAsync(string albumName);

        public Task<int> GetMaxIndexOfCategoriesAsync();//获取最大序号
       
        public Task<int> GetMaxIndexOfAlbumsAsync(Guid  categotyId);
        
        public Task<int> GetMaxIndexOfEpisodesAsync(Guid albumId);
     




    }
}
