using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;
using System.Linq.Expressions;
using Motr.Data.Builder;

namespace Motr.Data
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DbQuery<T>
    {
        /// <summary>
        /// 访问数据源上下文
        /// </summary>
        private Db _context;
        /// <summary>
        /// 事务
        /// </summary>
        private DbTransaction _tran;
        /// <summary>
        /// sql生成器
        /// </summary>
        public IBuilder<T> SqlBuilder { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public DbQuery(Db context)
        {
            this._context = context;
            switch (context.ProviderName)
            {
                case "System.Data.SqlClient": this.SqlBuilder = new SqlClientBuilder<T>(context); break;
                case "System.Data.SQLite": this.SqlBuilder = new SQLiteBuilder<T>(context); break;
            }
        }
        /// <summary>
        /// 附上事务
        /// </summary>
        /// <param name="tran"></param>
        /// <returns></returns>
        public DbQuery<T> AttachDbTrans(DbTransaction tran)
        {
            this._tran = tran;
            return this;
        }
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public Int32 Insert(T t)
        {
            SqlBuilder.Insert(t);
            return ExecuteScalar<Int32>();
        }
       
        /// <summary>
        /// 选择字段;匿名类方式选择 o=> new{o.x,o.xx,o.xxx}
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public DbQuery<T> Select(Expression<Func<T, Object>> selector)
        {
            SqlBuilder.Select(selector);
            return this;
        }

       
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public Boolean Update(Expression<Func<T, Boolean>> predicate, T t)
        {
            SqlBuilder.Where(predicate);
            SqlBuilder.Update(t);
            return ExecuteNonQuery() > 0;
        }
        /// <summary>
        /// 更新(部分)
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="expr"></param>
        /// <returns></returns>
        public Int32 Update(Expression<Func<T, Boolean>> predicate, Expression<Func<T, Object>> expr)
        {
            SqlBuilder.Where(predicate);
            SqlBuilder.Update(expr);
            return ExecuteNonQuery();
        }
        /// <summary>
        /// 更新(部分)
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public DbQuery<T> Update(Expression<Func<T, Object>> expr)
        {
            SqlBuilder.Update(expr);
            return this;
        }
        /// <summary>
        /// 查找一行记录
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public T Find(Expression<Func<T, Boolean>> predicate)
        {
            SqlBuilder.Top(1);
            SqlBuilder.Where(predicate);
            return ToEntity();
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public Int32 Delete(Expression<Func<T, Boolean>> predicate)
        {
            SqlBuilder.Delete(predicate);
            return ExecuteNonQuery();
        }
        /// <summary>
        /// 排序(ASC)
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public DbQuery<T> OrderBy<K>(Expression<Func<T, K>> expr)
        {
            SqlBuilder.OrderBy(expr);
            return this;
        }
        /// <summary>
        /// 排序(DESC)
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public DbQuery<T> OrderByDescending<K>(Expression<Func<T, K>> expr)
        {
            SqlBuilder.OrderByDescending(expr);
            return this;
        }
        /// <summary>
        /// 剔除K重复
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <param name="expr"></param>
        /// <returns></returns>
        public DbQuery<T> Distinct<K>(Expression<Func<T, K>> expr)
        {
            SqlBuilder.Distinct<K>(expr);
            return this;
        }
        /// <summary>
        /// 求K最大
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <param name="expr"></param>
        /// <returns></returns>
        public DbQuery<T> Max<K>(Expression<Func<T, K>> expr)
        {
            SqlBuilder.Max<K>(expr);
            return this;
        }
        /// <summary>
        /// 求K最小
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <param name="expr"></param>
        /// <returns></returns>
        public DbQuery<T> Min<K>(Expression<Func<T, K>> expr)
        {
            SqlBuilder.Min<K>(expr);
            return this;
        }
        /// <summary>
        /// 求K和
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <param name="expr"></param>
        /// <returns></returns>
        public DbQuery<T> Sum<K>(Expression<Func<T, K>> expr)
        {
            SqlBuilder.Sum<K>(expr);
            return this;
        }
        /// <summary>
        /// 求K平均
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <param name="expr"></param>
        /// <returns></returns>
        public DbQuery<T> Avg<K>(Expression<Func<T, K>> expr)
        {
            SqlBuilder.Avg<K>(expr);
            return this;
        }
        /// <summary>
        /// Top n
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public DbQuery<T> Top(Int32 n)
        {
            SqlBuilder.Top(n);
            return this;
        }
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public DbQuery<T> Pager(Int32 pageSize, Int32 pageIndex)
        {
            SqlBuilder.Pager(pageSize, pageIndex);
            return this;
        }
        /// <summary>
        /// 查询条件
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public DbQuery<T> Where(Expression<Func<T, Boolean>> predicate)
        {
            SqlBuilder.Where(predicate);
            return this;
        }
        #region Execute
        /// <summary>
        /// 统计计数
        /// </summary>
        /// <returns></returns>
        public Int32 Count()
        {
            SqlBuilder.Action = DbAction.Count;
            return ExecuteScalar<Int32>();
        }
        /// <summary>
        /// 受影响的行数
        /// </summary>
        /// <returns></returns>
        public Int32 ExecuteNonQuery()
        {
            return _context.ExecuteNonQuery(_tran, CommandType.Text, SqlBuilder.Sql, SqlBuilder.Parameters.ToArray());
        }
        /// <summary>
        /// 一个DbDataReader
        /// </summary>
        /// <returns></returns>
        public DbDataReader ExecuteDataReader()
        {
            return _context.ExecuteReader(_tran, CommandType.Text, SqlBuilder.Sql, SqlBuilder.Parameters.ToArray());
        }
        /// <summary>
        /// 一行一列
        /// </summary>
        /// <returns></returns>
        public K ExecuteScalar<K>()
        {
            return _context.ExecuteScalar(_tran, CommandType.Text, SqlBuilder.Sql, SqlBuilder.Parameters.ToArray()).Parse<K>();
        }
        /// <summary>
        /// 数据集
        /// </summary>
        /// <returns></returns>
        public DataSet ToDataSet()
        {
            return _context.ExecuteDataSet(_tran, CommandType.Text, SqlBuilder.Sql, SqlBuilder.Parameters.ToArray());
        }
        /// <summary>
        /// 数据表
        /// </summary>
        /// <returns></returns>
        public DataTable ToDataTable()
        {
            return ToDataSet().Tables[0];
        }
        /// <summary>
        /// 数据行
        /// </summary>
        /// <returns></returns>
        public DataRow ToDataRow()
        {
            DataTable dt = ToDataTable();
            if (dt.Rows.Count == 0) return null;
            return dt.Rows[0];
        }
        /// <summary>
        /// 转为T
        /// </summary>
        /// <returns></returns>
        public T ToEntity()
        {
            var list = ToList();
            if (list.Count == 0) return default(T);
            return list[0];
        }
        /// <summary>
        /// 转为List&lt;T&lg;
        /// </summary>
        /// <returns></returns>
        public List<T> ToList()
        {
            if (this._tran != null) return ToDataTable().ToList<T>();
            DbDataReader reader = ExecuteDataReader();
            var list = reader.ToList<T>();
            reader.Close();
            reader.Dispose();
            return list;
        } 
        #endregion

    }
}
