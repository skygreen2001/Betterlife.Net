//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class Userdetail
    {
        public decimal ID { get; set; }
        public decimal User_ID { get; set; }
        public string Realname { get; set; }
        public string Profile { get; set; }
        public Nullable<int> Country { get; set; }
        public Nullable<int> Province { get; set; }
        public Nullable<int> City { get; set; }
        public Nullable<int> District { get; set; }
        public string Address { get; set; }
        public string Qq { get; set; }
        public Nullable<short> Sex { get; set; }
        public Nullable<System.DateTime> Birthday { get; set; }
        public Nullable<System.DateTime> Committime { get; set; }
        public Nullable<System.DateTime> Updatetime { get; set; }
    
        public virtual User User { get; set; }
    }
}
