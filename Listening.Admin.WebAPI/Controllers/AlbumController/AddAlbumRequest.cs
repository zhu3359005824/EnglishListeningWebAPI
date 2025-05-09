﻿using FluentValidation;
using Listening.Admin.WebAPI.Controllers.AlbumController;
using Listening.Infrastructure;

namespace Listening.Admin.WebAPI.Controllers.AlbumController
{

    public record AddAlbumRequest(string AlbumName, string CategoryName, int ShowIndex)
    {


    }

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





    }
}
