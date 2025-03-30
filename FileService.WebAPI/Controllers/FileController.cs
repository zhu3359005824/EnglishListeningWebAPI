using FileService.Domain;
using FileService.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace FileService.WebAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class FileController : ControllerBase
    {

        private readonly FileDomainService _fileService;
        private readonly MyDbContext _dbContext;
        private readonly FileDomainService _fileDomainService;

        public FileController(FileDomainService fileService, MyDbContext dbContext, FileDomainService fileDomainService)
        {
            _fileService = fileService;
            _dbContext = dbContext;
            _fileDomainService = fileDomainService;
        }

        [HttpPost]
        public async Task<ActionResult<Uri>> UploadItems([FromForm] UploadRequest uploadRequest)
        {
            var file = uploadRequest.File;
            string fileName = file.FileName;
            using Stream stream = file.OpenReadStream();

            var uploadItem = await _fileService.UploadItemAsync(stream, fileName);

            await _dbContext.UploadItems.AddAsync(uploadItem);
            await _dbContext.SaveChangesAsync();



            return uploadItem.SourceUrl;

        }
    }
}
