using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Motr
{
    public static class ExString
    {
        /// <summary>
        /// 从字符串左边截取制定长度的字符串
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <param name="length">截取长度</param>
        /// <returns>截取结果</returns>
        public static String Left(this String str, Int32 length)
        {
            if (String.IsNullOrEmpty(str)) return str;
            if (str.Length < length) return str;
            return str.Substring(0, length);
        }
        /// <summary>
        /// 从字符串右边截取制定长度的字符串
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <param name="length">截取长度</param>
        /// <returns>截取结果</returns>
        public static String Right(this String str, Int32 length)
        {
            if (String.IsNullOrEmpty(str)) return str;
            if (str.Length < length) return str;
            return str.Substring(str.Length - length);
        }
        /// <summary>
        /// 格式化参数
        /// </summary>
        /// <param name="s"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static String Format(this String str, params String[] args)
        {
            if (args == null || args.Length == 0)  throw new Exception("args can not is null");
            if (str == null) return str;
            Int32 capacity = str.Length + args.Where(o => o != null).Sum(o => o.Length);
            StringBuilder build = new StringBuilder(capacity);
            return build.AppendFormat(str, args).ToString();
        }
        /// <summary>
        /// 替换第一个出现的第一个oldValue
        /// </summary>
        /// <param name="str"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public static String ReplaceFirst(this String str, String oldValue, String newValue)
        {
            if (str == null) return str;
            Int32 firstIndex = str.IndexOf(oldValue);
            if (firstIndex == -1) return str;
            return str.Remove(firstIndex, oldValue.Length).Insert(firstIndex, newValue);
        }
        /// <summary>
        /// 替换第一个出现的第一个oldValue
        /// </summary>
        /// <param name="strBuilder"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public static StringBuilder ReplaceFirst(this StringBuilder strBuilder, String oldValue, String newValue)
        {
            Int32 firstIndex = strBuilder.ToString().IndexOf(oldValue);
            if (firstIndex == -1) return strBuilder;
            return strBuilder.Remove(firstIndex, oldValue.Length).Insert(firstIndex, newValue);
        }
        /// <summary>
        /// 重复单字符生成输出
        /// </summary>
        /// <param name="c">一个字符</param>
        /// <param name="count">重复数</param>
        /// <returns></returns>
        public static String Repeat(this char c, Int32 count)
        {
            return string.Empty.PadLeft(count, c);
        }
        /// <summary>
        /// 重复字符串生成输出
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="count">重复数</param>
        /// <returns></returns>
        public static String Repeat(this String str, Int32 count)
        {
            return String.Join(string.Empty, Enumerable.Repeat<String>(str, count).ToArray());
        }
    }
}