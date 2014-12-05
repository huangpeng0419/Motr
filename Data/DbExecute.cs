using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;

namespace Motr.Data
{
    /// <summary>
    /// Db Execution
    /// </summary>
    public partial class Db
    {
        #region ExecuteDataSet
        /// <summary>
        /// 填充数据集
        /// </summary>
        /// <param name="tran">事务对象</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cmdText">命令文本</param>
        /// <param name="paramArray">命令参数</param>
        /// <returns>数据集</returns>
        public DataSet ExecuteDataSet(DbTransaction tran, CommandType cmdType, String cmdText, params IDbDataParameter[] paramArray)
        {
            using (DbCommand cmd = DbFactory.CreateCommand())
            {
                try
                {
                    PerpareCommand(tran, cmd, cmdType, cmdText, false, paramArray);
                    using (DbDataAdapter adapter = DbFactory.CreateDataAdapter())
                    {
                        adapter.SelectCommand = cmd;
                        DataSet tmp = new DataSet();
                        adapter.Fill(tmp);
                        return tmp;
                    }
                }
                catch (Exception ex)
                {
                    cmd.Connection.Close();
                    throw ex;
                }
                finally
                {
                    if (tran == null) cmd.Connection.Close();
                    cmd.Parameters.Clear();
                }
            }
        }
        /// <summary>
        /// 填充数据集
        /// </summary>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cmdText">命令文本</param>
        /// <param name="paramArray">命令参数</param>
        /// <returns>数据集</returns>
        public DataSet ExecuteDataSet(CommandType cmdType, String cmdText, params IDbDataParameter[] paramArray)
        {
            return ExecuteDataSet(null, cmdType, cmdText, paramArray);
        }
        /// <summary>
        /// 填充数据集
        /// </summary>
        /// <param name="cmdText">命令文本</param>
        /// <param name="paramArray">命令参数</param>
        /// <returns>数据集</returns>
        public DataSet ExecuteDataSet(String cmdText, params IDbDataParameter[] paramArray)
        {
            return ExecuteDataSet(null, CommandType.Text, cmdText, paramArray);
        }
        /// <summary>
        /// 填充数据集
        /// </summary>
        /// <param name="cmdText">命令文本</param>
        /// <returns>数据集</returns>
        public DataSet ExecuteDataSet(String cmdText)
        {
            return ExecuteDataSet(null, CommandType.Text, cmdText);
        }
        #endregion
        #region ExecuteNonQuery
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="tran">事务对象</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cmdText">命令文本</param>
        /// <param name="paramArray">命令参数</param>
        /// <returns>受影响的行数</returns>
        public Int32 ExecuteNonQuery(DbTransaction tran, CommandType cmdType, String cmdText, params IDbDataParameter[] paramArray)
        {
            using (DbCommand cmd = DbFactory.CreateCommand())
            {
                try
                {
                    PerpareCommand(tran, cmd, cmdType, cmdText, paramArray);
                    return cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    cmd.Connection.Close();
                    throw ex;
                }
                finally
                {
                    if (tran == null) cmd.Connection.Close();
                    cmd.Parameters.Clear();
                }
            }
        }
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cmdText">命令文本</param>
        /// <param name="paramArray">命令参数</param>
        /// <returns>受影响的行数</returns>
        public Int32 ExecuteNonQuery(CommandType cmdType, String cmdText, params IDbDataParameter[] paramArray)
        {
            return ExecuteNonQuery(null, cmdType, cmdText, paramArray);
        }
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="cmdText">命令文本</param>
        /// <param name="paramArray">命令参数</param>
        /// <returns>受影响的行数</returns>
        public Int32 ExecuteNonQuery(String cmdText, params IDbDataParameter[] paramArray)
        {
            return ExecuteNonQuery(null, CommandType.Text, cmdText, paramArray);
        }
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="cmdText">命令文本</param>
        /// <returns>受影响的行数</returns>
        public Int32 ExecuteNonQuery(String cmdText)
        {
            return ExecuteNonQuery(null, CommandType.Text, cmdText);
        }
        #endregion
        #region ExecuteScalar
        /// <summary>
        /// 执行查询，并返回查询所返回的结果集中第一行的第一列。所有其他的列和行将被忽略。
        /// </summary>
        /// <param name="tran">事务对象</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cmdText">命令文本</param>
        /// <param name="paramArray">命令参数</param>
        /// <returns>结果集中第一行的第一列。</returns>
        public Object ExecuteScalar(DbTransaction tran, CommandType cmdType, String cmdText, params IDbDataParameter[] paramArray)
        {
            DbCommand cmd = null;
            try
            {
                cmd = DbFactory.CreateCommand();
                PerpareCommand(tran, cmd, cmdType, cmdText, paramArray);
                return cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                cmd.Connection.Close();
                throw ex;
            }
            finally
            {
              if(tran==null)  cmd.Connection.Close();
                cmd.Parameters.Clear();
            }
        }
        /// <summary>
        /// 执行查询，并返回查询所返回的结果集中第一行的第一列。所有其他的列和行将被忽略。
        /// </summary>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cmdText">命令文本</param>
        /// <param name="paramArray">命令参数</param>
        /// <returns>结果集中第一行的第一列。</returns>
        public Object ExecuteScalar(CommandType cmdType, String cmdText, params IDbDataParameter[] paramArray)
        {
            return ExecuteScalar(null, cmdType, cmdText, paramArray);
        }
        /// <summary>
        /// 执行查询，并返回查询所返回的结果集中第一行的第一列。所有其他的列和行将被忽略。
        /// </summary>
        /// <param name="cmdText">命令文本</param>
        /// <param name="paramArray">命令参数</param>
        /// <returns>结果集中第一行的第一列。</returns>
        public Object ExecuteScalar(String cmdText, params IDbDataParameter[] paramArray)
        {
            return ExecuteScalar(null, CommandType.Text, cmdText, paramArray);
        }
        /// <summary>
        /// 执行查询，并返回查询所返回的结果集中第一行的第一列。所有其他的列和行将被忽略。
        /// </summary>
        /// <param name="cmdText">命令文本</param>
        /// <returns>结果集中第一行的第一列。</returns>
        public Object ExecuteScalar(String cmdText)
        {
            return ExecuteScalar(null, CommandType.Text, cmdText);
        }
        #endregion
        #region ExecuteReader
        /// <summary>
        /// 执行SQL命令 返回一个DbDataReader对象,需调用DbDataReader的Close方法用于关闭DbConnection对象
        /// </summary>
        /// <param name="tran">事务对象</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cmdText">命令文本</param>
        /// <param name="paramArray">命令参数</param>
        /// <returns>一个 System.Data.Common.DbDataReader 对象</returns>
        public DbDataReader ExecuteReader(DbTransaction tran, CommandType cmdType, String cmdText, params IDbDataParameter[] paramArray)
        {

            using (DbCommand cmd = DbFactory.CreateCommand())
            {
                try
                {
                    PerpareCommand(tran, cmd, cmdType, cmdText, paramArray);
                    return cmd.ExecuteReader(CommandBehavior.CloseConnection);
                }
                catch (Exception ex)
                {
                    cmd.Connection.Close();
                    throw ex;
                }
                finally
                {
                    cmd.Parameters.Clear();
                }
            }

        }
        /// <summary>
        /// 执行SQL命令 返回一个DbDataReader对象,需调用DbDataReader的Close方法用于关闭DbConnection对象
        /// </summary>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cmdText">命令文本</param>
        /// <param name="paramArray">命令参数</param>
        /// <returns>一个 System.Data.Common.DbDataReader 对象</returns>
        public DbDataReader ExecuteReader(CommandType cmdType, String cmdText, params IDbDataParameter[] paramArray)
        {
            return ExecuteReader(null, cmdType, cmdText, paramArray);
        }
        /// <summary>
        /// 执行SQL命令 返回一个DbDataReader对象,需调用DbDataReader的Close方法用于关闭DbConnection对象
        /// </summary>
        /// <param name="cmdText">命令文本</param>
        /// <param name="paramArray">命令参数</param>
        /// <returns>一个 System.Data.Common.DbDataReader 对象</returns>
        public DbDataReader ExecuteReader(String cmdText, params IDbDataParameter[] paramArray)
        {
            return ExecuteReader(null, CommandType.Text, cmdText, paramArray);
        }
        /// <summary>
        /// 执行SQL命令 返回一个DbDataReader对象,需调用DbDataReader的Close方法用于关闭DbConnection对象
        /// </summary>
        /// <param name="cmdText">命令文本</param>
        /// <returns>一个 System.Data.Common.DbDataReader 对象</returns>
        public DbDataReader ExecuteReader(String cmdText)
        {
            return ExecuteReader(null, CommandType.Text, cmdText);
        }
        #endregion
        #region PerpareCommand
        /// <summary>
        /// 设置命令对象（根据openConn参数 打开连接）
        /// </summary>
        /// <param name="tran">事务对象</param>
        /// <param name="cmd">命令对象</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cmdText">命令文本</param>
        /// <param name="openConn">是否打开连接</param>
        /// <param name="paramArray">参数数组</param>
        private void PerpareCommand(DbTransaction tran, DbCommand cmd, CommandType cmdType, String cmdText, Boolean openConn, params IDbDataParameter[] paramArray)
        {
            cmd.Transaction = tran;
            if (tran == null)
                cmd.Connection = CreateConnection();
            else
                cmd.Connection = tran.Connection;
            if (openConn && cmd.Connection.State == ConnectionState.Closed)
                cmd.Connection.Open();
            cmd.CommandType = cmdType;
            cmd.CommandText = cmdText;
            foreach (var p in paramArray)
                cmd.Parameters.Add(p);
        }
        /// <summary>
        /// 设置命令对象（默认打开连接）
        /// </summary>
        /// <param name="tran">事务对象</param>
        /// <param name="cmd">命令对象</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cmdText">命令文本</param>
        /// <param name="paramArray">参数数组</param>
        private void PerpareCommand(DbTransaction tran, DbCommand cmd, CommandType cmdType, String cmdText, params IDbDataParameter[] paramArray)
        {
            PerpareCommand(tran, cmd, cmdType, cmdText, true, paramArray);
        }
        #endregion
        #region CreateParameter
        /// <summary>
        /// 创建参数
        /// </summary>
        /// <param name="parameterName">参数名</param>
        /// <param name="dbType">数据类型</param>
        /// <param name="size">大小</param>
        /// <param name="direction">参数类型</param>
        /// <param name="value">值</param>
        /// <returns>一个 System.Data.Common.DbParameter 对象</returns>
        public DbParameter CreateParameter(String parameterName, DbType dbType, Int32 size, ParameterDirection direction, Object value)
        {
            DbParameter param = DbFactory.CreateParameter();
            param.ParameterName = String.Format("{0}{1}", parameterName.StartsWith(ParamPrefix) ? String.Empty : ParamPrefix, parameterName);
            param.DbType = dbType;
            param.Size = size;
            param.Direction = direction;
            if (direction != ParameterDirection.Output || direction != ParameterDirection.ReturnValue)
                param.Value = value;
            return param;
        }
        /// <summary>
        /// 创建参数
        /// </summary>
        /// <param name="parameterName">参数名</param>
        /// <param name="dbType">数据类型</param>
        /// <param name="size">大小</param>
        /// <param name="value">值</param>
        /// <returns>一个 System.Data.Common.DbParameter 对象</returns>
        public DbParameter CreateParameter(String parameterName, DbType dbType, Int32 size, Object value)
        {
            return CreateParameter(parameterName, dbType, size, ParameterDirection.Input, value);
        }
        /// <summary>
        /// 创建参数
        /// </summary>
        /// <param name="parameterName">参数名</param>
        /// <param name="value">值</param>
        /// <returns>一个 System.Data.Common.DbParameter 对象</returns>
        public DbParameter CreateParameter(String parameterName, Object value)
        {
            DbParameter param = DbFactory.CreateParameter();
            param.ParameterName = String.Format("{0}{1}", parameterName.StartsWith(ParamPrefix) ? String.Empty : ParamPrefix, parameterName);
            param.Value = value;
            return param;
        }
        #endregion
    }
}
