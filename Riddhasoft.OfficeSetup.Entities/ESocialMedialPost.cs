using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.OfficeSetup.Entities
{
    public class ESocialMedialPost
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string PhotoURL { get; set; }
        public bool Publish { get; set; }
        public int PublishDuration { get; set; }
    }
}
