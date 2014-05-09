using System;
using System.IO;
using System.Text;
using System.Xml;

namespace Util.xml
{
    // helper class to omit XML decl at start of document when serializing
    public class XTWFND : XmlTextWriter
    {
        public XTWFND(System.IO.TextWriter w) : base(w) { Formatting = System.Xml.Formatting.Indented; }

        public XTWFND(Stream w, Encoding encoding)
            : base(w, encoding)
        {

        }
        public XTWFND(string filename, Encoding encoding)
            : base(filename, encoding)
        {

        }

        //public override void WriteStartDocument() { }
    }
}
