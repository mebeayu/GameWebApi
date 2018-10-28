 
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameWebApi.DAL
{
    public class SqlDataBase : DataBase
    {
        public override string ConnectionString
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["ConnectString"];
            }
        }

        /// <summary>
        /// 尽少用
        /// </summary>
        /// <returns></returns>
        public override DbCommand CreateCommand()
        {
            return new System.Data.SqlClient.SqlCommand(); 
        }

        /// <summary>
        /// 常用
        /// </summary>
        /// <returns></returns>
        public override DbConnection CreateConnection()
        {
            DbConnection conn = new System.Data.SqlClient.SqlConnection(); 
            conn.ConnectionString = ConnectionString;
            return conn;
        }


    }
}
