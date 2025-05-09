﻿using Listening.Domain;
using Listening.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Listening.Infrastructure
{
    public class ListeningRepository : IListeningRepository
    {
        private readonly ListeningDbContext _context;

        public ListeningRepository(ListeningDbContext context)
        {
            _context = context;
        }

        public Task AddAlbum(Album album, string categoryName)
        {
            throw new NotImplementedException();
        }

        public Task AddCategory(Category category)
        {
            throw new NotImplementedException();
        }

        public Task AddEpisode(Episode episode, string albumName)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAlbum(Album album, string categoryName)
        {
            throw new NotImplementedException();
        }

        public Task DeleteCategory(Category category)
        {
            throw new NotImplementedException();
        }

        public Task DeleteEpisode(Episode episode, string albumName)
        {
            throw new NotImplementedException();
        }

        public async Task<Album?> FindAlbumByIdAsync(Guid id)
        {
            return await _context.FindAsync<Album>(id);
        }

        public async Task<Album?> FindAlbumByNameAsync(string albumName)
        {
            return await _context.Albums.FirstOrDefaultAsync(x => x.AlbumName == albumName);
        }

        public async Task<Category?> FindCategoryByIdAsync(Guid id)
        {
            return await _context.FindAsync<Category>(id);
        }

        public async Task<Category?> FindCategoryByNameAsync(string categoryName)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(x => x.CategoryName == categoryName);
            return category;
        }

        public async Task<Episode?> FindEpisodeByIdAsync(Guid id)
        {
            return await _context.FindAsync<Episode>(id);
        }

        public async Task<Episode?> FindEpisodeByNameAsync(string episodeName)
        {
            return await _context.Episodes.FirstOrDefaultAsync(x => x.EpisodeName == episodeName);
        }

        public async Task<Album[]?> GetAllAlbumByCategoryNameAsync(string categoryName)
        {
            return await _context.Albums.Where(e=>e.CategoryName==categoryName).OrderBy(r => r.ShowIndex).ToArrayAsync();
        }

        public async Task<Category[]?> GetAllCategoryAsync()
        {
            return await _context.Categories.OrderBy(e => e.ShowIndex).ToArrayAsync();
        }

        public async Task<Episode[]?> GetAllEpisodeByAlbumNameAsync(string albumName)
        {
            return await _context.Episodes.Where(e=>e.AlbumName==albumName).OrderBy(e => e.ShowIndex).ToArrayAsync();
        }

        public async Task<int> GetMaxIndexOfAlbumsAsync(Guid categotyId)
        {


            var maxIndex = await _context.Albums.Where(e => e.CategoryId == categotyId).OrderByDescending(e => e.ShowIndex).FirstAsync();

            return maxIndex.ShowIndex;
        }

        public async Task<int> GetMaxIndexOfCategoriesAsync()
        {
            var maxIndex = await _context.Categories.OrderByDescending(e => e.ShowIndex).FirstAsync();
            return maxIndex.ShowIndex;
        }



        public async Task<int> GetMaxIndexOfEpisodesAsync(string albumName)
        {
            var maxIndex = await _context.Episodes.Where(e => e.AlbumName == albumName).OrderByDescending(e => e.ShowIndex).FirstAsync();

            return maxIndex.ShowIndex;
        }

       

        public Task UpdateAlbum(Album album, string categoryName)
        {
            throw new NotImplementedException();
        }

        public Task UpdateCategory(Category category)
        {
            throw new NotImplementedException();
        }

        public Task UpdateEpisode(Episode episode, string albumName)
        {
            throw new NotImplementedException();
        }
    }
}
