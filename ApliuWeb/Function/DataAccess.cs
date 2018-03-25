using ApliuDatabase;
using ApliuTools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ApliuWeb
{
    public class DataAccess
    {
        private static DataAccess _Instance = null;
        public static DataAccess Instance
        {
            get
            {
                if (_Instance == null) LoadDataAccess();
                return _Instance;
            }
        }

        /// <summary>
        /// 初始化数据库连接通道
        /// </summary>
        private static void LoadDataAccess()
        {
            try
            {
                DatabaseType.dbType = (DatabaseTypeEnum)Enum.Parse(typeof(DatabaseTypeEnum), SiteConfig.Instance.DatabaseType);
                DatabaseHelper.DatabaseConnection = SiteConfig.Instance.DatabaseConnection;
                _Instance = new DataAccess();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 查询数据库 Select
        /// </summary>
        /// <param name="Sql"></param>
        /// <returns></returns>
        public DataSet GetData(string Sql)
        {
            return GetData(Sql, null);
        }

        /// <summary>
        /// 更新数据库 Delete Update Insert
        /// </summary>
        /// <param name="Sql"></param>
        /// <returns></returns>
        public bool PostData(string Sql)
        {
            return PostData(Sql, null);
        }

        /// <summary>
        /// 测试用例
        /// </summary>
        [Obsolete]
        public void Ceshi()
        {
            string guid = Guid.NewGuid().ToString().ToLower();
            string sql = "insert into Test values(@name1,@name2,@name3);";
            SqlParameter[] myprams = { 
                        MakeInParam("@name1" , SqlDbType.VarChar , 50 ,guid) ,
                        MakeInParam("@name2" , SqlDbType.VarChar , 50 ,"CeshiPrams"),
                        MakeInParam("@name3" , SqlDbType.VarChar , 50 ,DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")) 
                    };
            bool p1 = PostData(sql, myprams);

            bool p2 = PostData("update Test set Msg='Ceshi' where ID='" + guid + "';");
        }

        /// <summary>
        /// 查询数据库
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="Args"></param>
        /// <returns></returns>
        public DataSet GetData(string Sql, params SqlParameter[] Args)
        {
            DataSet dsData = null;
            try
            {
                dsData = DatabaseHelper.GetData(Sql, Args);
            }
            catch (Exception ex)
            {
                Logger.WriteLogWeb("数据库查询失败，Sql：" + Sql + "，详情：" + ex.Message);
            }
            return dsData;
        }

        /// <summary>
        /// 更新数据库
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="Args"></param>
        /// <returns></returns>
        public bool PostData(string Sql, params SqlParameter[] Args)
        {
            bool result = false;
            try
            {
                if (DatabaseHelper.PostData(Sql, Args) >= 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLogWeb("数据库更新失败，Sql：" + Sql + "，详情：" + ex.Message);
            }
            return result;
        }

        /// <summary>
        /// Post 返回受影响数据条数
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="Args"></param>
        /// <returns></returns>
        public int PostDataInt(string Sql, params SqlParameter[] Args)
        {
            int result = -1;
            try
            {
                result = DatabaseHelper.PostData(Sql, Args);
            }
            catch (Exception ex)
            {
                Logger.WriteLogWeb("数据库更新失败，Sql：" + Sql + "，详情：" + ex.Message);
            }
            return result;
        }

        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="ParamName"></param>
        /// <param name="DbType"></param>
        /// <param name="Size"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public SqlParameter MakeInParam(string ParamName, SqlDbType DbType, int Size, object Value)
        {
            System.Data.SqlClient.SqlParameter sqlParams = DatabaseHelper.MakeParamSqlServer(ParamName, DbType, Size, ParameterDirection.Input, Value);
            return sqlParams;
        }
    }
}