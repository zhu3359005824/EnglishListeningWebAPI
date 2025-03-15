using FluentValidation;
using Listening.Admin.WebAPI.Controllers.CategoryController;
using Listening.Domain.Entity;
using Listening.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Listening.Admin.WebAPI.Controllers.CategoryController
{
    public record AddCategoryRequest(string CategoryName, int ShowIndex);

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


        RuleFor(x => x.CategoryName).MustAsync(async (CategoryName, cancellation) =>
        {
            // 查询数据库，检查 CategoryId 是否存在
            return await dbCtx.Categories.AnyAsync(c => c.CategoryName == CategoryName, cancellation);
        }).WithMessage("AddCategory123456不存在");

        Console.WriteLine("123");

    }
}
