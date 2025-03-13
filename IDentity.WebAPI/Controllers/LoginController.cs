
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
                return BadRequest("�û���������");
            }
             string passwordHash= HashHelper.ComputeSha256Hash(password);

            var (IsLogin, token) = await _identityDomainService.LoginByPhoneNumberAndPwdAsync(phoneNumber, passwordHash);

            if (IsLogin.Succeeded)
            {
                return token;
            }
            else if (IsLogin.IsLockedOut)
            {
                return BadRequest("�û�������");
            }
            else
            {
                return BadRequest("�ֻ��Ż��������");
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
            return BadRequest("����");
        }
    }
}
