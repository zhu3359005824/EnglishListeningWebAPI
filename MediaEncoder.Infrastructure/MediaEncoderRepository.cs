using MediaEncoder.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaEncoder.Infrastructure
{
    public class MediaEncoderRepository : IMediaEncoderRepository
    {

        private readonly MediaEncoderDbContext _mediaEncoderDbContext;

        public MediaEncoderRepository(MediaEncoderDbContext mediaEncoderDbContext)
        {
            _mediaEncoderDbContext = mediaEncoderDbContext;
        }

        public Task<EncodingItem?> FindOneFinishAsync(string fileSHA256Hash, long fileByteSize)
        {
            var result= _mediaEncoderDbContext.EncodingItems.Where(x=>x.FileSHA256Hash==fileSHA256Hash&&x.FileByteSize==fileByteSize).ToList();

            return Task.FromResult(result[0]); 
        }

        public Task<EncodingItem[]> FindAsync(ItemStatus itemStatus)
        {
            var result= _mediaEncoderDbContext.EncodingItems.Where(e=>e.Status==itemStatus).ToArray();

            return Task.FromResult(result);
        }
    }
}
