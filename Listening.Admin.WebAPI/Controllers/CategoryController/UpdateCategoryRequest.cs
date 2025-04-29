using FluentValidation;
using Listening.Admin.WebAPI.Controllers.CategoryController;
using Listening.Infrastructure;

namespace Listening.Admin.WebAPI.Controllers.CategoryController
{
    public record UpdateCategoryRequest(string oldCategoryName,string newCategoryName,Uri? coverUri,int? showIndex)
    {
    }
}
public class CategoryUpdateRequestValidator : AbstractValidator<UpdateCategoryRequest>
{
    public CategoryUpdateRequestValidator(ListeningDbContext dbCtx)
    {

        RuleFor(x => x.newCategoryName).NotEmpty().WithMessage("专辑名称不能为空");

        RuleFor(x => x.newCategoryName).NotNull().Length(1, 200).WithMessage("专辑名称长度必须在 1 到 200 之间");
        




    }
}