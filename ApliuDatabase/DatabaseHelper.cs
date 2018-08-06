using System;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Collections;
using System.Data.SqlClient;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Data.Common;
using System.Transactions;
using System.Data.OracleClient;
using System.Threading.Tasks;
using System.Threading;

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

        private string _databaseConnection = String.Empty;
        /// <summary>
        /// 数据库链接字符串 System.Data.SqlClient.SqlConnectionStringBuilder
        /// </summary>
        public string databaseConnection
        {
            // Data Source={0};Initial Catalog={1};Integrated Security=False;User ID={2};Password={3};Connect Timeout=15;Encrypt=False;TrustServerCertificate=False
            get
            {
                return CreateDatabaseConnectionStr(_databaseConnection);
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
                    SqlConnection sqlConnection = new SqlConnection(databaseConnection);
                    //sqlConnection.OpenAsync();
                    break;
                case DatabaseType.Oracle:
                    break;
                case DatabaseType.MySql:
                    MySqlConnection mySqlConnection = new MySqlConnection(databaseConnection);
                    //mySqlConnection.OpenAsync();
                    break;
                case DatabaseType.OleDb:
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 执行SQL语句查询数据库
        /// </summary>
        /// <param name="Sql">Sql语句</param>
        /// <returns>结果集</returns>
        public DataSet GetData(string Sql)
        {
            return GetDataExecute(CommandType.Text, Sql, 30, null);
        }

        /// <summary>
        /// 执行SQL语句更新数据库
        /// </summary>
        /// <param name="Sql">Sql语句</param>
        /// <returns>受影响的行数</returns>
        public int PostData(string Sql)
        {
            return PostDataExecute(CommandType.Text, Sql, 30, null);
        }

        /// <summary>
        /// 数据库事务范围
        /// </summary>
        private TransactionScope transactionScope = null;

        /// <summary>
        /// 开启数据库事务
        /// </summary>
        /// <param name="seconds">事务超时时间 单位秒</param>
        public void BeginTransaction(int seconds)
        {
            TransactionOptions transactionOption = new TransactionOptions();
            //设置事务隔离级别
            transactionOption.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            // 设置事务超时时间
            transactionOption.Timeout = new TimeSpan(0, 0, seconds);
            transactionScope = new TransactionScope(TransactionScopeOption.Required, transactionOption);

            //当时间超过之后，主动注销该事务
            Task.Factory.StartNew(() => { Thread.Sleep(seconds * 1000); Dispose(); });
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        public void Complete()
        {
            if (transactionScope != null)
            {
                transactionScope.Complete();
                transactionScope.Dispose();
                transactionScope = null;
            }
        }

        /// <summary>
        /// 撤销事务
        /// </summary>
        public void Dispose()
        {
            if (transactionScope != null)
            {
                transactionScope.Dispose();
                transactionScope = null;
            }
        }

        #region 执行 Transact-SQL 语句并返回受影响的行数
        /// <summary>
        /// 执行 Transact-SQL 语句并返回受影响的行数
        /// </summary>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <param name="commandText">Sql语句或存储过程</param>
        /// <param name="commandTimeout">语句执行的超时时间</param>
        /// <param name="commandParameters">语句参数 </param>
        /// <returns>返回受影响的行数</returns>
        public int PostDataExecute(CommandType commandType, string commandText, int commandTimeout, params object[] commandParameters)
        {
            int val = -1;
            try
            {
                //SQL语句以分号结束
                if (!commandText.Trim().EndsWith(";")) commandText = commandText.Trim() + ";";

                switch (databaseType)
                {
                    case DatabaseType.SqlServer:
                        break;
                    case DatabaseType.Oracle://Oracle SQL语句必须加上Begin End
                        if (!commandText.Trim().ToUpper().EndsWith("END;")) commandText = "begin " + commandText + " end;";
                        break;
                    case DatabaseType.MySql:
                        break;
                    case DatabaseType.OleDb:
                        break;
                    default:
                        break;
                }

                using (DbConnection dbConnection = CreateDbConnection(databaseConnection))
                {
                    if (dbConnection.State != ConnectionState.Open) dbConnection.Open();
                    using (DbCommand dbCommand = dbConnection.CreateCommand())
                    {
                        dbCommand.CommandText = commandText;
                        dbCommand.CommandTimeout = commandTimeout;
                        dbCommand.CommandType = commandType;
                        if (commandParameters != null)
                        {
                            foreach (DbParameter parm in commandParameters)
                                dbCommand.Parameters.Add(parm);
                        }
                        val = dbCommand.ExecuteNonQuery();
                        dbConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                val = -1;
                throw ex;
            }

            #region 过期方式
            //switch (databaseType)
            //{
            //    case DatabaseType.SqlServer:
            //        SqlServerExecuteNonQuery(commandType, commandText, commandTimeout, out val, commandParameters);
            //        break;
            //    case DatabaseType.Oracle:
            //        break;
            //    case DatabaseType.MySql:
            //        MySqlExecuteNonQuery(commandType, commandText, commandTimeout, out val, commandParameters);
            //        break;
            //    case DatabaseType.Access:
            //        break;
            //    default:
            //        break;
            //}
            #endregion

            return val;
        }

        #region 过期方式
        /// <summary>
        /// Sql Server具体执行PostData
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="val"></param>
        /// <param name="commandParameters"></param>
        [Obsolete]
        private void SqlServerExecuteNonQuery(CommandType commandType, string commandText, int commandTimeout, out int val, params object[] commandParameters)
        {
            SqlTransaction sqlTransaction = null;
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(databaseConnection))
                {
                    if (sqlConnection.State != ConnectionState.Open) sqlConnection.Open();
                    sqlTransaction = sqlConnection.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);

                    using (SqlCommand sqlCommand = new SqlCommand(commandText, sqlConnection))
                    {
                        sqlCommand.CommandTimeout = commandTimeout;
                        sqlCommand.CommandType = commandType;
                        sqlCommand.Transaction = sqlTransaction;
                        if (commandParameters != null)
                        {
                            //sqlCommand.Parameters.Clear();
                            foreach (SqlParameter parm in commandParameters)
                                sqlCommand.Parameters.Add(parm);
                        }
                        val = sqlCommand.ExecuteNonQuery();
                        if (val >= 0) sqlTransaction.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                val = -1;
                if (sqlTransaction != null) sqlTransaction.Rollback();
                throw ex;
            }
        }

        /// <summary>
        /// MySql具体执行GetData
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="val"></param>
        /// <param name="commandParameters"></param>
        [Obsolete]
        private void MySqlExecuteNonQuery(CommandType commandType, string commandText, int commandTimeout, out int val, params object[] commandParameters)
        {
            MySqlTransaction mySqlTransaction = null;
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(databaseConnection))
                {
                    if (mySqlConnection.State != ConnectionState.Open) mySqlConnection.Open();
                    mySqlTransaction = mySqlConnection.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                    using (MySqlCommand mySqlCommand = new MySqlCommand(commandText, mySqlConnection, mySqlTransaction))
                    {
                        mySqlCommand.CommandTimeout = commandTimeout;
                        mySqlCommand.CommandType = commandType;
                        if (commandParameters != null)
                        {
                            //mySqlCommand.Parameters.Clear();
                            foreach (MySqlParameter parm in commandParameters)
                                mySqlCommand.Parameters.Add(parm);
                        }
                        MySqlDataAdapter da = new MySqlDataAdapter(mySqlCommand);

                        val = mySqlCommand.ExecuteNonQuery();
                        if (val >= 0) mySqlTransaction.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                if (mySqlTransaction != null) mySqlTransaction.Rollback();
                val = -1;
                throw ex;
            }
        }
        #endregion

        #endregion

        #region 执行Transact-SQL语句或存储过程，并返回查询结果
        /// <summary>
        /// 执行Transact-SQL语句或存储过程，并返回查询结果
        /// </summary>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <param name="commandText">Sql语句或存储过程</param>
        /// <param name="commandTimeout">语句执行的超时时间</param>
        /// <param name="commandParameters">语句参数</param>
        /// <returns>返回结果集</returns>
        public DataSet GetDataExecute(CommandType commandType, string commandText, int commandTimeout, params object[] commandParameters)
        {
            DataSet dsData = null;
            try
            {
                using (DbConnection dbConnection = CreateDbConnection(databaseConnection))
                {
                    if (dbConnection.State != ConnectionState.Open) dbConnection.Open();
                    using (DbCommand dbCommand = dbConnection.CreateCommand())
                    {
                        dbCommand.CommandText = commandText;
                        dbCommand.CommandTimeout = commandTimeout;
                        dbCommand.CommandType = commandType;
                        if (commandParameters != null)
                        {
                            //sqlCommand.Parameters.Clear();
                            foreach (DbParameter parm in commandParameters)
                                dbCommand.Parameters.Add(parm);
                        }
                        DbDataAdapter da = CreateDbDataAdapter(dbCommand);
                        dsData = new DataSet();
                        da.Fill(dsData);
                    }
                }
            }
            catch (Exception ex)
            {
                dsData = null;
                throw ex;
            }

            #region 过期方式
            //switch (databaseType)
            //{
            //    case DatabaseType.SqlServer:
            //        SqlServerDataAdapter(commandType, commandText, commandTimeout, out dsData, commandParameters);
            //        break;
            //    case DatabaseType.Oracle:
            //        break;
            //    case DatabaseType.MySql:
            //        MySqlDataAdapter(commandType, commandText, commandTimeout, out dsData, commandParameters);
            //        break;
            //    case DatabaseType.Access:
            //        break;
            //    default:
            //        break;
            //}
            #endregion

            return dsData;
        }

        #region 过期方式
        /// <summary>
        /// Sql Server具体执行GetData
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="dsData"></param>
        /// <param name="commandParameters"></param>
        [Obsolete]
        private void SqlServerDataAdapter(CommandType commandType, string commandText, int commandTimeout, out DataSet dsData, params object[] commandParameters)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(databaseConnection))
                {
                    if (sqlConnection.State != ConnectionState.Open) sqlConnection.Open();
                    SqlTransaction sqlTransaction = sqlConnection.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                    using (SqlCommand sqlCommand = new SqlCommand(commandText, sqlConnection))
                    {
                        sqlCommand.CommandTimeout = commandTimeout;
                        sqlCommand.CommandType = commandType;
                        sqlCommand.Transaction = sqlTransaction;
                        if (commandParameters != null)
                        {
                            //sqlCommand.Parameters.Clear();
                            foreach (SqlParameter parm in commandParameters)
                                sqlCommand.Parameters.Add(parm);
                        }
                        SqlDataAdapter da = new SqlDataAdapter(sqlCommand);

                        dsData = new DataSet();
                        da.Fill(dsData);
                    }
                }
            }
            catch (Exception ex)
            {
                dsData = null;
                throw ex;
            }
        }

        /// <summary>
        /// MySql具体执行GetData
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="dsData"></param>
        /// <param name="commandParameters"></param>
        [Obsolete]
        private void MySqlDataAdapter(CommandType commandType, string commandText, int commandTimeout, out DataSet dsData, params object[] commandParameters)
        {
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(databaseConnection))
                {
                    if (mySqlConnection.State != ConnectionState.Open) mySqlConnection.Open();
                    MySqlTransaction mySqlTransaction = mySqlConnection.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                    using (MySqlCommand mySqlCommand = new MySqlCommand(commandText, mySqlConnection, mySqlTransaction))
                    {
                        mySqlCommand.CommandTimeout = commandTimeout;
                        mySqlCommand.CommandType = commandType;
                        if (commandParameters != null)
                        {
                            //mySqlCommand.Parameters.Clear();
                            foreach (MySqlParameter parm in commandParameters)
                                mySqlCommand.Parameters.Add(parm);
                        }
                        MySqlDataAdapter da = new MySqlDataAdapter(mySqlCommand);

                        dsData = new DataSet();
                        da.Fill(dsData);
                    }
                }
            }
            catch (Exception ex)
            {
                dsData = null;
                throw ex;
            }
        }
        #endregion

        #endregion

        /// <summary>
        /// 初始化数据库链接字符串
        /// </summary>
        /// <param name="beginConnectionStr"></param>
        /// <returns></returns>
        private String CreateDatabaseConnectionStr(String beginConnectionStr)
        {
            String databaseConnectionStr = null;
            switch (databaseType)
            {
                case DatabaseType.SqlServer:
                    databaseConnectionStr = beginConnectionStr + ";"
                                        + "Max Pool Size=" + MaxPool + ";"
                                        + "Min Pool Size=" + MinPool + ";"
                                        + "Connect Timeout=" + Conn_Timeout + ";"
                                        + "Connection Lifetime=" + Conn_Lifetime + ";"
                                        + "Pooling =True;";
                    break;
                case DatabaseType.MySql:
                    databaseConnectionStr = beginConnectionStr + ";"
                                        + "maxpoolsize=" + MaxPool + ";"
                                        + "minpoolsize=" + MinPool + ";"
                                        + "connectiontimeout=" + Conn_Timeout + ";"
                                        + "connectionlifetime=" + Conn_Lifetime + ";"
                                        + "pooling=True;SslMode = none;";
                    break;
                case DatabaseType.Oracle:
                    databaseConnectionStr = beginConnectionStr + ";"
                                        + "MIN POOL SIZE=" + MinPool + ";"
                                        + "MAX POOL SIZE=" + MaxPool + ";"
                                        //+ "CONNECTION TIMEOUT=" + Conn_Timeout + ";"
                                        + "LOAD BALANCE TIMEOUT=" + Conn_Lifetime + ";"
                                        + "POOLING=True;";
                    break;
                case DatabaseType.OleDb:
                    OleDbConnectionStringBuilder oledbStr = new OleDbConnectionStringBuilder();
                    oledbStr.DataSource = beginConnectionStr;
                    oledbStr.Provider = "Microsoft.ACE.OleDB.15.0";
                    databaseConnectionStr = oledbStr.ToString();
                    break;
                default:
                    throw new Exception("数据库类型有误或未初始化 databaseType：" + databaseType.ToString());
                    break;
            }
            return databaseConnectionStr;
        }

        /// <summary>
        /// 获取数据库链接
        /// </summary>
        /// <param name="databaseConnection"></param>
        /// <returns></returns>
        private DbConnection CreateDbConnection(String databaseConnection)
        {
            DbConnection dbConnection = null;
            switch (databaseType)
            {
                case DatabaseType.SqlServer:
                    dbConnection = new SqlConnection(databaseConnection);
                    break;
                case DatabaseType.Oracle:
                    dbConnection = new OracleConnection(databaseConnection);
                    break;
                case DatabaseType.MySql:
                    dbConnection = new MySqlConnection(databaseConnection);
                    break;
                case DatabaseType.OleDb:
                    dbConnection = new OleDbConnection(databaseConnection);
                    break;
                default:
                    throw new Exception("数据库类型有误或未初始化 databaseType：" + databaseType.ToString());
                    break;
            }
            return dbConnection;
        }

        /// <summary>
        /// 获取用于填充 System.Data.DataSet 的对象
        /// </summary>
        /// <param name="dbCommand"></param>
        /// <returns></returns>
        private DbDataAdapter CreateDbDataAdapter(DbCommand dbCommand)
        {
            DbDataAdapter dbDataAdapter = null;
            switch (databaseType)
            {
                case DatabaseType.SqlServer:
                    dbDataAdapter = new SqlDataAdapter(dbCommand as SqlCommand);
                    break;
                case DatabaseType.Oracle:
                    dbDataAdapter = new OracleDataAdapter(dbCommand as OracleCommand);
                    break;
                case DatabaseType.MySql:
                    dbDataAdapter = new MySqlDataAdapter(dbCommand as MySqlCommand);
                    break;
                case DatabaseType.OleDb:
                    dbDataAdapter = new OleDbDataAdapter(dbCommand as OleDbCommand);
                    break;
                default:
                    throw new Exception("数据库类型有误或未初始化 databaseType：" + databaseType.ToString());
                    break;
            }
            return dbDataAdapter;
        }

        /// <summary>
        /// Sql初始化参数 MakeParam("@name" , 枚举.VarChar.ToString() , 50 ,value) as SqlParameter
        /// </summary>
        /// <param name="paramName">参数名称</param>
        /// <param name="dbType">SqlDbType/MySqlDbType枚举</param>
        /// <param name="size">参数长度</param>
        /// <param name="direction">参数类型</param>
        /// <param name="value">参数值</param>
        /// <returns>SqlParameter/MySqlParameter类型</returns>
        public DbParameter MakeParam(string paramName, String dbType, Int32 size, ParameterDirection direction, object value)
        {
            DbParameter dbParameter = null;
            switch (databaseType)
            {
                case DatabaseType.SqlServer:
                    dbParameter = new SqlParameter();
                    ((SqlParameter)dbParameter).SqlDbType = (SqlDbType)Enum.Parse(typeof(SqlDbType), dbType);
                    break;
                case DatabaseType.Oracle:
                    dbParameter = new OracleParameter();
                    ((OracleParameter)dbParameter).OracleType = (OracleType)Enum.Parse(typeof(OracleType), dbType);
                    break;
                case DatabaseType.MySql:
                    dbParameter = new MySqlParameter();
                    ((MySqlParameter)dbParameter).MySqlDbType = (MySqlDbType)Enum.Parse(typeof(MySqlDbType), dbType);
                    break;
                case DatabaseType.OleDb:
                    dbParameter = new OleDbParameter();
                    ((OleDbParameter)dbParameter).OleDbType = (OleDbType)Enum.Parse(typeof(OleDbType), dbType);
                    break;
                default:
                    break;
            }
            dbParameter.ParameterName = paramName;
            if (size > 0) dbParameter.Size = size;
            dbParameter.Direction = direction;

            if ((direction == ParameterDirection.Input || direction == ParameterDirection.InputOutput) && value != null) dbParameter.Value = value;

            //if (!(direction == ParameterDirection.Output && value == null)) dbParameter.Value = value;

            return dbParameter;
        }
    }
}
