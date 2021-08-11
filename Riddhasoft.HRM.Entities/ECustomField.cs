using Riddhasoft.OfficeSetup.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.HRM.Entities
{
    public class ECustomField
    {
        [Key]
        public int Id { get; set; }
        public string FieldName { get; set; }
        public FieldType FieldType { get; set; }
        public int Length { get; set; }
        public int CompanyId { get; set; }

        public virtual ECompany Company { get; set; }

    }

    public enum FieldType
    {
        Text
    }
}
