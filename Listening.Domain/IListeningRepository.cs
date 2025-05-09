﻿using Listening.Domain.Entity;

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

        public Task<int> GetMaxIndexOfAlbumsAsync(Guid categotyId);

        public Task<int> GetMaxIndexOfEpisodesAsync(string albumName);



        Task AddAlbum(Album album, string categoryName);
        Task AddEpisode(Episode episode, string albumName);
        Task AddCategory(Category category);



        Task DeleteAlbum(Album album, string categoryName);
        Task DeleteEpisode(Episode episode, string albumName);
        Task DeleteCategory(Category category);



        Task UpdateAlbum(Album album, string categoryName);
        Task UpdateEpisode(Episode episode, string albumName);
        Task UpdateCategory(Category category);
    }
}
