using Riddhasoft.User.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.Entity.User
{
    public class EContext
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime LastLogin { get; set; }
        public TimeSpan TimeOut { get; set; }
        public string Token { get; set; }

        public virtual EUser User { get; set; }
    }
    
}
