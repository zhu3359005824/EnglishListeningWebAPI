using IDentity.Domain;
using IDentity.Domain.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IDentity.WebAPI.Controllers.Login
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class CRUDController : ControllerBase
    {

        private readonly UserManager<MyUser> _userManager;
        private readonly IIdentityRepository _identityRepository;

        public CRUDController(UserManager<MyUser> userManager, IIdentityRepository identityRepository)
        {
            _userManager = userManager;
            _identityRepository = identityRepository;
        }

        public async Task<ActionResult> AddUser(AddNewAdminRequest req)
        {
            MyUser user = new MyUser(req.userName);
            var hasUser = await _identityRepository.FindByPhoneNumberAsync(req.phoneNum);
            if (hasUser != null) return BadRequest($"用户已存在");

           await _identityRepository.AddUserAsync(user, req.password);

            return Ok("添加成功");


        }
    }

   
}
