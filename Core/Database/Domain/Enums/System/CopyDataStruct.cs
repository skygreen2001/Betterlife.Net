using System;
using System.Runtime.InteropServices;

namespace Database.Domain.Enums.System
{
    //WM_COPYDATA消息所要求的数据结构
    public struct CopyDataStruct
    {
        public IntPtr dwData;
        public int cbData;

        [MarshalAs(UnmanagedType.LPStr)]
        public string lpData;
    }
}
