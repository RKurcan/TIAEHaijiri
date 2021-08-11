using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.HRM.Entities.Training
{
    public class ESession
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(200)]
        public string Name { get; set; }
        public int CourseId { get; set; }
        [StringLength(100)]
        public string Duration { get; set; }
        [StringLength(500)]
        public string Location { get; set; }
        public Method Method { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        public int BranchId { get; set; }

        public virtual ECourse Course { get; set; }
    }
    public enum Method
    {
        Classroom,
        SelfStudy,
        WebEx
    }
}
