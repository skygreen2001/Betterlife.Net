using System.Collections.Generic;
using System.Data;
using Util.DataType.Datatable;

namespace Util.DataType.Datatable
{
    /// <summary>
    /// Casting from a Collection to a Data Table using Generics and Attributes
    /// Step 5: Build your own Generic List
    /// </summary>
    /// <see cref="http://www.codeproject.com/KB/cs/coreweb01.aspx"/>
    /// <typeparam name="T"></typeparam>
    public class MyList<T> : List<T>
    {
        private static bool m_enforceKeysInDataTableConversion;

        public MyList()
        {
            init();
        }

        public MyList(List<T> list)
        {
            init();
            if (list != null&&list.Count> 0)
            {
                foreach (T member in list)
                {
                    this.Add(member);
                }
            }
        }

        public void init()
        {
            m_enforceKeysInDataTableConversion = false;
        }

        public bool EnforceKeysInDataTableConversion
        {
            get { return m_enforceKeysInDataTableConversion; }
            set { m_enforceKeysInDataTableConversion = value; }
        }

        public static explicit operator DataTable(MyList<T> list)
        {
            IDataTableConverter<T> converter = new DataTableConverter<T>(
               m_enforceKeysInDataTableConversion);

            return converter.GetDataTable(list);
        }
    }
}
