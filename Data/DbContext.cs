using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;
using System.Configuration;

namespace Motr.Data
{
    /// <summary>
    /// Db Context
    /// </summary>
    public partial class Db
    {
        /// <summary>
        /// 默认实例
        /// </summary>
        public static Db Instance
        {
            get { return Nested.Instance; }
        }
        /// <summary>
        /// 内部类
        /// </summary>
        private class Nested
        {
            public static Db Instance= new Db(); 
        }
        /// <summary>
        /// 
        /// </summary>
        public DbProviderFactory DbFactory { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public String ConnectionString { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public String ProviderName { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public String ParamPrefix { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        private Db() : this(ConfigurationManager.ConnectionStrings[0]) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="setting"></param>
        public Db(ConnectionStringSettings setting) : this(setting.ProviderName, setting.ConnectionString) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="connString"></param>
        public Db(String providerName, String connString)
        {
            this.ConnectionString = connString;
            this.ProviderName = providerName;
            this.DbFactory = DbProviderFactories.GetFactory(providerName);
            switch (providerName)
            {
                case "System.Data.SqlClient": ParamPrefix = "@"; break;
                case "System.Data.SQLite": ParamPrefix = "@"; break;
            }
        }
        /// <summary>
        /// 创建连接对象(Open)
        /// </summary>
        /// <returns></returns>
        public DbConnection CreateConnection()
        {
            DbConnection conn = DbFactory.CreateConnection();
            conn.ConnectionString = this.ConnectionString;
            conn.Open();
            return conn;
        }
        /// <summary>
        /// 实体操作能力
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public DbQuery<T> Query<T>()
        {
            return new DbQuery<T>(this);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        public void TransactionMethod(TransactionMethod method)
        {
            using (DbConnection conn = this.CreateConnection())
            {
                using (DbTransaction tran = conn.BeginTransaction())
                {
                    method(tran);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method"></param>
        /// <returns></returns>
        public T TransactionMethod<T>(TransactionMethod<T> method)
        {
            using (DbConnection conn = this.CreateConnection())
            {
                using (DbTransaction tran = conn.BeginTransaction())
                {
                    return method(tran);
                }
            }
        }
    }
}
