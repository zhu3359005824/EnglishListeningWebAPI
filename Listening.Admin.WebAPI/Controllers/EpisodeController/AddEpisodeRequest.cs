using FluentValidation;
using Listening.Admin.WebAPI.Controllers.CategoryController;
using Listening.Admin.WebAPI.Controllers.EpisodeController;
using Listening.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Listening.Admin.WebAPI.Controllers.EpisodeController
{
    public record AddEpisodeRequest(string albumName,string sentenceContext,string sentenceType,string episodeName)
    {
    }
}

public class AddEpisodeRequestValidator : AbstractValidator<AddEpisodeRequest>
{
    public AddEpisodeRequestValidator(ListeningDbContext dbCtx)
    {

        RuleFor(x => x.albumName).NotEmpty().WithMessage("专辑名称不能为空");

        RuleFor(x => x.albumName).NotNull().Length(1, 200).WithMessage("专辑名称长度必须在 1 到 200 之间");

        RuleFor(x => x.episodeName).NotEmpty().WithMessage("专辑名称不能为空");

        RuleFor(x => x.episodeName).NotNull().Length(1, 200).WithMessage("专辑名称长度必须在 1 到 200 之间");


        // 校验 ShowIndex 不为空
        RuleFor(x => x.sentenceContext).NotEmpty().WithMessage("展示索引不能为空");
        RuleFor(x => x.sentenceType).NotEmpty().WithMessage("展示索引不能为空");


        RuleFor(x => x.albumName).MustAsync(async (AlbumName, cancellation) =>
        {
            
            return await dbCtx.Albums.AnyAsync(c => c.AlbumName == AlbumName, cancellation);
        }).WithMessage($"Album不存在");



    }
}