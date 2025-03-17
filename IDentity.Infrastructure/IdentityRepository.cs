using IDentity.Domain;
using IDentity.Domain.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZHZ.Tools;

namespace IDentity.Infrastructure
{
    public class IdentityRepository : IIdentityRepository
    {
        private readonly UserManager<MyUser> _userManager;
        private readonly RoleManager<MyRole> _roleManager;

        public IdentityRepository(UserManager<MyUser> userManager, RoleManager<MyRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IdentityResult> AccessFailedAsync(MyUser user)
        {
             var identityResult=await  _userManager.AccessFailedAsync(user);
            return identityResult;
        }

        public  async Task<IdentityResult> ChangePasswordAsync(MyUser user, string newpassword)
        {

            if (newpassword.Length < 5)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "密码不符合要求",
                    Description = "密码长度必须大于5"
                });

            }    

         //_userManager.ChangePasswordAsync(user, oldpassword, newpassword);
         //这种方式是确认密码正确后,再修改


         //_userManager.ResetPasswordAsync(user, token, newpassword);
         //这种方式是确认验证码后,再修改

            string token=await  _userManager.GeneratePasswordResetTokenAsync(user);
            string passwordHash = HashHelper.ComputeSha256Hash(newpassword);
            return  await _userManager.ResetPasswordAsync(user,token, passwordHash);


        }

        public Task<IdentityResult> ChangePhoneNumber(MyUser user, string phoneNum,string token)
        {
          return _userManager.ChangePhoneNumberAsync(user, phoneNum,token);
        }

        public async Task<SignInResult> CheckPassword(MyUser user, string password)
        {

            if(await _userManager.IsLockedOutAsync(user))
            {
                return SignInResult.LockedOut;
            }



          if (await _userManager.CheckPasswordAsync(user, password))
            {
                return SignInResult.Success;
            }
            else
            {
                return SignInResult.Failed;
            }
        }

        public Task<IdentityResult> AddUserAsync(MyUser user, string password)
        {
          string passwordHash=  HashHelper.ComputeSha256Hash(password);
          return  _userManager.CreateAsync(user, passwordHash);
        }

        public Task<MyUser?> FindByIdAsync(Guid userId)
        {
            return _userManager.FindByIdAsync(userId.ToString());
        }

        public Task<MyUser?> FindByNameAsync(string userName)
        {
           return _userManager.FindByNameAsync(userName);
        }

        public Task<MyUser?> FindByPhoneNumberAsync(string phoneNum)
        {
            return _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNum);
        }

        public  Task<IList<string>> GetRolesAsync(MyUser user)
        {
            return  _userManager.GetRolesAsync(user);
        }

        public async Task ResetAccessFailedCount(MyUser user)
        {
            await _userManager.ResetAccessFailedCountAsync(user);
           
        }

       

        public async Task<IdentityResult> UserSetRole(MyUser user, string role)
        {
           return  await _userManager.AddToRoleAsync(user, role);
        }

        public Task<IdentityResult> CreateRole(string roleName)
        {
            MyRole role = new MyRole();
            role.Name = roleName;

           return   _roleManager.CreateAsync(role);
        }
    }
}
