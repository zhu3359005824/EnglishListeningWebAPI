using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDentity.Domain.Entity
{
    public class MyRole:IdentityRole<Guid>
    {
        public MyRole() 
        {
            this.Id = Guid.NewGuid();
        }
    }
}
