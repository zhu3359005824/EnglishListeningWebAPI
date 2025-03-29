using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalConfigurations
{
    public class MyResponseData
    {
        public MyResponseData() { }
       
        public int Code { get; set; }
        public string Message { get; set; }
        //public string Message { get; set; }
        public object Data { get; set; }

    }
}
