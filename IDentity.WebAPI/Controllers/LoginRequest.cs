﻿using FluentValidation;
using IDentity.WebAPI.Controllers;

namespace IDentity.WebAPI.Controllers
{
    public record LoginRequest(string phoneNumber,string password)
    {
    }
}

public class LoginRequestValidator: AbstractValidator<LoginRequest>
{
     public LoginRequestValidator()
    {
        RuleFor(x => x.phoneNumber).NotNull().Length(8,15);
        RuleFor(x => x.password).NotNull().Length(6,20);
    }
}
