using FluentValidation;
using Listening.Admin.WebAPI.Controllers.EpisodeController;

namespace Listening.Admin.WebAPI.Controllers.EpisodeController
{
    public record DeleteEpisodeRequest(string albumName,string episodeName)
    {
    }


}
public class DeleteEpisodeRequestValidator : AbstractValidator<DeleteEpisodeRequest>
{
    public DeleteEpisodeRequestValidator()
    {

        RuleFor(x => x.albumName).NotEmpty().WithMessage("专辑名称不能为空");

        RuleFor(x => x.albumName).NotNull().Length(1, 200).WithMessage("专辑名称长度必须在 1 到 200 之间");

        RuleFor(x => x.episodeName).NotEmpty().WithMessage("单曲名称不能为空");

        RuleFor(x => x.episodeName).NotNull().Length(1, 200).WithMessage("单曲名称长度必须在 1 到 200 之间");

    }
}
