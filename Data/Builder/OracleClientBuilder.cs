using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Motr.Data.Builder
{
    public class OracleClientBuilder<T> : SqlBuilder<T>, IBuilder<T>
    {
        public OracleClientBuilder(Db context) : base(context) { }
        /// <summary>
        /// 忽略模糊查找通配符
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override String EscapeLikeParamValue(String value)
        {
            return value.Replace("?", @"\?").Replace("_", @"\_").Replace("%", @"\%");
        }
        /// <summary>
        /// 生成分页
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public new IBuilder<T> Pager(Int32 pageSize, Int32 pageIndex)
        {

            return this;
        }
    }
}
