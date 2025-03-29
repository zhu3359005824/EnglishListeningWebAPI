

using IDentity.Domain.Entity;

namespace IdentityService.WebAPI.Controllers.UserAdmin;
public record UserDTO(Guid Id, string UserName, string PhoneNumber, DateTime? CreationTime)
{
    public static UserDTO Create(MyUser user)
    {
        return new UserDTO(user.Id, user.UserName, user.PhoneNumber, user.CreateTime);
    }
}