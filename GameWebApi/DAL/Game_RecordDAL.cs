using GameWebApi.Models;
using GameWebApi.util;
using System;
using System.Collections.Generic;

namespace GameWebApi.DAL
{
    public class Game_RecordDAL
    {


        public async void saveWinRecord(GameWinRecord gameWinRecord, string type, string year)
        {

            SqlDataBase db = new SqlDataBase();
            string sql = "insert into game_record_win_" + type + "_" + year + " (uid,goldenBeans,redPacket,time_stamp,result_type,play_id,result_message,goldenCoin,freeRedPacket) VALUES(@uid,@goldenBeans,@redPacket,@time_stamp,@result_type,@play_id,@result_message,@goldenCoin,@freeRedPacket)";
            int rs = db.Execute(sql, new { uid = gameWinRecord.uid, goldenBeans = gameWinRecord.goldenBeans, redPacket = gameWinRecord.redPacket, time_stamp = gameWinRecord.timeStamp, result_type = gameWinRecord.resultType, play_id = gameWinRecord.playId, result_message = gameWinRecord.resultMessage, goldenCoin = gameWinRecord.goldenCoin, freeRedPacket = gameWinRecord.freeRedPacket });
            // return rs;
        }
        public async void saveLoseRecord(GameWinRecord gameWinRecord, string type, string year)
        {
            SqlDataBase db = new SqlDataBase();
            string sql = "insert into game_record_lose_" + type + "_" + year + " (uid,goldenBeans,redPacket,time_stamp,result_type,play_id,result_message,goldenCoin,freeRedPacket) VALUES(@uid,@goldenBeans,@redPacket,@time_stamp,@result_type,@play_id,@result_message,@goldenCoin,@freeRedPacket)";
            int rs = db.Execute(sql, new { uid = gameWinRecord.uid, goldenBeans = gameWinRecord.goldenBeans, redPacket = gameWinRecord.redPacket, time_stamp = gameWinRecord.timeStamp, result_type = gameWinRecord.resultType, play_id = gameWinRecord.playId, result_message = gameWinRecord.resultMessage, goldenCoin = gameWinRecord.goldenCoin, freeRedPacket = gameWinRecord.freeRedPacket });
            //return rs;
        }
        public string getWinPlayId(string play_id, string type, string year)
        {
            string sql = "SELECT play_id FROM game_record_win_" + type + "_" + year + " WHERE play_id = @play_id";
            try
            {
                SqlDataBase db = new SqlDataBase();
                string rs = db.Single<string>(sql, new { play_id = play_id });
                return rs;
            }
            catch (Exception)
            {

                return null;
            }
        }
        public t_GameWinRecord getWinAllByPlayId(string play_id, string type, string year)
        {
            string sql = "SELECT * FROM game_record_lose_" + type + "_" + year + " WHERE play_id = @play_id";
            try
            {
                SqlDataBase db = new SqlDataBase();
                var rs = db.Select<t_GameWinRecord>(sql, new { play_id = play_id });
                if (rs != null && rs.Count > 0)
                {
                    return rs[0];
                }
                return null;
            }
            catch (Exception)
            {

                return null;
            }
        }

