//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RTech.Demo.WdmsData
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public partial class user_priv
    {
        public user_priv()
        {
            this.auth_user = new HashSet<auth_user>();
        }
        [Key]
        public int Privilege { get; set; }
        public string PrivName { get; set; }
    
        public virtual ICollection<auth_user> auth_user { get; set; }
    }
}
