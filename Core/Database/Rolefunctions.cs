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
    
    public partial class Rolefunctions
    {
        public decimal ID { get; set; }
        public decimal Role_ID { get; set; }
        public decimal Functions_ID { get; set; }
    
        public virtual Functions Functions { get; set; }
        public virtual Role Role { get; set; }
    }
}
