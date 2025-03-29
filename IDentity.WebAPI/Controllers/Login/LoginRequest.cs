using FluentValidation;
using GlobalConfigurations;
using IDentity.WebAPI.Controllers.Login;

namespace IDentity.WebAPI.Controllers.Login
{
    public record LoginRequest(string phoneNumber, string password) : IValidationData
    {
    }
}

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.phoneNumber).NotNull().Length(8, 15);
        RuleFor(x => x.password).NotNull().Length(6, 20);
    }
}