        public string _getWinPlayId(string play_id, string type, string year, int uid)
        {
            string sql = "SELECT play_id FROM game_record_win_" + type + "_" + year + " WHERE play_id = @play_id and uid = @uid";
            try
            {
                SqlDataBase db = new SqlDataBase();
                string rs = db.Single<string>(sql, new { play_id = play_id, uid = uid });
                return rs;
            }
            catch (Exception)
            {

                return null;
            }
        }
        public int getPlayCount(string play_id, string type, string year)
        {
            string sql = "SELECT count(*) FROM game_record_lose_" + type + "_" + year + " WHERE play_id = @play_id";
            try
            {
                SqlDataBase db = new SqlDataBase();
                int rs = db.ExecuteScalar<int>(sql, new { play_id = play_id });
                return rs;
            }
            catch (Exception)
            {

                return 0;
            }
        }
        public UserAccount getUserInfoByToken(string token)
        {
            if (string.IsNullOrEmpty(token) || "null" == token || "undefined" == token)
            {
                return null;
            }
            int userId = UserTokenUtils.getIdByTokenString(token);
            string sql = @"SELECT *,
case when DATEDIFF(day, addtime, getdate())> isnull((select top 1 value from rrl_config where item = 'NewUser_Confirm_Days'),7)
then '0' ELSE '1' end as is_news_user
 FROM rrl_user WHERE id = @id";
            try
            {
                SqlDataBase db = new SqlDataBase();
                UserAccount user = db.Single<UserAccount>(sql, new { id = userId });
                return user;
            }
            catch (Exception)
            {

                return null;
            }
            /*
            string sql = "SELECT * FROM rrl_user WHERE short_time_token = @token";
            try
            {
                SqlDataBase db = new SqlDataBase();
                UserAccount user = db.Single<UserAccount>(sql, new { token = token });
                return user;
            }
            catch (Exception)
            {

                return null;
            }
            */

        }

        public UserAccount getUserInfoById(int userId)
        {

            string sql = @"SELECT *,
case when DATEDIFF(day, addtime, getdate())> isnull((select top 1 value from rrl_config where item = 'NewUser_Confirm_Days'),7)
then '0' ELSE '1' end as is_news_user
 FROM rrl_user WHERE id = @id";
            try
            {
                SqlDataBase db = new SqlDataBase();
                UserAccount user = db.Single<UserAccount>(sql, new { id = userId });
                return user;
            }
            catch (Exception)
            {

                return null;
            }


        }
        public int updateUserGoldenBeans(int id, decimal goldenBeans, decimal redPacket, decimal freeRedPacket)
        {
            SqlDataBase db = new SqlDataBase();

            // lcl 2018-07-13 Insert
            if (freeRedPacket > 0)
            {
                // 免费红包大于0，则表示是赢了，直接转金豆
                goldenBeans = goldenBeans + freeRedPacket;
                // 免费红包设置为0，不参与免费红包账户的累加
                freeRedPacket = 0;
            }
           
            string sql = "UPDATE rrl_user SET h_money = h_money + @goldenBeans ,h_money_free = h_money_free + @freeRedPacket,h_money_pay = h_money_pay + @redPacket WHERE id =@id and h_money+@goldenBeans>=0 and h_money_free+@freeRedPacket >=0 and isnull(h_money_pay,0) + @redPacket >= 0 and isnull(is_locked_login,'0')='0' and isnull(is_locked_trade,'0')='0'";
            ////去掉所有的锁2018-08-07 20:05:00
            //string sql = "UPDATE rrl_user SET h_money = h_money + @goldenBeans ,h_money_free = h_money_free + @freeRedPacket,h_money_pay = h_money_pay + @redPacket WHERE id =@id and h_money+@goldenBeans>=0 and h_money_free+@freeRedPacket >=0 and isnull(h_money_pay,0) + @redPacket >= 0 ";
            int rs = db.Execute(sql, new { id = id, goldenBeans = goldenBeans, redPacket = redPacket, freeRedPacket = freeRedPacket });
            return rs;
        }

