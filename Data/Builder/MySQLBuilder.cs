using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Motr.Data.Builder
{
    public class MySQLBuilder<T>:SqlBuilder<T>,IBuilder<T>
    {
        public MySQLBuilder(Db context):base(context){}
    }
}
