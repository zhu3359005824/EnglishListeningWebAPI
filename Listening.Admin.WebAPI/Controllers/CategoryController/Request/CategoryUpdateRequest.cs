
using FluentValidation;

namespace Listening.Admin.WebAPI.Categories;

//定义只是偶然和CategoryAddRequest一样，所以不应该复用它
public record CategoryUpdateRequest(string Name, Uri CoverUrl);

public class CategoryUpdateRequestValidator : AbstractValidator<CategoryUpdateRequest>
{
    public CategoryUpdateRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        
    }
}