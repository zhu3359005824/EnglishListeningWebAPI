using FileService.Domain;
using FileService.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileService.Infrastructure
{
    public class FileRepository : IFileRepository
    {
        private readonly MyDbContext _dbContext;

        public FileRepository(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UploadItem?> FindOneAsync(long fileByteSize, string fileSHA256Hash)
        {
           return _dbContext.UploadItems.FirstOrDefault(x=>x.FileByteSize==fileByteSize&&
           x.FileSHA256Hash==fileSHA256Hash);
        }
    }
}
