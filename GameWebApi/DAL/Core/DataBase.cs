using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace GameWebApi.DAL
{
    public abstract class DataBase
    {
        public abstract string ConnectionString { get;  }

        //public virtual DbProviderFactory DbProvider { get;  }
         
        public   DbParameter CreateParameter(DbCommand cmd,String pName, Object value, System.Data.DbType type)
        {
            var p = cmd.CreateParameter();
            p.ParameterName = pName;
            p.Value = (value == null ? DBNull.Value : value);
            p.DbType = type;
            return p;
        }

        public abstract DbCommand CreateCommand();


        public abstract DbConnection CreateConnection();
        
 

        public List<T> Select<T>(string sql,  Object paramObject)
        {
             
            DbConnection conn = null;
            try
            {
                conn = CreateConnection();
                conn.Open();
                var list=  Dapper.SqlMapper.Query<T>(conn, sql, paramObject);
                return list.ToList<T>();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return null;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                }
                    
            }

        }

        public List<dynamic> Select(string sql, Object paramObject)
        {

            DbConnection conn = null;
            try
            {
                conn = CreateConnection();
                conn.Open();
                var list = Dapper.SqlMapper.Query(conn, sql, paramObject);
                return list.ToList<dynamic>();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return null;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                    conn.Dispose();

                }
            }

        }


        public  dynamic   Single(string sql, Object paramObject)
        {

            DbConnection conn = null;
            try
            {
                conn = CreateConnection();
                conn.Open();
                var list = Dapper.SqlMapper.QuerySingleOrDefault(conn, sql, paramObject);
                return list.ToList<dynamic>();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return null;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }

        }

        public T  Single<T>(string sql, Object paramObject)
        {

            DbConnection conn = null;
            try
            {
                conn = CreateConnection();
                conn.Open();
                var list = Dapper.SqlMapper.QuerySingleOrDefault<T>(conn, sql, paramObject);
                return list;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return default(T);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }

        }
        public T ExecuteScalar<T>(string sql, Object paramObject)
        {

            DbConnection conn = null;
            try
            {
                conn = CreateConnection();
                conn.Open();
                T t = Dapper.SqlMapper.ExecuteScalar<T>(conn, sql, paramObject);
                return t;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return default(T);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }

        }

        public int Execute(string sql , Object paramObject)
        {
 
            DbConnection conn = null;
            try
            {
                conn = CreateConnection();
                conn.Open();
                int count = Dapper.SqlMapper.Execute(conn, sql, paramObject);
                return count;
            }
            catch (Exception ex)
            {
                WriteLogs("DataBase", "Execute", ex.Message);
                System.Console.WriteLine(ex.Message);
                return 0;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }

        }

        public void ExecuteAsyc(string sql, Object paramObject)
        {

            DbConnection conn = null;
            try
            {
                conn = CreateConnection();
                conn.Open();
                Dapper.SqlMapper.ExecuteAsync(conn, sql, paramObject);
                
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }

        }
        /// <summary>
        /// 自行维护事务和连接
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="sql"></param>
        /// <param name="paramObject"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public int ExecuteTran(DbConnection conn, string sql, Object paramObject, DbTransaction transaction)
        {
            int count = Dapper.SqlMapper.Execute(conn, sql, paramObject, transaction);
            return count;
        }


        public int Execute_Tran(DbConnection conn, string sql, Object paramObject, DbTransaction transaction)
        {
            int count = Dapper.SqlMapper.Execute(conn, sql, paramObject, transaction);
            return count;
        }


        public List<T> ExecuteStoredProcedure<T>(string sql, Object paramObject)
        {

            DbConnection conn = null;
            try
            {
                conn = CreateConnection();
                conn.Open();
                var list = Dapper.SqlMapper.Query<T>(conn, sql, paramObject, null, true, null, System.Data.CommandType.StoredProcedure);
                return list.ToList<T>();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return null;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }

        }

        public GridReader ExecuteStoredProcedureMultiple(string sql, Object paramObject)
        {

            DbConnection conn = null;
            try
            {
                conn = CreateConnection();
                conn.Open();
                var list = Dapper.SqlMapper.QueryMultiple(conn, sql, paramObject, null, null, System.Data.CommandType.StoredProcedure);
                // return list.ToList<T>();
                return list;

            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return null;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }

        }
        public static void WriteLogs(string fileName, string type, string content)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            if (!string.IsNullOrEmpty(path))
            {
                path = AppDomain.CurrentDomain.BaseDirectory + fileName;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                path = path + "\\" + DateTime.Now.ToString("yyyyMMdd");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                path = path + "\\" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                if (!File.Exists(path))
                {
                    FileStream fs = File.Create(path);
                    fs.Close();
                }
                if (File.Exists(path))
                {
                    StreamWriter sw = new StreamWriter(path, true, System.Text.Encoding.Default);
                    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + type + "-->" + content);
                    //  sw.WriteLine("----------------------------------------");
                    sw.Close();
                }
            }
        }
    }
}
