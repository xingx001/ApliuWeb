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
        public static string DatabaseConnection = string.Empty;
        private static Hashtable parmCache = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        /// 查询数据库
        /// </summary>
        /// <param name="Sql"></param>
        /// <returns></returns>
        public static DataSet GetData(string Sql)
        {
            return ExecuteGet(DatabaseConnection, CommandType.Text, Sql, new object[0]);
        }

        /// <summary>
        /// 更新数据库
        /// </summary>
        /// <param name="Sql"></param>
        /// <returns></returns>
        public static int PostData(string Sql)
        {
            return ExecutePost(DatabaseConnection, CommandType.Text, Sql, new object[0]);
        }

        /// <summary>
        /// 查询数据库
        /// </summary>
        /// <param name="Sql"></param>
        /// <returns></returns>
        public static DataSet GetData(string Sql, params object[] Args)
        {
            return ExecuteGet(DatabaseConnection, CommandType.Text, Sql, Args);
        }

        /// <summary>
        /// 更新数据库
        /// </summary>
        /// <param name="Sql"></param>
        /// <returns></returns>
        public static int PostData(string Sql, params object[] Args)
        {
            return ExecutePost(DatabaseConnection, CommandType.Text, Sql, Args);
        }

        /// <summary>
        /// PostData
        /// </summary>
        /// <returns>返回受影响的行数</returns>
        public static int ExecutePost(string connectionString, CommandType cmdType, string cmdText, params object[] commandParameters)
        {
            int val = -1;
            switch (DatabaseType.dbType)
            {
                case DatabaseTypeEnum.SqlServer:
                    SqlCommand cmdsqlmain = new SqlCommand();
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        PrepareCommand(cmdsqlmain, connection, null, cmdType, cmdText, commandParameters as SqlParameter[]);
                        val = cmdsqlmain.ExecuteNonQuery();
                        cmdsqlmain.Parameters.Clear();
                    }
                    break;
                case DatabaseTypeEnum.Oracle:
                    break;
                case DatabaseTypeEnum.MySql:
                    MySqlCommand cmdmysql = new MySqlCommand();
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        PrepareCommand(cmdmysql, connection, null, cmdType, cmdText, commandParameters as MySqlParameter[]);
                        val = cmdmysql.ExecuteNonQuery();
                        cmdmysql.Parameters.Clear();
                    }
                    break;
                case DatabaseTypeEnum.Access:
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
        public static DataSet ExecuteGet(string connectionString, CommandType commandType, string commandText, params object[] commandParameters)
        {
            DataSet dsData = null;
            switch (DatabaseType.dbType)
            {
                case DatabaseTypeEnum.SqlServer:
                    using (SqlConnection cn = new SqlConnection(connectionString))
                    {
                        cn.Open();
                        SqlCommand cmd = new SqlCommand();
                        PrepareCommand(cmd, cn, (SqlTransaction)null, commandType, commandText, commandParameters as SqlParameter[]);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dsData);
                    }
                    break;
                case DatabaseTypeEnum.Oracle:
                    break;
                case DatabaseTypeEnum.MySql:
                    using (MySqlConnection cn = new MySqlConnection(connectionString))
                    {
                        cn.Open();
                        MySqlCommand cmd = new MySqlCommand();
                        PrepareCommand(cmd, cn, (MySqlTransaction)null, commandType, commandText, commandParameters as MySqlParameter[]);
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        da.Fill(dsData);
                    }
                    break;
                case DatabaseTypeEnum.Access:
                    break;
                default:
                    break;
            }
            return dsData;
        }

        /// <summary>
        /// SqlServer 设置SqlCommand
        /// </summary>
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] commandParameters)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();

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
        private static void PrepareCommand(MySqlCommand cmd, MySqlConnection conn, MySqlTransaction trans, CommandType cmdType, string cmdText, MySqlParameter[] commandParameters)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();

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
        public static SqlParameter MakeParamSqlServer(string ParamName, SqlDbType DbType, Int32 Size, ParameterDirection Direction, object Value)
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
        public static MySqlParameter MakeParamMySql(string ParamName, MySqlDbType DbType, Int32 Size, ParameterDirection Direction, object Value)
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
