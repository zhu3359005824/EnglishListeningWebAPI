using Microsoft.AspNetCore.Identity;

namespace IDentity.Domain.Entity
{
    public class MyRole : IdentityRole<Guid>
    {
        public MyRole()
        {
            this.Id = Guid.NewGuid();
        }
    }
}
