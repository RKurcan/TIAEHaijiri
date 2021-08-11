using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTech.Application.Utility.AcivityManager
{
    public enum ResultStatus
    {
        Success,
        ApplicationWarning,
        ApplicationError,
        ExceptionOccure,
        InsufficientRight,
        ModuleNotSigned,
        KeyExpire
    }
}
