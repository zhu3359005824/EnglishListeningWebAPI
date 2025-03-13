using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZHZ.JWT
{
    public class JWTSettings
    {
        public required string Key { get; set; }

        public int ExpireSeconds { get; set; }
         public string Audience { get; set; }
        public string     Issuer { get; set; }
    }

    
}
