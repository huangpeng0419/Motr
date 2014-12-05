using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data.Common;

namespace Motr.Data
{
    public delegate void PropertyOptHandler(PropertyInfo pi);
    public delegate T TransactionMethod<T>(DbTransaction tran);
    public delegate void TransactionMethod(DbTransaction tran);
}
