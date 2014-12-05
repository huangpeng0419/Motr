using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
using Motr.Data.Mapping;
namespace Motr.Data
{
    /// <summary>
    /// Db Execution
    /// </summary>
    public static class DbExtension
    {
        #region IDataReader Converter
        /// <summary>
        /// IDataReader转字典集合
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static IList<Dictionary<String, Object>> ToDictionarys(this IDataReader reader)
        {
            List<Dictionary<String, Object>> dicList = new List<Dictionary<String, Object>>();
            Dictionary<String, Object> dic;
            while ((dic = reader.ToDictionary()) != null)
                dicList.Add(dic);
            return dicList;
        }

        /// <summary>
        /// IDataReader转字典
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static Dictionary<String, Object> ToDictionary(this IDataReader reader)
        {
            if (!reader.Read()) return null;
            Dictionary<String, Object> dic = new Dictionary<String, Object>();
            for (Int32 i = 0; i < reader.FieldCount; i++)
                dic.Add(reader.GetName(i), reader[i]);
            return dic;
        }

        /// <summary>
        /// IDataReader转实体对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader">reader</param>
        /// <param name="columnNameList">列名集合</param>
        /// <param name="propArray">属性数组</param>
        /// <returns></returns>
        public static T ToEntity<T>(this IDataReader reader, List<String> columnNameList, Dictionary<String, PropertyInfo> propArray)
        {
            if (!reader.Read()) return default(T);
            T t = Activator.CreateInstance<T>();
            foreach (var pi in propArray)
            {

                if (pi.Value.CanWrite == false || columnNameList.Contains(pi.Key) == false) continue;
                Object v = reader[pi.Key];
                pi.Value.SetValue(t, v != DBNull.Value ? (pi.Value.PropertyType.IsEnum ? Enum.Parse(pi.Value.PropertyType, v.ToString()) : v) : null, null);
            }
            return t;
        }

        /// <summary>
        /// IDataReader转实体对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static T ToEntity<T>(this IDataReader reader)
        {
            return reader.ToEntity<T>(reader.GetColumnNameList(), typeof(T).GetProperties().ToDictionary(o => o.Name.ToLower()));
        }

        /// <summary>
        /// IDataReader转实体对象集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this IDataReader reader)
        {
            var list = new List<T>();
            T t = default(T);
            var columnNameList = reader.GetColumnNameList();
            var propArray = typeof(T).GetProperties().ToDictionary(o => o.Name.ToLower());
            while ((t = reader.ToEntity<T>(columnNameList, propArray)) != null)
                list.Add(t);
            return list;
        }

        /// <summary>
        /// 收集reader的所有字段名称
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static List<String> GetColumnNameList(this IDataReader reader)
        {
            var columnNameList = new List<String>();
            for (Int32 i = 0; i < reader.FieldCount; i++)
            {
                columnNameList.Add(reader.GetName(i).ToLower());
            }
            return columnNameList;
        }
        #endregion

        #region DataTable DataRow Converter
        /// <summary>
        /// DataRow转实体对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="row"></param>
        /// <param name="columnNameList"></param>
        /// <param name="propArray"></param>
        /// <returns></returns>
        public static T ToEntity<T>(this DataRow row, List<String> columnNameList, Dictionary<String, PropertyInfo> propArray)
        {
            if (row == null) return default(T);
            T t = Activator.CreateInstance<T>();
            foreach (var pi in propArray)
            {
                if (!pi.Value.CanWrite || !columnNameList.Contains(pi.Key)) continue;
                Object v = row[pi.Key];
                pi.Value.SetValue(t, v != DBNull.Value ? (pi.Value.PropertyType.IsEnum ? Enum.Parse(pi.Value.PropertyType, v.ToString()) : v) : null, null);
            }
            return t;
        }

        /// <summary>
        /// DataRow转实体对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="row"></param>
        /// <returns></returns>
        public static T ToEntity<T>(this DataRow row)
        {
            return row.ToEntity<T>(row.Table.GetColumnNameList(), typeof(T).GetProperties().ToDictionary(o => o.Name.ToLower()));
        }

        /// <summary>
        /// DataTable转实体对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <returns></returns>
        public static T ToEntity<T>(this DataTable table)
        {
            if (table == null || table.Rows.Count == 0) return default(T);
            return table.Rows[0].ToEntity<T>();
        }

        /// <summary>
        /// DataTable转实体对象集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this DataTable table)
        {
            var list = new List<T>();
            if (table == null || table.Rows.Count == 0) return list;
            var columnNameList = table.GetColumnNameList();
            var propArray = typeof(T).GetProperties().ToDictionary(o => o.Name.ToLower());
            foreach (DataRow row in table.Rows)
                list.Add(row.ToEntity<T>(columnNameList, propArray));
            return list;
        }

        /// <summary>
        /// 收集reader的所有字段名称
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static List<String> GetColumnNameList(this DataTable table)
        {
            var columnNameList = new List<String>();
            for (Int32 i = 0; i < table.Columns.Count; i++)
            {
                columnNameList.Add(table.Columns[i].ColumnName.ToLower());
            }
            return columnNameList;
        }
        #endregion

        #region Other Converter
        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Object DbValueConverter(this Object val, Type type)
        {
            if (val == null) return DBNull.Value;
            if (type.IsEnum)
            {
                if (Enum.IsDefined(type, val))   return Convert.ChangeType(val, Enum.GetUnderlyingType(type));
                return val.ToString();
            }
            else if (!type.IsGenericType) return Convert.ChangeType(val, type);
            return val;
        }
        #endregion
    }
}
