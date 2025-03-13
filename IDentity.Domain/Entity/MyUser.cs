using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDentity.Domain.Entity
{
    public class MyUser:IdentityUser<Guid>
    {
        public DateTime? CreateTime { get; set; }

        public DateTime? DeleteTime { get; set; }

        public bool IsDeleted { get; private set; }

        private MyUser() { }

        public MyUser(string username)
        {
            this.UserName = username;
            this.Id = Guid.NewGuid();
            this.CreateTime = DateTime.Now;

        }

        public void SoftDelete()
        {
            this.IsDeleted = true;
            this.DeleteTime = DateTime.Now;
        }
    }
}
