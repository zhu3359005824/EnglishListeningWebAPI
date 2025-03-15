using FluentValidation;
using Listening.Admin.WebAPI.Controllers.AlbumController;
using Listening.Domain.Entity;
using Listening.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Listening.Admin.WebAPI.Controllers.AlbumController
{
    public record AddAlbumRequest(string AlbumName,int ShowIndex ,string CategoryName);

    //把校验规则写到单独的文件，也是DDD的一种原则
   
}
public class AddAlbumRequestValidator : AbstractValidator<AddAlbumRequest>
{
    public AddAlbumRequestValidator(ListeningDbContext dbCtx)
    {
        // 校验 AlbumName 不为空
        RuleFor(x => x.AlbumName).NotEmpty().WithMessage("专辑名称不能为空");
        // 校验 AlbumName 长度在 1 到 200 之间
        RuleFor(x => x.AlbumName).NotNull().Length(1, 200).WithMessage("专辑名称长度必须在 1 到 200 之间");
        // 校验 ShowIndex 不为空
        RuleFor(x => x.ShowIndex).NotEmpty().WithMessage("展示索引不能为空");

        // 校验 CategoryId 是否存在
        RuleFor(x => x.CategoryName).MustAsync(async (CategoryName, cancellation) =>
        {
            // 查询数据库，检查 CategoryId 是否存在
            return await dbCtx.Categories.AnyAsync(c => c.CategoryName == CategoryName, cancellation);
        }).WithMessage("CategoryId 不存在");



    }
}
