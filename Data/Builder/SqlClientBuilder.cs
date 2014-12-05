using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Motr.Data.Builder
{
    public class SqlClientBuilder<T> : SqlBuilder<T>, IBuilder<T>
    {
        public SqlClientBuilder(Db context) : base(context) { }
        /// <summary>
        /// 忽略模糊查询通配符
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override String EscapeLikeParamValue(String value)
        {
            return value.Replace("[", "[[]").Replace("?", "[?]").Replace("_", "[_]").Replace("%", "[%]");
        }
        /// <summary>
        /// 生成分页
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public new IBuilder<T> Pager(Int32 pageSize, Int32 pageIndex)
        {
            if (this._sqlOrder.Length == 0) throw new Exception("Paged query must set orderBy Clause");
            this.Action = DbAction.Pager;
            base._sqlPager = String.Format("SELECT * FROM (SELECT {0},ROW_NUMBER() OVER({1}) AS ROW FROM {2}{3}) Temp WHERE ROW BETWEEN @Start AND @End", base._sqlField.ToString().TrimStart(','), base._sqlOrder.ToString(), base._dbTable.Name,base._sqlWhere.ToString());
            this.Parameters.Add(_context.CreateParameter("Start", System.Data.DbType.Int32, 4, (pageIndex * pageSize) - pageSize + 1));
            this.Parameters.Add(_context.CreateParameter("End", System.Data.DbType.Int32, 4, (pageIndex * pageSize)));
            return this;
        }

    }
}
