using FileService.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
