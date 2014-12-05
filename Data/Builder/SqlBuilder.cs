using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Linq.Expressions;
using System.Reflection;
using Motr.Data.Mapping;

namespace Motr.Data.Builder
{
    /// <summary>
    /// sql 生成
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SqlBuilder<T> : IBuilder<T>
    {
        protected Db _context;                      // 上下文
        protected Type _generic;                    // 类型
        protected DbTable _dbTable;                 // attribute dbtable
        protected String _sqlTop;                   // top 分句
        protected String _sqlPager;                 // 分页sql
        protected StringBuilder _sqlField;          // 参与字段
        protected StringBuilder _sqlWhere;          // 条件sql
        protected StringBuilder _sqlOrder;          // 排序sql
        protected List<DbParameter> _parameters;    // 参数
        protected Int32 _i;                         // 为使参数名称不重名的增长数字
        public SqlBuilder(Db provider)
        {
            this._context = provider;
            this._generic = typeof(T);
            this._dbTable = _GetDbTable();
            this._sqlField = new StringBuilder();
            this._sqlWhere = new StringBuilder();
            this._sqlOrder = new StringBuilder();
            this._parameters = new List<DbParameter>();
            this.Action = DbAction.Default;
        }
        /// <summary>
        /// 生成动作
        /// </summary>
        public DbAction Action { get; set; }
        /// <summary>
        /// 输出组装Sql
        /// </summary>
        public String Sql
        {
            get
            {
                if (this._sqlField.Length == 0) this._sqlField.Append("*");
                else if(this._sqlField.ToString()[0]==',') this._sqlField.Remove(0, 1);
                switch (this.Action)
                {
                    case DbAction.Insert: return String.Format("INSERT INTO [{0}]({1}) VALUES({2});SELECT SCOPE_IDENTITY();", this._dbTable.Name, this._sqlField.ToString(), this._sqlField.Replace("[", this._context.ParamPrefix).Replace("]", ""));
                    case DbAction.Update: return String.Format("UPDATE [{0}] SET {1}{2}", this._dbTable.Name, this._sqlField.ToString(), this._sqlWhere.ToString());
                    case DbAction.Delete: return String.Format("DELETE FROM [{0}] {1}", this._dbTable.Name, this._sqlWhere.ToString());
                    case DbAction.Count: return String.Format("SELECT COUNT(0) FROM {0}{1}{2}", this._dbTable.Name, this._sqlWhere.ToString(), this._sqlOrder.ToString());
                    case DbAction.Pager: return this._sqlPager;
                    default: return String.Format("SELECT {0}{1} FROM [{2}]{3}{4}", this._sqlTop, this._sqlField.ToString(), this._dbTable.Name, this._sqlWhere.ToString(), this._sqlOrder.ToString());
                }
            }
        }
        /// <summary>
        /// 参数
        /// </summary>
        public List<DbParameter> Parameters
        {
            get { return this._parameters; }
            set { this._parameters = value; }
        }
        /// <summary>
        /// 生成插入
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public IBuilder<T> Insert(T t)
        {
            this.Action = DbAction.Insert;
            PropertyOpt(new PropertyOptHandler(delegate(PropertyInfo pi)
            {
                this._sqlField.AppendFormat(",[{0}]", pi.Name);
                this._parameters.Add(_context.CreateParameter(pi.Name, pi.GetValue(t, null).DbValueConverter(pi.PropertyType)));
            }));
            return this;
        }
        /// <summary>
        /// 生成更新
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public IBuilder<T> Update(T t)
        {
            this.Action = DbAction.Update;
            PropertyOpt(new PropertyOptHandler(delegate(PropertyInfo pi)
            {
                this._sqlField.AppendFormat(",[{0}]={1}{0}", pi.Name, _context.ParamPrefix);
                this._parameters.Add(_context.CreateParameter(pi.Name, pi.GetValue(t, null).DbValueConverter(pi.PropertyType)));
            }));
            return this;
        }

        /// <summary>
        /// 生成更新
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public IBuilder<T> Update(Expression<Func<T, Object>> expr)
        {
            this.Action = DbAction.Update;
            var miExpr = expr.Body as MemberInitExpression;
            if (miExpr == null) throw new ArgumentException("use o=>new T(){ }");
            foreach (var item in miExpr.Bindings)
            {
                this._sqlField.AppendFormat(",[{0}]={1}", item.Member.Name, GetRigth((item as MemberAssignment).Expression));
            }
            return this;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public IBuilder<T> Delete(Expression<Func<T, Boolean>> predicate)
        {
            this.Action = DbAction.Delete;
            this.Where(predicate);
            return this;
        }
        /// <summary>
        /// 升序
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public IBuilder<T> OrderBy<K>(Expression<Func<T, K>> keySelector)
        {
            if (this._sqlOrder.Length == 0) this._sqlOrder.AppendFormat(" ORDER BY {0}", GetMemberName(keySelector, "[{0}] ASC"));
            else this._sqlOrder.AppendFormat(GetMemberName(keySelector, ",[{0}] ASC"));
            return this;
        }
        /// <summary>
        /// 降序
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public IBuilder<T> OrderByDescending<K>(Expression<Func<T, K>> keySelector)
        {
            if (this._sqlOrder.Length == 0) _sqlOrder.AppendFormat(" ORDER BY {0}", GetMemberName(keySelector, "[{0}] DESC"));
            else this._sqlOrder.AppendFormat(GetMemberName(keySelector, ",[{0}] DESC"));
            return this;
        }
        /// <summary>
        /// 选择字段
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public IBuilder<T> Select(Expression<Func<T, Object>> selector)
        {
            var newExpr = (selector.Body as NewExpression);
            if (newExpr == null) throw new ArgumentException("use o=>new{o.x,o.xx,o.xxx}");
            foreach (var item in newExpr.Members)
                this._sqlField.AppendFormat(",[{0}]", item.Name.Remove(0, 4));
            return this;
        }
        /// <summary>
        /// 剔除重复
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <param name="expr"></param>
        /// <returns></returns>
        public IBuilder<T> Distinct<K>(Expression<Func<T, K>> expr)
        {
            this._sqlField.Append(GetMemberName(expr, ",DISTINCT([{0}])"));
            return this;
        }
        /// <summary>
        /// 最大
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <param name="expr"></param>
        /// <returns></returns>
        public IBuilder<T> Max<K>(Expression<Func<T, K>> expr)
        {
            this._sqlField.Append(GetMemberName(expr, ",MAX([{0}])"));
            return this;
        }
        /// <summary>
        /// 最小
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <param name="expr"></param>
        /// <returns></returns>
        public IBuilder<T> Min<K>(Expression<Func<T, K>> expr)
        {
            this._sqlField.Append(GetMemberName(expr, ",MIN([{0}])"));
            return this;
        }
        /// <summary>
        /// 求和
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <param name="expr"></param>
        /// <returns></returns>
        public IBuilder<T> Sum<K>(Expression<Func<T, K>> expr)
        {
            this._sqlField.Append(GetMemberName(expr, ",SUM([{0}])"));
            return this;
        }
        /// <summary>
        /// 求平均
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <param name="expr"></param>
        /// <returns></returns>
        public IBuilder<T> Avg<K>(Expression<Func<T, K>> expr)
        {
            this._sqlField.Append(GetMemberName(expr, ",AVG([{0}])"));
            return this;
        }
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public IBuilder<T> Pager(Int32 pageSize, Int32 pageIndex)
        {
            return this;
        }
        public IBuilder<T> Top(Int32 top)
        {
            this._sqlTop = String.Format("TOP {0} ", top);
            return this;
        }
        /// <summary>
        /// 生成查询条件Sql
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public IBuilder<T> Where(Expression<Func<T, Boolean>> predicate)
        {
            if (_sqlWhere.Length == 0)
                _sqlWhere.AppendFormat(" WHERE {0}", BuildWhere(predicate.Body));
            else
                _sqlWhere.AppendFormat(" AND {0}", BuildWhere(predicate.Body));
            return this;
        }
        /// <summary>
        /// 忽略模糊查询通配符
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual String EscapeLikeParamValue(String value)
        {
            return value;
        }
        /// <summary>
        /// 忽略字符串单引号
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual String EscapeValue(String value)
        {
            return value ?? value.Replace("'", "''");
        }
        #region private method
        /// <summary>
        /// 获取DbTable
        /// </summary>
        /// <returns></returns>
        private DbTable _GetDbTable()
        {
            DbTable dbTable = _generic.GetAttr<DbTable>();
            if (dbTable == null) return new DbTable(_generic.Name);
            return dbTable;
        }
        /// <summary>
        /// 属性操作
        /// </summary>
        /// <param name="method"></param>
        private void PropertyOpt(PropertyOptHandler method)
        {
            DbColumn columnAttr = null;
            foreach (PropertyInfo pi in _generic.GetProperties())
            {
                if (!pi.CanWrite) continue;
                columnAttr = pi.GetAttr<DbColumn>();
                if (columnAttr != null && (columnAttr.IsIdentity || columnAttr.IsExtensionProperty)) continue;
                method(pi);
            }
        }
        /// <summary>
        /// 获取字段或属性的名称
        /// </summary>
        /// <param name="expr"></param>
        /// <param name="notFormatString"></param>
        /// <returns></returns>
        private String GetMemberName<K>(Expression<Func<T, K>> expr, String notFormatString)
        {
            var memberExpr = (expr.Body as MemberExpression);
            if (memberExpr == null) throw new ArgumentException("use o=>o.x");
            return String.Format(notFormatString, memberExpr.Member.Name);
        }
        /// <summary>
        /// 更新部分字段获取等号右边表达式
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        private String GetRigth(Expression expr)
        {
            String rightExpr = String.Empty;
            if (expr is ConstantExpression)
            {
                var constExpr = expr as ConstantExpression;
                rightExpr = String.Format("{0}Field{1}", _context.ParamPrefix, _i);
                this.Parameters.Add(_context.CreateParameter(rightExpr, constExpr.Value));
                _i++;
            }
            else if (expr is MemberExpression)
            {
                var memberExpr = (expr as MemberExpression);
                if (memberExpr.Expression != null)
                {
                    switch (memberExpr.Expression.NodeType)
                    {
                        case ExpressionType.Parameter: rightExpr = String.Format("[{0}]", memberExpr.Member.Name); break;
                        case ExpressionType.Constant:
                        case ExpressionType.MemberAccess:
                            rightExpr = String.Format("{0}Field{1}", _context.ParamPrefix, _i);
                            this.Parameters.Add(_context.CreateParameter(rightExpr, memberExpr.Compile()));
                            _i++;
                            break;
                    }
                }
                else
                {
                    switch (expr.NodeType)
                    {
                        //case ExpressionType.Parameter: rightExpr = String.Format("[{0}]", expr.Member.Name); break;
                        case ExpressionType.Constant:
                        case ExpressionType.MemberAccess:
                            rightExpr = String.Format("{0}Field{1}", _context.ParamPrefix, _i);
                            this.Parameters.Add(_context.CreateParameter(rightExpr, expr.Compile()));
                            _i++;
                            break;
                    }
                }
            }
            else if (expr is BinaryExpression)
            {
                var binaryExpr = (expr as BinaryExpression);
                rightExpr = String.Format("{0}{1}{2}", GetRigth(binaryExpr.Left), GetSymbol(binaryExpr.NodeType), GetRigth(binaryExpr.Right));
            }
            else if (expr is UnaryExpression)
            {
                rightExpr = String.Format("{0}Field{1}", _context.ParamPrefix, _i);
                this.Parameters.Add(_context.CreateParameter(rightExpr, (expr as UnaryExpression).Operand.Compile()));
                _i++;
            }
            else if (expr is MethodCallExpression)
            {
                var methodCallExpr = (expr as MethodCallExpression);
                rightExpr = String.Format("{0}Field{1}", _context.ParamPrefix, _i);
                this.Parameters.Add(_context.CreateParameter(rightExpr,Expression.Call(methodCallExpr.Object,methodCallExpr.Method,methodCallExpr.Arguments).Compile()));
                _i++;
            }
            return rightExpr;
        }
        private String GetSymbol(ExpressionType exprType)
        {
            switch (exprType)
            {
                case ExpressionType.Add: return "+";
                case ExpressionType.Divide: return "/";
                case ExpressionType.Equal: return "=";
                case ExpressionType.GreaterThan: return ">";
                case ExpressionType.GreaterThanOrEqual: return ">=";
                case ExpressionType.LessThan: return "<";
                case ExpressionType.LessThanOrEqual: return "<=";
                case ExpressionType.Multiply: return "*";
                case ExpressionType.NotEqual: return "<>";
                case ExpressionType.Subtract: return "-";
            }
            return String.Empty;
        }
        /// <summary>
        /// 生成查询条件Sql
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        private String BuildWhere(Expression expr)
        {
            var clauseBuilder = new StringBuilder();
            if (expr is BinaryExpression)
            {
                var binaryExpr = expr as BinaryExpression;
                switch (binaryExpr.NodeType)
                {
                    case ExpressionType.AndAlso: clauseBuilder.AppendFormat("{0} AND {1}", BuildWhere(binaryExpr.Left), BuildWhere(binaryExpr.Right)); break;
                    case ExpressionType.OrElse: clauseBuilder.AppendFormat("({0} OR {1})", BuildWhere(binaryExpr.Left), BuildWhere(binaryExpr.Right)); break;
                }
                var symbol = GetSymbol(binaryExpr.NodeType);
                if (symbol != String.Empty)
                {
                    if (binaryExpr.Left is MemberExpression)
                        clauseBuilder.AppendFormat("[{0}] {1} {2}", (binaryExpr.Left as MemberExpression).Member.Name, symbol, GetRigth(binaryExpr.Right));
                    else if (binaryExpr.Left is UnaryExpression)
                        clauseBuilder.AppendFormat("[{0}] {1} {2}", ((binaryExpr.Left as UnaryExpression).Operand as MemberExpression).Member.Name, symbol, GetRigth(binaryExpr.Right));
                    else if (binaryExpr.Left is MethodCallExpression)
                    {
                        var mcExpr = (binaryExpr.Left as MethodCallExpression);
                       if( mcExpr.Method.Name=="ToString")
                           clauseBuilder.AppendFormat("[{0}] {1} {2}", (mcExpr.Object as MemberExpression).Member.Name, symbol, GetRigth(binaryExpr.Right));
                    }
                }
            }
            else if (expr is MethodCallExpression)
            {
                MethodCallResolve(expr, expr.NodeType, clauseBuilder);
            }
            else if (expr is UnaryExpression)
            {
                var unaryExpression = expr as UnaryExpression;
                if (unaryExpression.Operand is MethodCallExpression)
                    MethodCallResolve(unaryExpression.Operand, unaryExpression.NodeType, clauseBuilder);

            }
            return clauseBuilder.ToString();
        }
        /// <summary>
        /// 方法调用分解
        /// </summary>
        /// <param name="expr"></param>
        /// <param name="exprType"></param>
        /// <param name="clauseBuilder"></param>
        private void MethodCallResolve(Expression expr, ExpressionType exprType, StringBuilder clauseBuilder)
        {
            var not = exprType == ExpressionType.Not ? " NOT " : " ";
            var methodCallExpr = (expr as MethodCallExpression);
            var methodName = methodCallExpr.Method.Name;
            var field = String.Empty;
            switch (methodCallExpr.Method.DeclaringType.Name)
            {
                case "String":
                    field = (methodCallExpr.Object as MemberExpression).Member.Name;
                    if (methodName == "IsNullOrEmpty")
                        clauseBuilder.AppendFormat("ISNULL([{0}],''){1}''", field, exprType == ExpressionType.Not ? "<>" : "=");
                    else
                    {
                        var val = methodCallExpr.Arguments[0].Compile().ToString();
                        switch (methodName)
                        {
                            case "StartsWith": val = String.Format("{0}%", EscapeLikeParamValue(val)); break;
                            case "EndsWith": val = String.Format("%{0}", EscapeLikeParamValue(val)); break;
                            case "Contains": val = String.Format("%{0}%", EscapeLikeParamValue(val)); break;
                        }
                        clauseBuilder.AppendFormat("[{0}]{3}LIKE {1}{0}{2}", field, _context.ParamPrefix, _i.ToString(), not);
                        this.Parameters.Add(_context.CreateParameter(field + _i.ToString(), val));
                        _i++;
                    }
                    break;
                case "List`1":
                    if (methodName == "Contains")
                    {
                        field= ((MemberExpression)methodCallExpr.Arguments[0]).Member.Name;
                        var obj = methodCallExpr.Object;
                        if (obj.Type.FullName == typeof(List<Int32>).FullName) clauseBuilder.Append(BuilderIn<Int32>(field, not, (List<Int32>)obj.Compile()));
                        else if (obj.Type.FullName == typeof(List<Int16>).FullName) clauseBuilder.Append(BuilderIn<Int16>(field, not, (List<Int16>)obj.Compile()));
                        else if (obj.Type.FullName == typeof(List<Int64>).FullName) clauseBuilder.Append(BuilderIn<Int64>(field, not, (List<Int64>)obj.Compile()));
                        else if (obj.Type.FullName == typeof(List<Byte>).FullName) clauseBuilder.Append(BuilderIn<Byte>(field, not, (List<Byte>)obj.Compile()));
                        else if (obj.Type.FullName == typeof(List<Double>).FullName) clauseBuilder.Append(BuilderIn<Double>(field, not, (List<Double>)obj.Compile()));
                        else if (obj.Type.FullName == typeof(List<Single>).FullName) clauseBuilder.Append(BuilderIn<Single>(field, not, (List<Single>)obj.Compile()));
                        else if (obj.Type.FullName == typeof(List<Decimal>).FullName) clauseBuilder.Append(BuilderIn<Decimal>(field, not, (List<Decimal>)obj.Compile()));
                        else if (obj.Type.FullName == typeof(List<Boolean>).FullName) clauseBuilder.Append(BuilderIn<Boolean>(field, not, (List<Boolean>)obj.Compile()));
                        else if (obj.Type.FullName == typeof(List<Guid>).FullName) clauseBuilder.Append(BuilderIn<Guid>(field, not, (List<Guid>)obj.Compile()));
                        else if (obj.Type.FullName == typeof(List<DateTime>).FullName) clauseBuilder.Append(BuilderIn<DateTime>(field, not, (List<DateTime>)obj.Compile()));
                        else if (obj.Type.FullName == typeof(List<String>).FullName) clauseBuilder.Append(BuilderIn<String>(field, not, (List<String>)obj.Compile()));
                    }
                    break;
            }
        }
        /// <summary>
        /// 生成IN
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <param name="not">一元运算Not</param>
        /// <param name="field"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        private String BuilderIn<K>(String field, String not, List<K> list)
        {
            if (list.Count == 0) return String.Empty;
            var inBuilder = new StringBuilder();
            inBuilder.AppendFormat("[{0}]{1}IN(", field, not);
            list.ForEach(o =>
            {
                var paramName = String.Format("{0}{1}{2}", _context.ParamPrefix, field, _i);
                inBuilder.Append(paramName).Append(",");
                this.Parameters.Add(_context.CreateParameter(paramName, o));
                _i++;
            });
            inBuilder.Length--;
            inBuilder.Append(")");
            return inBuilder.ToString();
        }
        #endregion
    }
}
