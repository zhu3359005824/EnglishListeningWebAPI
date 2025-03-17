using FileService.Infrastructure;
using FileService.WebAPI.Controllers;
using FluentValidation;
using GlobalConfigurations;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.EntityFrameworkCore;

namespace FileService.WebAPI.Controllers
{
    public class UploadRequest:IValidationData
    {
        public IFormFile File { get; set; }
    }
  



}

public class UploadRequestValidator:AbstractValidator<UploadRequest>
{
    public UploadRequestValidator()
    {
      
        RuleFor(e=>e.File).NotNull();
    }
}
