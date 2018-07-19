using System;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Collections;
using System.Data.SqlClient;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace ApliuDatabase
{
    public class DatabaseHelper
    {
        /// <summary>
        /// 数据库类型
        /// </summary>
        public DatabaseType databaseType = DatabaseType.SqlServer;
        /// <summary>
        /// 最大连接数
        /// </summary>
        public int MaxPool = 10;
        /// <summary>
        /// 最小连接数
        /// </summary>
        public int MinPool = 3;
        /// <summary>
        /// 异步访问数据库
        /// </summary>
        public bool Asyn_Process = true;
        /// <summary>
        /// 连接等待时间 单位秒
        /// </summary>
        public int Conn_Timeout = 10;
        /// <summary>
        /// 连接的生命周期 单位秒
        /// </summary>
        public int Conn_Lifetime = 120;
        /// <summary>
        /// 连接池对象
        /// </summary>
        private SqlConnection sqlConnection = null;
        /// <summary>
        /// 连接池对象
        /// </summary>
        private MySqlConnection mySqlConnection = null;

        private string _databaseConnection = String.Empty;
        /// <summary>
        /// 数据库链接字符串 System.Data.SqlClient.SqlConnectionStringBuilder
        /// </summary>
        public string databaseConnection
        {
            // Data Source={0};Initial Catalog={1};Integrated Security=False;User ID={2};Password={3};Connect Timeout=15;Encrypt=False;TrustServerCertificate=False
            get
            {
                String dbConnStr = String.Empty;
                switch (databaseType)
                {
                    case DatabaseType.SqlServer:
                        dbConnStr = _databaseConnection + ";"
                                + "Max Pool Size=" + MaxPool + ";"
                                + "Min Pool Size=" + MinPool + ";"
                                + "Connect Timeout=" + Conn_Timeout + ";"
                                + "Connection Lifetime=" + Conn_Lifetime + ";"
                                + "Asynchronous Processing=" + Asyn_Process + ";";
                        break;
                    case DatabaseType.Oracle:
                        break;
                    case DatabaseType.MySql:
                        break;
                    case DatabaseType.Access:
                        break;
                    default:
                        break;
                }
                return dbConnStr;
            }
            set
            {
                if (value.EndsWith(";")) _databaseConnection = value.Substring(0, value.Length);
                else _databaseConnection = value;
            }
        }

        /// <summary>
        /// 初始化数据库链接
        /// </summary>
        [Obsolete]
        public void InitializtionConnection()
        {
            switch (databaseType)
            {
                case DatabaseType.SqlServer:
                    sqlConnection = new SqlConnection(databaseConnection);
                    //sqlConnection.OpenAsync();
                    break;
                case DatabaseType.Oracle:
                    break;
                case DatabaseType.MySql:
                    mySqlConnection = new MySqlConnection(databaseConnection);
                    //mySqlConnection.OpenAsync();
                    break;
                case DatabaseType.Access:
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 查询数据库
        /// </summary>
        /// <param name="Sql"></param>
        /// <returns></returns>
        public DataSet GetData(string Sql)
        {
            return ExecuteGet(CommandType.Text, Sql, new object[0]);
        }

        /// <summary>
        /// 更新数据库
        /// </summary>
        /// <param name="Sql"></param>
        /// <returns></returns>
        public int PostData(string Sql)
        {
            return ExecutePost(CommandType.Text, Sql, new object[0]);
        }

        /// <summary>
        /// 查询数据库
        /// </summary>
        /// <param name="Sql"></param>
        /// <returns></returns>
        public DataSet GetData(string Sql, params object[] Args)
        {
            return ExecuteGet(CommandType.Text, Sql, Args);
        }

        /// <summary>
        /// 更新数据库
        /// </summary>
        /// <param name="Sql"></param>
        /// <returns></returns>
        public int PostData(string Sql, params object[] Args)
        {
            return ExecutePost(CommandType.Text, Sql, Args);
        }

        /// <summary>
        /// PostData
        /// </summary>
        /// <returns>返回受影响的行数</returns>
        public int ExecutePost(CommandType cmdType, string cmdText, params object[] commandParameters)
        {
            int val = -1;
            switch (databaseType)
            {
                case DatabaseType.SqlServer:
                    using (SqlCommand cmdsqlmain = new SqlCommand())
                    {
                        PrepareCommand(cmdsqlmain, sqlConnection, null, cmdType, cmdText, commandParameters as SqlParameter[]);
                        val = cmdsqlmain.ExecuteNonQuery();
                        cmdsqlmain.Parameters.Clear();
                    }
                    break;
                case DatabaseType.Oracle:
                    break;
                case DatabaseType.MySql:
                    using (MySqlCommand cmdmysql = new MySqlCommand())
                    {
                        PrepareCommand(cmdmysql, mySqlConnection, null, cmdType, cmdText, commandParameters as MySqlParameter[]);
                        val = cmdmysql.ExecuteNonQuery();
                        cmdmysql.Parameters.Clear();
                    }
                    break;
                case DatabaseType.Access:
                    break;
                default:
                    break;
            }
            return val;
        }

        /// <summary>
        /// GetData
        /// </summary>
        /// <returns>返回查询结果</returns>
        public DataSet ExecuteGet(CommandType commandType, string commandText, params object[] commandParameters)
        {
            DataSet dsData = null;
            switch (databaseType)
            {
                case DatabaseType.SqlServer:
                    using (sqlConnection = new SqlConnection(databaseConnection))
                    {
                        if (sqlConnection.State != ConnectionState.Open) sqlConnection.Open();
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            dsData = new DataSet();
                            PrepareCommand(cmd, sqlConnection, (SqlTransaction)null, commandType, commandText, commandParameters as SqlParameter[]);
                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            da.Fill(dsData);
                        }
                        sqlConnection.Close();
                    }
                    break;
                case DatabaseType.Oracle:
                    break;
                case DatabaseType.MySql:
                    using (mySqlConnection = new MySqlConnection(databaseConnection))
                    {
                        if (mySqlConnection.State != ConnectionState.Open) mySqlConnection.Open();
                        using (MySqlCommand cmd = new MySqlCommand())
                        {
                            dsData = new DataSet();
                            PrepareCommand(cmd, mySqlConnection, (MySqlTransaction)null, commandType, commandText, commandParameters as MySqlParameter[]);
                            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                            da.Fill(dsData);
                        }
                        mySqlConnection.Close();
                    }
                    break;
                case DatabaseType.Access:
                    break;
                default:
                    break;
            }
            return dsData;
        }

        /// <summary>
        /// SqlServer 设置SqlCommand
        /// </summary>
        private void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] commandParameters)
        {
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            cmd.CommandType = cmdType;

            if (trans != null)
                cmd.Transaction = trans;

            if (commandParameters != null)
            {
                foreach (SqlParameter parm in commandParameters)
                    cmd.Parameters.Add(parm);
            }
        }

        /// <summary>
        /// MySql 设置MySqlCommand
        /// </summary>
        private void PrepareCommand(MySqlCommand cmd, MySqlConnection conn, MySqlTransaction trans, CommandType cmdType, string cmdText, MySqlParameter[] commandParameters)
        {
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            cmd.CommandType = cmdType;

            if (trans != null)
                cmd.Transaction = trans;

            if (commandParameters != null)
            {
                foreach (MySqlParameter parm in commandParameters)
                    cmd.Parameters.Add(parm);
            }
        }

        /// <summary>
        /// SqlServer 初始化参数
        /// </summary>
        /// <param name="ParamName"></param>
        /// <param name="DbType"></param>
        /// <param name="Size"></param>
        /// <param name="Direction">默认 ParameterDirection.Input</param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public SqlParameter MakeParamSqlServer(string ParamName, SqlDbType DbType, Int32 Size, ParameterDirection Direction, object Value)
        {
            SqlParameter param;

            if (Size > 0)
                param = new SqlParameter(ParamName, DbType, Size);
            else
                param = new SqlParameter(ParamName, DbType);

            param.Direction = Direction;
            if (!(Direction == ParameterDirection.Output && Value == null))
                param.Value = Value;

            return param;
        }

        /// <summary>
        /// Msql 初始化参数
        /// </summary>
        /// <param name="ParamName"></param>
        /// <param name="DbType"></param>
        /// <param name="Size"></param>
        /// <param name="Direction">默认 ParameterDirection.Input</param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public MySqlParameter MakeParamMySql(string ParamName, MySqlDbType DbType, Int32 Size, ParameterDirection Direction, object Value)
        {
            MySqlParameter param;

            if (Size > 0)
                param = new MySqlParameter(ParamName, DbType, Size);
            else
                param = new MySqlParameter(ParamName, DbType);

            param.Direction = Direction;
            if (!(Direction == ParameterDirection.Output && Value == null))
                param.Value = Value;

            return param;
        }
    }
}
