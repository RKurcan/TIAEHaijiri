using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RTech.Demo.Models
{
    public class DropdownViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string NameNp { get; set; }
        public bool IsManager { get; set; }
    }
}