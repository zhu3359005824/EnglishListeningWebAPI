namespace FileService.Domain
{
    /// <summary>
    /// 服务器接口
    /// </summary>
    public interface ICloundClient
    {
        CloundClientType type { get; }


        Task<Uri> SaveUploadItemAsync(string key, Stream context);
    }
}


public enum CloundClientType
{
    Public,
    Local
}
