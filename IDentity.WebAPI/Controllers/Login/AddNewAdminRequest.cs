using FluentValidation;
using GlobalConfigurations;
using IDentity.WebAPI.Controllers.Login;

namespace IDentity.WebAPI.Controllers.Login
{
    public record AddNewAdminRequest(string userName, string phoneNum, string password): IValidationData
    {
    }
}
public class AddNewAdminRequestValidator : AbstractValidator<AddNewAdminRequest>
{
    public AddNewAdminRequestValidator()
    {
        RuleFor(x => x.userName).NotNull().Length(1, 15);
        RuleFor(x => x.phoneNum).NotNull().Length(8, 15);
        RuleFor(x => x.password).NotNull().Length(6, 20);
    }
}