        /// <summary>
        /// 疯狂转转赚
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="goldenBeans"></param>
        /// <param name="redPacket"></param>
        /// <param name="freeRedPacket"></param>
        /// <returns></returns>
        public GameMoney updateUserGoldenBeansV2(int uid, decimal goldenBeans, decimal redPacket, decimal freeRedPacket,decimal total_play_money)
        {
            SqlDataBase db = new SqlDataBase();
            List<GameMoney> list = db.ExecuteStoredProcedure<GameMoney>("sp_v3_gamewebapi_update_user_account", new { uid = uid, goldenBeans = goldenBeans, redPacket = redPacket, freeRedPacket = freeRedPacket, total_play_money = @total_play_money });
            //string sql = "UPDATE rrl_user SET h_money = h_money + @goldenBeans ,h_money_free = h_money_free + @freeRedPacket,h_money_pay = h_money_pay + @redPacket WHERE id =@id and h_money+@goldenBeans>=0 and h_money_free+@freeRedPacket >=0 and isnull(h_money_pay,0) + @redPacket >= 0 and isnull(is_locked_login,'0')='0' and isnull(is_locked_trade,'0')='0'";
            //int rs = db.Execute(sql, new { id = id, goldenBeans = goldenBeans, redPacket = redPacket, freeRedPacket = freeRedPacket });
            if (list == null || list.Count == 0)
                return null;
            else
                return list[0];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="goldenBeans"></param>
        /// <param name="redPacket"></param>
        /// <param name="freeRedPacket"></param>
        /// <param name="type">1=开始玩,2=游戏结束</param>
        /// <returns></returns>
        public int _updateUserGoldenBeans(int id, decimal goldenBeans, decimal redPacket, decimal freeRedPacket, int type)
        {
            SqlDataBase db = new SqlDataBase();
            string sql = "";
            if (type == 1)
            {

                sql = "UPDATE rrl_user SET h_money = h_money + @goldenBeans ,h_money_free = h_money_free + @freeRedPacket,h_money_pay = h_money_pay + @redPacket ,is_locked_trade = 1 WHERE id =@id and h_money+@goldenBeans>=0 and h_money_free+@freeRedPacket >=0 and isnull(h_money_pay,0) + @redPacket >= 0 and isnull(is_locked_login,'0')='0' and isnull(is_locked_trade,'0')='0'";
                //去掉所有的锁2018-08-07 20:05:00
                //sql = "UPDATE rrl_user SET h_money = h_money + @goldenBeans ,h_money_free = h_money_free + @freeRedPacket,h_money_pay = h_money_pay + @redPacket WHERE id =@id and h_money+@goldenBeans>=0 and h_money_free+@freeRedPacket >=0 and isnull(h_money_pay,0) + @redPacket >= 0 ";
            }
            else if (type == 2)
            {
                // lcl 2018-07-13 Insert
                if (freeRedPacket > 0)
                {
                    // 免费红包大于0，则表示是赢了，直接转金豆
                    goldenBeans = goldenBeans + freeRedPacket;
                    // 免费红包设置为0，不参与免费红包账户的累加
                    freeRedPacket = 0;
                }
                //去掉所有的锁2018-08-07 20:05:00
                //sql = "UPDATE rrl_user SET h_money = h_money + @goldenBeans ,h_money_free = h_money_free + @freeRedPacket,h_money_pay = h_money_pay + @redPacket  WHERE id =@id and h_money+@goldenBeans>=0 and h_money_free+@freeRedPacket >=0 and isnull(h_money_pay,0) + @redPacket >= 0 ";
                sql = "UPDATE rrl_user SET h_money = h_money + @goldenBeans ,h_money_free = h_money_free + @freeRedPacket,h_money_pay = h_money_pay + @redPacket ,is_locked_trade = 0 WHERE id =@id and h_money+@goldenBeans>=0 and h_money_free+@freeRedPacket >=0 and isnull(h_money_pay,0) + @redPacket >= 0 and isnull(is_locked_login,'0')='0' and is_locked_trade = '1'";
            }
            
            int rs = db.Execute(sql, new { id = id, goldenBeans = goldenBeans, redPacket = redPacket, freeRedPacket = freeRedPacket });
            //在这里更新用户每日玩游戏次数，表：game_user_daily_count
            string today = DateTime.Now.ToString("yyyy-MM-dd");
            int res = db.Execute("update game_user_daily_count set count=count+1 where date=@today and uid=@uid", new { uid = id, today = today });
            if (res == 0)
            {
                db.Execute("insert into game_user_daily_count(uid,date,count) values(@uid,@date,1)", new { uid = id, date = today });
            }
            return rs;
        }
        public void updateUser(int id, string gameType, DateTime dateTime)
        {
            //return; //去掉所有的锁2018-08-07 20:05:00
            SqlDataBase db = new SqlDataBase();
            string sql = "UPDATE rrl_user SET is_locked_trade_game_type = @gameType,recently_game_start_time = @dateTime WHERE id =@id ";
            int rs = db.Execute(sql, new { id = id, gameType = gameType, dateTime = dateTime });
        }
        /// <summary>
        /// 锁用户
        /// </summary>
        /// <param name="id"></param>
        /// <param name="gameType"></param>
        /// <param name="dateTime"></param>
        public int lockUser(int id, string gameType, DateTime dateTime)
        {
            //return; //去掉所有的锁2018-08-07 20:05:00
            SqlDataBase db = new SqlDataBase();
            string sql = "UPDATE rrl_user SET is_locked_trade='1',is_locked_trade_game_type = @gameType,recently_game_start_time = @dateTime WHERE id =@id ";
            int rs = db.Execute(sql, new { id = id, gameType = gameType, dateTime = dateTime });
            return rs;
        }

        /// <summary>
        /// 获取指定用户游戏赢的数据
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="gameId">游戏ID</param>
        /// <returns></returns>
        public List<dynamic> GetWinDataByUser(int uid, string gameId, int dataNumber = 50)
        {
            string strTableName = $@"game_record_win_{gameId}_{DateTime.Now.Year}";
            string strSql = $@"select {GetTopSql(dataNumber)}  [id]
      ,[uid]
      ,[goldenBeans]
      ,[redPacket]
      ,[time_stamp]
      ,[result_type]
      ,[play_id]
      ,[result_type]+'，'+[result_message] as result_message
      ,[goldenCoin]
      ,[freeRedPacket] from {strTableName}(nolock)
                                where ([uid] = @uid)
                                order by [id] desc";

            return new SqlDataBase().Select<dynamic>(strSql, new { uid = uid });
        }

        /// <summary>
        /// 获取全部用户的游戏赢的数据
        /// </summary>
        /// <param name="gameId">游戏ID</param>
        /// <returns></returns>
        public List<dynamic> GetAllUserWinData(string gameId, int dataNumber = 50)
        {
            string strTableName = $@"game_record_win_{gameId}_{DateTime.Now.Year}";
            string strSql = $@"select {GetTopSql(dataNumber)} * ,u.username
                                 from {strTableName}(nolock) w
                                inner join rrl_user(nolock) u
                                   on u.[id] = w.[uid]
                                where (w.goldenBeans > 0) or (w.freeRedPacket > 0) or (w.redPacket > 0) or (w.goldenCoin > 0)
                                order by w.[id] desc";

            return new SqlDataBase().Select<dynamic>(strSql, null);
        }

        private string GetTopSql(int dataNumber)
        {
            if (dataNumber > 0)
            {
                return $@"top {dataNumber}";
            }

            return string.Empty;
        }


        /// <summary>
        /// 修改玩的次数
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="game_type"></param>
        /// <returns></returns>
        public async void upate_game_play_times(int uid, string game_type, string money_type)
        {
            var db=new SqlDataBase();
            string sql = @"MERGE INTO game_user_play_times as t
USING (SELECT @uid as uid,@game_type as game_type,@money_type as money_type) as s
ON t.uid=s.uid and t.game_type=s.game_type and t.money_type=s.money_type
WHEN MATCHED
Then update set play_times= play_times+1 
When Not Matched
Then Insert (id,uid,play_times,game_type,money_type) Values(newid(),@uid,1,@game_type,@money_type);";
           int effect= db.Execute(sql, new { uid = uid, game_type = game_type, money_type= money_type });
            

        }

        internal int force_unlock(int uid)
        {
            var db = new SqlDataBase();
            var sql = @"update rrl_user set is_locked_trade = 0 where id=@uid";
            return db.Execute(sql, new {uid=uid });
        }
    }
}