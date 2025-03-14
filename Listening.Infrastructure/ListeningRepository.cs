using Listening.Domain;
using Listening.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listening.Infrastructure
{
    public class ListeningRepository : IListeningRepository
    {
        private readonly ListeningDbContext _context;

        public ListeningRepository(ListeningDbContext context)
        {
            _context = context;
        }

        public async Task<Album?> FindAlbumByIdAsync(Guid id)
        {
           return await _context.FindAsync<Album>(id);  
        }

        public async Task<Album?> FindAlbumByNameAsync(string albumName)
        {
            return await _context.FindAsync<Album>(albumName);
        }

        public async Task<Category?> FindCategoryByIdAsync(Guid id)
        {
           return await _context.FindAsync<Category>(id);
        }

        public async Task<Category?> FindCategoryByNameAsync(string categoryName)
        {
            return await _context.FindAsync<Category>(categoryName );
        }

        public async Task<Episode?> FindEpisodeByIdAsync(Guid id)
        {
           return await _context.FindAsync<Episode>(id);
        }

        public  async Task<Episode?> FindEpisodeByNameAsync(string episodeName)
        {
            return await _context.FindAsync<Episode>(episodeName);
        }

        public async Task<Album[]?> GetAllAlbumByCategoryNameAsync(string categoryName)
        {
           return await _context.Albums.OrderBy(r=>r.ShowIndex).ToArrayAsync();
        }

        public async Task<Category[]?> GetAllCategoryAsync()
        {
           return await _context.Categories.OrderBy(e=>e.ShowIndex).ToArrayAsync();
        }

        public  async Task<Episode[]?> GetAllEpisodeByAlbumNameAsync(string albumName)
        {
            return await _context.Episodes.OrderBy(e => e.ShowIndex).ToArrayAsync();
        }

        public async Task<int> GetMaxIndexOfAlbumsAsync(Guid categotyId)
        {
           

            var maxIndex = await _context.Albums.Where(e => e.CategoryId == categotyId).OrderByDescending(e => e.ShowIndex).FirstAsync();

            return maxIndex.ShowIndex;
        }

        public async Task<int> GetMaxIndexOfCategoriesAsync()
        {
            var maxIndex = await  _context.Categories.OrderByDescending(e=>e.ShowIndex).FirstAsync();
            return maxIndex.ShowIndex;
        }

        

        public async Task<int> GetMaxIndexOfEpisodesAsync(Guid albumId)
        {
            var maxIndex = await _context.Episodes.Where(e => e.AlbumId == albumId).OrderByDescending(e => e.ShowIndex).FirstAsync();

            return maxIndex.ShowIndex;
        }
    }
}
