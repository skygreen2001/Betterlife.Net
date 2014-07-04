using System.Collections.Generic;
using System.Data;

namespace Util.DataType.Datatable
{
    /// <summary>
    /// Casting from a Collection to a Data Table using Generics and Attributes
    /// Step 3: Create an Interface - IDataTableConverter
    /// </summary>
    /// <see cref="http://www.codeproject.com/KB/cs/coreweb01.aspx"/>
    interface IDataTableConverter<T>
    {
        DataTable GetDataTable(List<T> items);
    }
}
