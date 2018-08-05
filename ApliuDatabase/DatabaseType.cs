using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApliuDatabase
{
    public enum DatabaseType
    {
        SqlServer = 0,
        Oracle = 1,
        MySql = 2,
        /// <summary>
        /// Microsoft Access 数据库(.accdb)
        /// </summary>
        OleDb = 3
    }
}
