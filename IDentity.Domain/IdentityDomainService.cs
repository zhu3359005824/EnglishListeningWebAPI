using IDentity.Domain.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ZHZ.JWT;

namespace IDentity.Domain
{
    public class IdentityDomainService
    {

        private readonly IIdentityRepository _identityRepository;
        private readonly IOptionsSnapshot<JWTSettings> _options;
        private readonly IJwtTokenService _jwtTokenService;

        public IdentityDomainService(IIdentityRepository identityRepository, IOptionsSnapshot<JWTSettings> options, IJwtTokenService jwtTokenService)
        {
            _identityRepository = identityRepository;
            _options = options;
            _jwtTokenService = jwtTokenService;
        }



        public async Task<SignInResult> CheckPassword(MyUser user, string password)
        {
           


            var result = await _identityRepository.CheckPassword(user, password);

            if (result.Succeeded)
            {

                await _identityRepository.ResetAccessFailedCount(user);
                return SignInResult.Success;
            }
            else if (result.IsLockedOut)
            {
                return SignInResult.LockedOut;
            }
              await _identityRepository.AccessFailedAsync(user);

            return SignInResult.Failed;



        }

        public async Task<(SignInResult signInResult,string? userToken)> LoginByPhoneNumberAndPwdAsync(string phoneNumber, string pwd)
        {
            var user = await _identityRepository.FindByPhoneNumberAsync(phoneNumber);
            if (user == null)
            {
              
                return (SignInResult.Failed, null);
            }
            else
            {
                var result = await CheckPassword(user, pwd);
                if (result.Succeeded)
                {
                   string userToken=await BuildUserTokenAsync(user);

                    return (SignInResult.Success,userToken);
                } else if (result.IsLockedOut)
                {
                    return(SignInResult.LockedOut,null);
                } 
                else 
                {
                    return (SignInResult.Failed, null);
                }
            }
        }


        public async Task<string> BuildUserTokenAsync(MyUser user)
        {

            var roles = await _identityRepository.GetRolesAsync(user);

            List<Claim> claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));

            foreach (var role in roles)
            {

                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return _jwtTokenService.BuildToken(claims, _options.Value);



        }
    }
}
