using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Permissions;

namespace Motr.IO
{
    public class TXTDocument
    {
        public TXTDocument() { }
        public TXTDocument(String path)
        {
            this.Path = path;
        }
        /// <summary>
        /// 路径
        /// </summary>
        public String Path { get; set; }
        /// <summary>
        /// 读取文本
        /// </summary>
        /// <returns></returns>
        public String ReadText()
        {
            using (StreamReader streamReader = new StreamReader(Path, Encoding.Default))
            {
                String text = streamReader.ReadToEnd();
                streamReader.Close();
                return text;
            }
        }
        /// <summary>
        /// 追加文本
        /// </summary>
        /// <param name="text"></param>
        public void Append(String text)
        {
            Append(text, false);
        }
        /// <summary>
        /// 追加一行文本
        /// </summary>
        /// <param name="text"></param>
        public void AppendLine(String text)
        {
            Append(text, true);
        }
        /// <summary>
        /// 追加文本
        /// </summary>
        /// <param name="text"></param>
        /// <param name="isLine"></param>
        private void Append(String text, Boolean isLine)
        {
            using (StreamWriter streamWriter = new StreamWriter(Path, true))
            {
                if (isLine)
                    streamWriter.WriteLine(text);
                else
                    streamWriter.Write(text);
                streamWriter.Close();
            }
        }
        /// <summary>
        /// 替换某个文本文件中的特定内容
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        public void ReplaceText(String oldValue, String newValue)
        {
            String text = String.Empty;
            using (StreamReader reader = new StreamReader(this.Path, Encoding.Default))
            {
                text = reader.ReadToEnd();
                reader.Close();
            }
            using (StreamWriter writer = new StreamWriter(this.Path, false))
            {
                writer.Write(text.Replace(oldValue, newValue));
                writer.Close();
            }
        }
    }
}
