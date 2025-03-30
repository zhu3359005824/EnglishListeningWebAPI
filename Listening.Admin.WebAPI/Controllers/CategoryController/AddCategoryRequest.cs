using FluentValidation;
using GlobalConfigurations;
using Listening.Admin.WebAPI.Controllers.CategoryController;
using Listening.Infrastructure;

namespace Listening.Admin.WebAPI.Controllers.CategoryController
{
    public record AddCategoryRequest(string CategoryName, int ShowIndex) : IValidationData;

    //把校验规则写到单独的文件，也是DDD的一种原则

}
public class CategoryAddRequestValidator : AbstractValidator<AddCategoryRequest>
{
    public CategoryAddRequestValidator(ListeningDbContext dbCtx)
    {

        RuleFor(x => x.CategoryName).NotEmpty().WithMessage("专辑名称不能为空");

        RuleFor(x => x.CategoryName).NotNull().Length(1, 200).WithMessage("专辑名称长度必须在 1 到 200 之间");
        // 校验 ShowIndex 不为空
        RuleFor(x => x.ShowIndex).NotEmpty().WithMessage("展示索引不能为空");




    }
}
