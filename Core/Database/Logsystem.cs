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
    
    public partial class Logsystem
    {
        public System.Guid ID { get; set; }
        public Nullable<System.DateTime> Logtime { get; set; }
        public string Ident { get; set; }
        public string Priority { get; set; }
        public string Message { get; set; }
    }
}
