using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Linq.Expressions;
using System.Xml.Serialization;

namespace Motr
{
    public static class ExTransform
    {
        /// <summary>
        /// 获取表达式的值
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public static Object Compile(this Expression expr)
        {
            if (expr is ConstantExpression) return (expr as ConstantExpression).Value;
            if (expr is MethodCallExpression)
            {
                var methodCallExpr = (expr as MethodCallExpression);
            }
            switch (expr.Type.Name)
            {
                case "String": return Expression.Lambda<Func<String>>(expr).Compile()();
                case "Int32": return Expression.Lambda<Func<Int32>>(expr).Compile()();
                case "Int16": return Expression.Lambda<Func<Int16>>(expr).Compile()();
                case "Int64": return Expression.Lambda<Func<Int64>>(expr).Compile()();
                case "Byte": return Expression.Lambda<Func<Byte>>(expr).Compile()();
                case "Double": return Expression.Lambda<Func<Double>>(expr).Compile()();
                case "Single": return Expression.Lambda<Func<Single>>(expr).Compile()();
                case "Decimal": return Expression.Lambda<Func<Decimal>>(expr).Compile()();
                case "Boolean": return Expression.Lambda<Func<Boolean>>(expr).Compile()();
                case "Guid": return Expression.Lambda<Func<Guid>>(expr).Compile()();
                case "DateTime": return Expression.Lambda<Func<DateTime>>(expr).Compile()();
                case "List`1":
                    if (expr.Type.FullName == typeof(List<Int32>).FullName) return Expression.Lambda<Func<List<Int32>>>(expr).Compile()();
                    else if (expr.Type.FullName == typeof(List<Int16>).FullName) return Expression.Lambda<Func<List<Int16>>>(expr).Compile()();
                    else if (expr.Type.FullName == typeof(List<Int64>).FullName) return Expression.Lambda<Func<List<Int64>>>(expr).Compile()();
                    else if (expr.Type.FullName == typeof(List<Byte>).FullName) return Expression.Lambda<Func<List<Byte>>>(expr).Compile()();
                    else if (expr.Type.FullName == typeof(List<Double>).FullName) return Expression.Lambda<Func<List<Double>>>(expr).Compile()();
                    else if (expr.Type.FullName == typeof(List<Single>).FullName) return Expression.Lambda<Func<List<Single>>>(expr).Compile()();
                    else if (expr.Type.FullName == typeof(List<Decimal>).FullName) return Expression.Lambda<Func<List<Decimal>>>(expr).Compile()();
                    else if (expr.Type.FullName == typeof(List<Boolean>).FullName) return Expression.Lambda<Func<List<Boolean>>>(expr).Compile()();
                    else if (expr.Type.FullName == typeof(List<Guid>).FullName) return Expression.Lambda<Func<List<Guid>>>(expr).Compile()();
                    else if (expr.Type.FullName == typeof(List<DateTime>).FullName) return Expression.Lambda<Func<List<DateTime>>>(expr).Compile()();
                    else if (expr.Type.FullName == typeof(List<String>).FullName) return Expression.Lambda<Func<List<String>>>(expr).Compile()();
                    break;
            }
            if (expr.Type.IsEnum)
            {
                var memberExpr = (expr as MemberExpression);
                var val = Expression.Lambda(expr).Compile().DynamicInvoke();
                if (Enum.IsDefined(expr.Type, val))
                    return Convert.ChangeType(val, Enum.GetUnderlyingType(expr.Type));
                return val.ToString();
                // memberExpr.Expression.GetType().InvokeMember()
                //  memberExpr.Member
            }
            throw new Exception("error");
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public static void Serialize<T>(this T t, String path)
        {
            if (t == null) return;
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, t);
            }
        }
        /// <summary>
        /// 反序列化 根据路径
        /// </summary>
        /// <returns></returns>
        public static T Deserialize<T>(this String path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                BinaryFormatter bf = new BinaryFormatter();
                return (T)bf.Deserialize(fs);
            }
        }
        /// <summary>
        /// 将对象序列化为Xml字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static String ToXml(this object o)
        {
            if (o == null) return String.Empty;
            var xs = new XmlSerializer(o.GetType());
            using (TextWriter tw = new StringWriter())
            {
                xs.Serialize(tw, o);
                return tw.ToString().Replace("utf-16", "utf-8");
            }
        }
        /// <summary>
        /// 将Xml字符串序列化为对象
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public static T ToObjectFromXmlString<T>(this String xmlString)
        {
            if (String.IsNullOrEmpty(xmlString)) return default(T);
            var xs = new XmlSerializer(typeof(T));
            using (TextReader tr = new StringReader(xmlString))
            {
                return (T)xs.Deserialize(tr);
            }
        }
        /// <summary>
        /// 通过Xml路径序列化为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlPath"></param>
        /// <returns></returns>
        public static T ToObjectFromXmlPath<T>(this String xmlPath)
        {
            return File.ReadAllText(xmlPath).ToObjectFromXmlString<T>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static String ToTxt(this Stream stream)
        {
            using(var sr =  new  StreamReader(stream,Encoding.UTF8))
            {
                return sr.ReadToEnd();
            }
        }
        /// <summary>
        /// 转换类型，如不能转换返回T的默认值
        /// </summary>
        /// <typeparam name="T">指定类型</typeparam>
        /// <param name="wait">待转化值</param>
        /// <returns></returns>
        public static T Parse<T>(this Object waitVal)
        {
            return waitVal.Parse<T>(default(T));
        }

        /// <summary>
        /// 转换类型
        /// </summary>
        /// <typeparam name="T">指定类型</typeparam>
        /// <param name="wait">待转化值</param>
        /// <returns></returns>
        public static T Parse<T>(this Object waitVal, T defaultVal)
        {
            try
            {
                if (typeof(T).IsEnum) return (T)Enum.Parse(typeof(T), waitVal.ToString());
                return (T)Convert.ChangeType(waitVal, typeof(T));
            }
            catch { return defaultVal; }
        }

        /// <summary>
        /// 人民币转为大写方式
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static String ToRMB(this double money)
        {
            String s = money.ToString("#L#E#D#C#K#E#D#C#J#E#D#C#I#E#D#C#H#E#D#C#G#E#D#C#F#E#D#C#.0B0A");
            String d = Regex.Replace(s, @"((?<=-|^)[^1-9]*)|((?'z'0)[0A-E]*((?=[1-9])|(?'-z'(?=[F-L\.]|$))))|((?'b'[F-L])(?'z'0)[0A-L]*((?=[1-9])|(?'-z'(?=[\.]|$))))", "${b}${z}");
            return Regex.Replace(d, ".", m => "负元空零壹贰叁肆伍陆柒捌玖空空空空空空空分角拾佰仟萬億兆京垓秭穰"[m.Value[0] - '-'].ToString());
        }
    }
}
