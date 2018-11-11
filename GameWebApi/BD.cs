using System;
using System.Data;
using System.Data.SqlClient;

namespace RRL.DB
{
    /// <summary>
    /// sqlserver
    /// </summary>
    public class DBSQL
    {
        private SqlConnection _conn;
        private SqlCommand _sm;

        public SqlConnection conn
        {
            get { return _conn; }
            private set { _conn = value; }
        }

        public SqlCommand sm
        {
            get { return _sm; }
            private set { _sm = value; }
        }

        public DBSQL()
        {
        }

        public DBSQL(string strConnectString)
        {
            //"Data Source=10.43.4.16;Initial Catalog=zbb;Persist Security Info=True;User ID=jfsys;Password=jfsys123"
            conn = new SqlConnection(strConnectString);
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            sm = new SqlCommand("", conn);
        }

        public DBSQL(string strAddr, string DBName, string ID, string Password)
        {
            conn = new SqlConnection(
                $@"Data Source={strAddr};Initial Catalog={DBName};Persist Security Info=True;User ID={ID};Password={
                    Password
                };Pooling=true;min pool size=5;max pool size=512");
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            sm = new SqlCommand("", conn);
        }

        public void ConnectSQLServerDB(string strAddr, string DBName, string ID, string Password)
        {
            conn = new SqlConnection(
                $@"Data Source={strAddr};Initial Catalog={DBName};Persist Security Info=True;User ID={ID};Password={
                    Password
                };Pooling=true;min pool size=5;max pool size=512");
            conn.Open();
            sm = new SqlCommand("", conn);
        }

        public void Close()
        {
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
                
            }
            if (conn!=null)
            {
                conn.Dispose();
            }
        }

        public DataSet ExeQuery(string sql)
        {
            SqlDataAdapter da = new SqlDataAdapter(sql, conn);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }

        public DataSet ExeQuery(string sql, params SqlParameter[] AParams)
        {
            try
            {
                sm.Parameters.Clear();
                sm.CommandText = sql;
                sm.Parameters.AddRange(AParams);
                //foreach (SqlParameter param in AParams)
                //{
                //    sm.Parameters.Add(param);
                //}
                SqlDataAdapter Adapter = new SqlDataAdapter(sm);
                DataSet Result = new DataSet();
                Adapter.Fill(Result);
                return Result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void BeginTran()
        {
            sm.Transaction = conn.BeginTransaction();
        }

        public void RollBackTran()
        {
            sm.Transaction.Rollback();
        }

        public void CommitTran()
        {
            sm.Transaction.Commit();
        }

        public int ExeCMD(string sql)
        {
            try
            {
                sm.CommandText = sql;
                int res = sm.ExecuteNonQuery();
                return res;
            }
            catch (Exception e)
            {
                return -1;
            }
        }

        public int ExeCMD(string sql, params SqlParameter[] AParams)
        {
            try
            {
                sm.Parameters.Clear();
                sm.CommandText = sql;
                sm.Parameters.AddRange(AParams);
                //foreach (SqlParameter param in AParams)
                //{
                //    sm.Parameters.Add(param);
                //}
                return sm.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                // string s = e.Message;
                return -1;
            }
        }


        public int ExecSP(string StoredProcedureName, params SqlParameter[] AParams)
        {
            try
            {
                sm.Parameters.Clear();
                sm.CommandText = StoredProcedureName;
                sm.Parameters.Add(new SqlParameter("@return", SqlDbType.Int));
                sm.Parameters["@return"].Direction = ParameterDirection.ReturnValue;
                foreach (SqlParameter param in AParams)
                {
                    param.Direction = ParameterDirection.Input;
                    sm.Parameters.Add(param);
                }
                //sm.Parameters.AddRange(AParams);
                sm.CommandType = CommandType.StoredProcedure;
                int res = sm.ExecuteNonQuery();
                return res;
            }
            catch
            {
                return -1;
            }
        }
    }
}