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
    
    public partial class Comment
    {
        public System.Guid ID { get; set; }
        public System.Guid User_ID { get; set; }
        public string Comment1 { get; set; }
        public System.Guid Blog_ID { get; set; }
        public Nullable<System.DateTime> Committime { get; set; }
        public Nullable<System.DateTime> Updatetime { get; set; }
    
        public virtual Blog Blog { get; set; }
        public virtual User User { get; set; }
    }
}
