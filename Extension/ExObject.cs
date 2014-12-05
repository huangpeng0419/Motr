using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;

namespace Motr
{
    public static class ExObject
    {
        /// <summary>
        /// 深复制
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <returns></returns>
        public static T DeepCopy<T>(this Object o)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, o);
                ms.Seek(0, 0);
                return (T)bf.Deserialize(ms);
            }
        }
        /// <summary>
        /// 获取类型特性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static T GetAttr<T>(this Type t)
        {
            Object[] o = t.GetCustomAttributes(typeof(T), true);
            if (o.Length == 0) return default(T);
            return (T)o[0];
        }
        /// <summary>
        /// 获取属性特性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pi"></param>
        /// <returns></returns>
        public static T GetAttr<T>(this PropertyInfo pi)
        {
            Object[] o = pi.GetCustomAttributes(typeof(T), true);
            if (o.Length == 0) return default(T);
            return (T)o[0];
        }
    }
}
