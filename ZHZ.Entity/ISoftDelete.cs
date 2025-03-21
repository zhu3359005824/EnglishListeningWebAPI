using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZHZ.Entity
{
    public interface ISoftDelete
    {
        bool IsDeleted { get;  }

        void SoftDelete();
    }
}
