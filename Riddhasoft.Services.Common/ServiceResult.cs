using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.Services.Common
{
    public class ServiceResult<t>
    {
        public ResultStatus Status { get; set; }
        public string Message { get; set; }
        public t Data { get; set; }
    }
    public class KendoGridResult<t>
    {
        public ResultStatus Status { get; set; }
        public string Message { get; set; }
        public t Data { get; set; }
        public int TotalCount { get; set; }
    }
    public enum ResultStatus
    {
        processError,
        dataBaseError,
        ComError,
        unHandeledError,
        Ok,
        InvalidToken
    }
}
