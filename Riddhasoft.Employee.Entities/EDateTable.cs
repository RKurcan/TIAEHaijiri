using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.Employee.Entities
{
    public class EDateTable
    {
        [Key]
        public int Id { get; set; }
        public string NepDate { get; set; }
        public DateTime EngDate { get; set; }
    }
}
