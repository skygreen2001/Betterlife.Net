using System;
using System.IO;
using System.Xml;

namespace Util.xml
{
    // helper class to ignore namespaces when de-serializing
    public class NamespaceIgnorantXmlTextReader : XmlTextReader
    {
        public NamespaceIgnorantXmlTextReader(System.IO.TextReader reader) : base(reader) { }
        protected NamespaceIgnorantXmlTextReader()
        {

        }
        protected NamespaceIgnorantXmlTextReader(XmlNameTable nt)
            : base(nt)
        {

        }
        public NamespaceIgnorantXmlTextReader(Stream input)
            : base(input)
        {

        }
        public NamespaceIgnorantXmlTextReader(string url, Stream input)
            : base(url, input)
        {

        }
        public NamespaceIgnorantXmlTextReader(Stream input, XmlNameTable nt)
            : base(input, nt)
        {

        }
        public NamespaceIgnorantXmlTextReader(string url, Stream input, XmlNameTable nt)
            : base(url, input, nt)
        {

        }
        public NamespaceIgnorantXmlTextReader(string url, TextReader input)
            : base(url, input)
        {

        }
        public NamespaceIgnorantXmlTextReader(TextReader input, XmlNameTable nt)
            : base(input, nt)
        {

        }
        public NamespaceIgnorantXmlTextReader(string url, TextReader input, XmlNameTable nt)
            : base(url, input, nt)
        {

        }
        public NamespaceIgnorantXmlTextReader(Stream xmlFragment, XmlNodeType fragType, XmlParserContext context)
            : base(xmlFragment, fragType, context)
        {

        }
        public NamespaceIgnorantXmlTextReader(string xmlFragment, XmlNodeType fragType, XmlParserContext context)
            : base(xmlFragment, fragType, context)
        {

        }
        public NamespaceIgnorantXmlTextReader(string url)
            : base(url)
        {

        }
        public NamespaceIgnorantXmlTextReader(string url, XmlNameTable nt)
            : base(url, nt)
        {

        }


        public override string NamespaceURI
        {
            get { return ""; }
        }
    }
}
