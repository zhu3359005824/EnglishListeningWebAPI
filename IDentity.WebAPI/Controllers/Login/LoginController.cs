using IDentity.Domain;
using IDentity.Domain.Entity;
using IDentity.Infrastructure;
using IDentity.WebAPI.Event;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ZHZ.EventBus;
using ZHZ.Tools;
using ZHZ.UnitOkWork;

namespace IDentity.WebAPI.Controllers.Login
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class LoginController : ControllerBase
    {
        private readonly IIdentityRepository _identityRepository;

        private readonly IdentityDomainService _identityDomainService;

        private readonly IEventBus _eventBus;

        public LoginController(IIdentityRepository identityRepository, IdentityDomainService identityDomainService, IEventBus eventBus)
        {
            _identityRepository = identityRepository;
            _identityDomainService = identityDomainService;
            _eventBus = eventBus;
        }




        //[HttpPost]
        //public async Task<ActionResult<MyResponseData>> LoginByPhoneNumberAndPwd(LoginRequest req)
        //{
        //    var user = await _identityRepository.FindByPhoneNumberAsync(req.phoneNumber);
        //    if (user == null)
        //    {
        //        return BadRequest("用户名不存在");
        //    }
        //    string passwordHash = HashHelper.ComputeSha256Hash(req.password);

        //    var (IsLogin, token) = await _identityDomainService.LoginByPhoneNumberAndPwdAsync(req.phoneNumber, passwordHash);

        //    if (IsLogin.Succeeded)
        //    {
        //        MyResponseData responseData = new MyResponseData();

        //        responseData.Data = new { token, UserName = user.UserName };
        //        responseData.Code = 200;
        //        responseData.Message = "登录成功";

        //        return responseData;
        //    }
        //    else if (IsLogin.IsLockedOut)
        //    {
        //        return BadRequest("用户已锁定");
        //    }
        //    else
        //    {
        //        return BadRequest("手机号或密码错误");
        //    }



        //}

        [HttpPost]
        public async Task<ActionResult<string>> LoginByPhoneNumberAndPwd(LoginRequest req)
        {
            var user = await _identityRepository.FindByPhoneNumberAsync(req.phoneNumber);
            if (user == null)
            {
                return BadRequest("用户名不存在");
            }
            string passwordHash = HashHelper.ComputeSha256Hash(req.password);

            var (IsLogin, token) = await _identityDomainService.LoginByPhoneNumberAndPwdAsync(req.phoneNumber, passwordHash);

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

            if (await _identityRepository.CreateRole("Admin") == IdentityResult.Success)
            {
                MyUser user = new MyUser("zhz");
                user.PhoneNumber = "11111111";
                await _identityRepository.AddUserAsync(user, "123456");

                var result = await _identityRepository.UserSetRole(user, "Admin");


                if (result.Succeeded)
                {
                    //实现事件发布
                    _eventBus.Publish("IdentityService.User.Created", new UserCreatedEvent(user.Id, user.UserName, user.PasswordHash, user.PhoneNumber));
                    return Ok("ok");
                }
                return BadRequest("错误");

            }
            return BadRequest("cuowu");





            //var hasUser=  await _identityRepository.FindByNameAsync(user.UserName);
            //if (hasUser != null)
            //{
            //    return Ok("ok");
            //}



        }


        [HttpGet]
        public ActionResult<string> Test()
        {
            return Ok("ok");
        }

        [HttpPost]
        [UnitOfWork(typeof(MyIdentityDbContext))]
        public async Task<ActionResult> AddUser(AddNewAdminRequest req)
        {
            MyUser user = new MyUser(req.userName);
           
            var hasUser = await _identityRepository.FindByPhoneNumberAsync(req.phoneNum);
            if (hasUser != null) return BadRequest($"用户已存在");
            user.PhoneNumber = req.phoneNum;

            await _identityRepository.AddUserAsync(user, req.password);
            await _identityRepository.UserSetRole(user, "Admin");

            return Ok("添加成功");


        }
    }

   
}

