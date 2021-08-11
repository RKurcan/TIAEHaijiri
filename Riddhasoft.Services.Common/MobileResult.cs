using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.Services.Common
{
    public class MobileResult<T>
    {
        public MobileResultStatus Status { get; set; }
        public T Data { get; set; }
        public string Token { get; set; }
        public string Message { get; set; }
    }
    public enum MobileResultStatus
    {
        InvalidToken,
        Ok,
        ParameterError,
        DatabaseConnectionError,
        ProcessError,
        UnhandledException
    }
}
