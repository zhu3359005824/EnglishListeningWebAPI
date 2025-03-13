using FileService.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileService.Domain
{
    /// <summary>
    /// 服务器接口
    /// </summary>
    public interface ICloundClient
    {
        CloundClientType type { get; }


        Task<Uri> SaveUploadItemAsync(string key,Stream context);
    }
}


    public enum CloundClientType
    {
       Public,
       Local
    }
