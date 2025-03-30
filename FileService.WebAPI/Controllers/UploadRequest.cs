using FileService.WebAPI.Controllers;
using FluentValidation;
using GlobalConfigurations;

namespace FileService.WebAPI.Controllers
{
    public class UploadRequest : IValidationData
    {
        public IFormFile File { get; set; }
    }




}

public class UploadRequestValidator : AbstractValidator<UploadRequest>
{
    public UploadRequestValidator()
    {

        RuleFor(e => e.File).NotNull();
    }
}
