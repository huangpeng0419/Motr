using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Motr.Net.Http
{
    /// <summary>
    /// 
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        public static byte[] BuildPostData(this List<Parameter> par)
        {
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        public static String ToQueryString(this List<Parameter> par)
        {
            if (par == null || par.Count == 0) return String.Empty;
            return String.Join("&", par.Select(o => String.Format("{0}={1}", Uri.EscapeDataString(o.Name), Uri.EscapeDataString(o.Value.ToString()))).ToArray());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        public static String ToQueryString(this Dictionary<String, Object> par)
        {
            if (par == null || par.Count == 0) return String.Empty;
            return String.Join("&", par.Select(o => String.Format("{0}={1}", Uri.EscapeDataString(o.Key), Uri.EscapeDataString(o.Value.ToString()))).ToArray());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public static List<Parameter> ToListParameter(this String queryString)
        {
            if (String.IsNullOrEmpty(queryString)) return null;
            var list = new List<Parameter>();
            String[] keyValue = null;
            foreach (var item in queryString.Split('&'))
            {
                keyValue = item.Split('=');
                list.Add(new Parameter() { Name = keyValue[0], Value = keyValue[1] });
            }
            return list;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public static Dictionary<String, String> ToDictionary(this String queryString)
        {
            if (String.IsNullOrEmpty(queryString)) return null;
            Dictionary<String, String> dic = new Dictionary<String, String>();
            String[] keyValue = null;
            foreach (var item in queryString.Split('&'))
            {
                keyValue = item.Split('=');
                dic.Add(keyValue[0], keyValue[1]);
            }
            return dic;
        }
    }
}
