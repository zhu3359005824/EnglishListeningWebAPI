using MediaEncoder.Domain;

namespace MediaEncoder.Infrastructure
{
    public class MediaEncoderRepository : IMediaEncoderRepository
    {

        private readonly MediaEncoderDbContext _mediaEncoderDbContext;

        public MediaEncoderRepository(MediaEncoderDbContext mediaEncoderDbContext)
        {
            _mediaEncoderDbContext = mediaEncoderDbContext;
        }

        public Task<EncodingItem?> FindOneFinishAsync(string fileSHA256Hash, long fileByteSize,ItemStatus status)
        {
            var result = _mediaEncoderDbContext.EncodingItems.FirstOrDefault(x => x.FileSHA256Hash == fileSHA256Hash && x.FileByteSize == fileByteSize&&x.Status== ItemStatus.Completed);

            return Task.FromResult(result);
        }

        public Task<EncodingItem[]> FindAsync(ItemStatus itemStatus)
        {
            var result = _mediaEncoderDbContext.EncodingItems.Where(e => e.Status == itemStatus).ToArray();

            return Task.FromResult(result);
        }
    }
}
