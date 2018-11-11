
using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace RRL.DB
{
    // ReSharper disable once InconsistentNaming
    public class RRLPICDB : DBSQL
    {
        public RRLPICDB()   
        {
            string ConnectString = System.Configuration.ConfigurationManager.AppSettings["ConnectString"];
            
        }

        public long AddPic(string filetype, string base64)
        {
            if(string.IsNullOrWhiteSpace(base64))
            {
                return 0;
            }
            var thekey = GetMD5Hash(base64);
            var ds = ExeQuery("select id from rrl_pics where thekey=@key", new SqlParameter("key", thekey));
            if (ds == null) return 0;
            if (ds.Tables[0].Rows.Count > 0)
            {
                return int.Parse(ds.Tables[0].Rows[0][0].ToString());
            }
            var imageBytes = Convert.FromBase64String(base64);

            ds = ExeQuery("insert into rrl_pics(filetype,thekey,pic_data,lenght) output inserted.id values(@filetype,@thekey,@pic_data,@lenght)",
                new SqlParameter("filetype", filetype), new SqlParameter("thekey", thekey),
                new SqlParameter("pic_data", imageBytes), new SqlParameter("lenght", imageBytes.LongLength));
            if (ds?.Tables[0].Rows.Count > 0)
            {
                var bid = long.Parse(ds.Tables[0].Rows[0][0].ToString());
                var res = ExeCMD("update rrl_pics set bid=@bid where id=@bid", new SqlParameter("bid", bid));
                return bid;
            }
            return 0;
        }

        private string GetMD5Hash(string input)
        {
            var md5 = new MD5CryptoServiceProvider();
            var bs = Encoding.UTF8.GetBytes(input);
            bs = md5.ComputeHash(bs);
            var s = new StringBuilder();
            foreach (var b in bs)
            {
                s.Append(b.ToString("x2"));
            }
            var res = s.ToString();
            return res;
        }
    }

    // ReSharper disable once InconsistentNaming
    public class RRLDB : DBSQL
    {
        public RRLDB() : base(System.Configuration.ConfigurationManager.AppSettings["ConnectString"])
        {
           
        }
    }
}