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
    
    public partial class Region
    {
        public decimal ID { get; set; }
        public decimal Parent_ID { get; set; }
        public string Region_Name { get; set; }
        public Nullable<short> Region_Type { get; set; }
        public Nullable<System.DateTime> Committime { get; set; }
        public Nullable<System.DateTime> Updatetime { get; set; }
    }
}
