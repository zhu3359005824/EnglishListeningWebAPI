using FileService.Domain.Entity;

namespace FileService.Domain
{

    public interface IFileRepository
    {
        /// <summary>
        /// 根据文件大小和SHA256确定唯一文件
        /// </summary>
        /// <param name="fileByteSize"></param>
        /// <param name="fileSHA256Hash"></param>
        /// <returns>UpLoadItem</returns>
        Task<UploadItem> FindOneAsync(long fileByteSize, string fileSHA256Hash);
    }
}
