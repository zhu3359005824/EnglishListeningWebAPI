namespace MediaEncoder.Domain
{
    public interface IMediaEncoderRepository
    {

        Task<EncodingItem?> FindOneFinishAsync(string fileSHA256Hash, long fileByteSize,ItemStatus status);


        Task<EncodingItem[]> FindAsync(ItemStatus itemStatus);
    }
}
