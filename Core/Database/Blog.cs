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
    
    public partial class Blog
    {
        public Blog()
        {
            this.Comment = new HashSet<Comment>();
        }
    
        public decimal ID { get; set; }
        public decimal User_ID { get; set; }
        public string Blog_Name { get; set; }
        public string Blog_Content { get; set; }
        public Nullable<System.DateTime> Committime { get; set; }
        public Nullable<System.DateTime> Updatetime { get; set; }
    
        public virtual User User { get; set; }
        public virtual ICollection<Comment> Comment { get; set; }
    }
}
