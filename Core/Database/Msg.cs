//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class Msg
    {
        public System.Guid ID { get; set; }
        public System.Guid Sender_ID { get; set; }
        public System.Guid Receiver_ID { get; set; }
        public string Sendername { get; set; }
        public string Receivername { get; set; }
        public string Content { get; set; }
        public Nullable<short> Status { get; set; }
        public Nullable<System.DateTime> Committime { get; set; }
        public Nullable<System.DateTime> Updatetime { get; set; }
    
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
    }
}
