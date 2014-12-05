using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Xsl;

namespace Motr.Util
{
    public sealed class Xsl
    {
        private XslCompiledTransform _xct;
        public Xsl(string xslString)
        {
            this._xct = new XslCompiledTransform();
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(xslString));
            var xr = XmlReader.Create(ms);
            this._xct.Load(xr);
            ms.Dispose();
            xr.Close();
        }
        public String Transform(String xmlString)
        {
            using (StringWriter sw = new StringWriter())
            {
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(xmlString));
                var xr = XmlReader.Create(ms);
                this._xct.Transform(xr, null, sw);
                ms.Dispose();
                xr.Close();
                return sw.ToString();
            }
        }
    }
}
