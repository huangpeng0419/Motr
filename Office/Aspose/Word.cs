using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aspose.Words;
using System.IO;

namespace Motr.Office
{
   public class Word
    {
       public Document Doc { get; set; }
       public String FileName { get; set; }
       public String ImagesFolder { get; set; }
       public Word(String fileName)
       {
           FileName = fileName;
       }
       public String ToHtml()
       {
           Doc.SaveOptions.HtmlExportImagesFolder = ImagesFolder;
           using (MemoryStream ms = new MemoryStream())
           {
               Doc.Save(ms, SaveFormat.Html);
               byte[] b = new byte[ms.Length];
               ms.Read(b,0,b.Length);
               return Encoding.UTF8.GetString(b);
           }
       }
    }
}
