using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApliuDatabase
{
    public class DatabaseType
    {
        private static DatabaseTypeEnum _dbType = DatabaseTypeEnum.SqlServer;
        public static DatabaseTypeEnum dbType
        {
            get
            {
                return _dbType;
            }
            set
            {
                _dbType = value;
            }
        }
    }

    public enum DatabaseTypeEnum
    {
        SqlServer = 0,
        Oracle = 1,
        MySql = 2,
        Access = 3
    }
}
