
using FluentValidation;

namespace Listening.Admin.WebAPI.Controllers.CategoryController.Request;

//启用了<Nullable>enable</Nullable>，所以string ChineseName就是非可空，会自动校验
public record CategoryAddRequest(string Name, int ShowIndex, Uri? CoverUrl);
public class CategoryAddRequestValidator : AbstractValidator<CategoryAddRequest>
{
    public CategoryAddRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty();

        RuleFor(x => x.CoverUrl);//CoverUrl允许为空
    }
}