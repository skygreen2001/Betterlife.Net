/**
 * 
 * The following step by step example will demonstrate how you can use Aspect Oriented Programming (AOP) 
 * to cast a collection of objects into a DataTable. 
 * This particular example will leverage the power of attributes, generics, and reflection 
 * to explicitly convert a collection of container classes into a DataTable.
 * 
 * Author:Joseph Finsterwald
 * 
 */
using System;

namespace Util.DataType.Datatable
{
    /// <summary>
    /// Casting from a Collection to a Data Table using Generics and Attributes
    /// Step 1: Build your own Custom Attribute Class
    ///     Custom Attributes always inherit from System.Attribute, in fact any class that inherits from System.Attribute whether directly or indirectly is an attribute class.
    ///     Attribute classes also follow the convention of having the word{Attribute}attached as a suffix to the class name.
    ///     Attributes allow you to add metadata to an object that you can read at run-time via reflection. As a result, they provide an elegant (and granular) solution to 
    ///     the object oriented problem of cross-cutting concerns.
    ///     Our first step will be to build a custom attribute class that will allow us to acquire meta data about the properties 
    ///     of our container class (which we haven�t built yet) at run-time. The beauty of this solution is that we can use our custom attribute class (in this case ConversionAttribute) 
    ///     to decorate any class that we decide to add to our project at a later date.
    /// </summary>
    /// <see cref="http://www.codeproject.com/KB/cs/coreweb01.aspx"/>
    [AttributeUsage(AttributeTargets.Property)]
    class ConversionAttribute:Attribute
    {
        private bool m_dataTableConversion;
        private bool m_allowDbNull;
        private bool m_keyField;
        public ConversionAttribute() { }
        public bool DataTableConversion
        {
            get { return m_dataTableConversion; }
            set { m_dataTableConversion = value; }
        }
        public bool AllowDbNull
        {
            get { return m_allowDbNull; }
            set { m_allowDbNull = value; }
        }
        public bool KeyField
        {
            get { return m_keyField; }
            set { m_keyField = value; }
        }

    }
}
