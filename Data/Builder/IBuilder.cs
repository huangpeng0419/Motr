using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Data.Common;

namespace Motr.Data.Builder
{
    public interface IBuilder<T>
    {
        /// <summary>
        /// 动作
        /// </summary>
        DbAction Action { get; set; }
        /// <summary>
        /// 生成Sql
        /// </summary>
        String Sql { get;  }
        /// <summary>
        /// 参数
        /// </summary>
        List<DbParameter> Parameters { get; set; }
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        IBuilder<T> Insert(T t);
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        IBuilder<T> Update(T t);
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        IBuilder<T> Update(Expression<Func<T, Object>> expr);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        IBuilder<T> Delete(Expression<Func<T, Boolean>> predicate);
        /// <summary>
        /// 过滤
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        IBuilder<T> Where(Expression<Func<T, Boolean>> predicate);
        /// <summary>
        /// 升序
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        IBuilder<T> OrderBy<K>(Expression<Func<T, K>> keySelector);
        /// <summary>
        /// 降序
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        IBuilder<T> OrderByDescending<K>(Expression<Func<T, K>> keySelector);
        /// <summary>
        /// 选择列
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        IBuilder<T> Select(Expression<Func<T, Object>> selector);
        /// <summary>
        /// 生成分页
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        IBuilder<T> Pager(Int32 pageSize, Int32 pageIndex);
        IBuilder<T> Distinct<K>(Expression<Func<T, K>> expr);
        IBuilder<T> Max<K>(Expression<Func<T, K>> expr);
        IBuilder<T> Min<K>(Expression<Func<T, K>> expr);
        IBuilder<T> Sum<K>(Expression<Func<T, K>> expr);
        IBuilder<T> Avg<K>(Expression<Func<T, K>> expr);
        IBuilder<T> Top(Int32 n);
    }
}
