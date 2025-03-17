using IDentity.Domain.Entity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDentity.Domain
{
    public interface IIdentityRepository
    {
        Task<MyUser?> FindByIdAsync(Guid userId);//根据Id获取用户
        Task<MyUser?> FindByNameAsync(string userName);//根据用户名获取用户
        Task<MyUser?> FindByPhoneNumberAsync(string phoneNum);//根据手机号获取用户
        /// <summary>
        /// 用户输入密码,但保存的是SHA256Hash值
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<IdentityResult> AddUserAsync(MyUser user, string password);//创建用户
        Task<IdentityResult> AccessFailedAsync(MyUser user);//记录一次登陆失败

        Task<SignInResult> CheckPassword(MyUser user,string password);

        Task<IdentityResult> UserSetRole(MyUser user,string roleName);

        Task<IdentityResult> ChangePhoneNumber(MyUser user,string phoneNum,string token);
        Task<IdentityResult> ChangePasswordAsync(MyUser user,string newpassword);

      Task<IdentityResult> CreateRole(string roleName);
        Task ResetAccessFailedCount(MyUser user);


     

        Task<IList<string>> GetRolesAsync(MyUser user);


       
    }
}
