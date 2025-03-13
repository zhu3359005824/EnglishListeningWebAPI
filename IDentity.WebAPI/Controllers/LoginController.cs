
using IDentity.Domain;
using IDentity.Domain.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZHZ.Tools;

namespace IDentity.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class LoginController : ControllerBase
    {
        private readonly IIdentityRepository _identityRepository;

        private readonly IdentityDomainService _identityDomainService;

        public LoginController(IIdentityRepository identityRepository, IdentityDomainService identityDomainService)
        {
            _identityRepository = identityRepository;
            _identityDomainService = identityDomainService;
        }

        [HttpGet]
        public async Task<ActionResult<string>> LoginByPhoneNumberAndPwd(string phoneNumber, string password)
        {
            var user = await _identityRepository.FindByPhoneNumberAsync(phoneNumber);
            if (user == null)
            {
                return BadRequest("用户名不存在");
            }
             string passwordHash= HashHelper.ComputeSha256Hash(password);

            var (IsLogin, token) = await _identityDomainService.LoginByPhoneNumberAndPwdAsync(phoneNumber, passwordHash);

            if (IsLogin.Succeeded)
            {
                return token;
            }
            else if (IsLogin.IsLockedOut)
            {
                return BadRequest("用户已锁定");
            }
            else
            {
                return BadRequest("手机号或密码错误");
            }



        }


       
        [UnitOfWork]
        [HttpGet]
        public async Task<ActionResult> InitTest()
        {

            MyUser user = new MyUser("zhz");
            user.PhoneNumber = "11111111";
            var hasUser=  await _identityRepository.FindByNameAsync(user.UserName);
            if (hasUser != null)
            {
                return Ok("ok");
            }
            
          
           var result= await _identityRepository.AddUserAsync(user, "123456");
            if (result.Succeeded)
            {
                return Ok("ok");
            }
            return BadRequest("错误");
        }
    }
}
