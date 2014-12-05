using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Motr.Data.Builder
{
    public class SQLiteBuilder<T> : SqlBuilder<T>, IBuilder<T>
    {
        public SQLiteBuilder(Db context) : base(context) { }
        public new String Sql
        {
            get
            {
                String sql = base.Sql;
                if (base.Action == DbAction.Default && String.IsNullOrEmpty(base._sqlTop) == false)
                    return String.Format("{0}{1}", sql.Replace(base._sqlTop, ""), base._sqlTop);
                else if (base.Action == DbAction.Insert)
                    return sql.Replace("SELECT SCOPE_IDENTITY();", "");
                return sql;
            }
        }
        public new IBuilder<T> Top(Int32 top)
        {
            base._sqlTop = String.Format(" LIMIT 0,{0}", top);
            return this;
        }
       
    }
}
