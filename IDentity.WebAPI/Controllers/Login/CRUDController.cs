using IDentity.Domain;
using IDentity.Domain.Entity;
using Microsoft.AspNetCore.Http;
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

        //public async Task<ActionResult> AddUser(string username,string password)
        //{
        //    MyUser user= new MyUser(username);
        //    var hasUser=  await _identityRepository.FindByNameAsync(username);
        //    if (hasUser != null) return BadRequest($"用户名{username }已存在");

        //    _identityRepository.AddUserAsync(user,password);


        //}
    }
}
