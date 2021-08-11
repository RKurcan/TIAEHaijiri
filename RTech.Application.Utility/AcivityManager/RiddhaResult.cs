using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTech.Application.Utility.AcivityManager
{
    public class RiddhaResult<data>
    {
        public ResultStatus Status { get; set; }
        public string Message { get; set; }
        public data Data { get; set; }

    }
}
