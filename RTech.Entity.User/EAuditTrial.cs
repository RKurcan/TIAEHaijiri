using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.Entity.User
{
    public class EAuditTrial
    {
        public int Id { get; set; }
        /// <summary>
        /// current menu
        /// </summary>
        public string MenuCode { get; set; }
        /// <summary>
        /// requested action
        /// </summary>
        public string ActionCode { get; set; }
        /// <summary>
        /// operating user
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// log time
        /// </summary>
        public DateTime LogTime { get; set; }
        /// <summary>
        /// system date time
        /// </summary>
        public DateTime SystemTime { get; set; }
        public string Message { get; set; }

        public int TargetId { get; set; }
    }
}
