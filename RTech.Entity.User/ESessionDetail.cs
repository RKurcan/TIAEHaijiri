using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.Entity.User
{
    public class ESessionDetail
    {
        [Key]
        public int Id { get; set; }
        public int ContextId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }

        public virtual EContext Context { get; set; }
    }
}
