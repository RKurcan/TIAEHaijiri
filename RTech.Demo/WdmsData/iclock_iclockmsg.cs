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
    
    public partial class iclock_iclockmsg
    {
        public int id { get; set; }
        public string SN_id { get; set; }
        public int MsgType { get; set; }
        public System.DateTime StartTime { get; set; }
        public Nullable<System.DateTime> EndTime { get; set; }
        public string MsgParam { get; set; }
        public string MsgContent { get; set; }
        public Nullable<System.DateTime> LastTime { get; set; }
    
        public virtual iclock iclock { get; set; }
    }
}
