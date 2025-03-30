using FileService.Domain;

namespace FileService.Infrastructure
{
    public class LocalCloundClient : ICloundClient
    {
        public CloundClientType type => CloundClientType.Local;

        public async Task<Uri> SaveUploadItemAsync(string key, Stream context)
        {
            using Stream writeStream = File.OpenWrite("E:/DDDProjectUpload" + key);
            await context.CopyToAsync(writeStream);

            return new Uri("E:/DDDProjectUpload" + key);

        }
    }



}
