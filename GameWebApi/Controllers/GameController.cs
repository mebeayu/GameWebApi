using GameWebApi.DAL;
using GameWebApi.Models;
using GameWebApi.util;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using System.Text;
using System.Linq;
using System.Data;
using Newtonsoft.Json;

namespace GameWebApi.Controllers
{
    public class GameController : ApiController, IDisposable
    {

        Game_RecordDAL game_RecordDAL = new Game_RecordDAL();
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // 获取个人游戏数据
        [HttpGet]
        [ActionName("get_userinfo")]
        public HttpResponseMessage get_userinfo(string token)
        {
            ResultData resultData = new ResultData();
            if (token != null && token.Length > 0)
            {
                var result = game_RecordDAL.getUserInfoByToken(token);
                if (result != null)
                {
                    if ("1".Equals(result.is_locked_login))
                    {
                        resultData.status = "1";
                        resultData.message = "登录被锁,不能玩游戏,请用注册手机号码联系客服!";
                        return ToJsonUtil.toJson(resultData);
                    }

                    if ("1".Equals(result.is_locked_trade))
                    {
                        resultData.status = "1";
                        string lockedGameType = result.is_locked_trade_game_type;
                        if ("0007".Equals(lockedGameType))
                        {
                            resultData.message = "你正玩斗地主游戏，请返回游戏，结束后，才能继续玩其它游戏或交易，";
                        }
                        else
                        {
                            resultData.message = "交易功能被锁,不能玩游戏,请用注册手机号码联系客服!";
                        }
                        return ToJsonUtil.toJson(resultData);
                    }
                    string imgeUrl = "";
                    if (!result.head_pic.Equals("0") && result.head_pic.Length > 0)
                    {
                        imgeUrl = "https://e-shop.rrlsz.com.cn/WebApi/Public/picture/" + result.head_pic;
                    }
                    else
                    {
                        imgeUrl = "https://e-shop.rrlsz.com.cn/Assets/Source/img/%E9%BB%98%E8%AE%A4%E5%9B%BE%E5%83%8F.png";//modify by zetee
                    }
                    resultData.status = "0";
                    resultData.message = "获取数据成功";
                    string name = result.real_name;
                    if (string.IsNullOrWhiteSpace(name))
                    {
                        string username = result.username;
                        name = System.Text.RegularExpressions.Regex.Replace(username, "(\\d{3})\\d{4}(\\d{4})", "$1****$2");
                    }
                    else
                    {
                        name = result.real_name;
                    }
                    resultData.data = new { uid = result.id, redPacket = result.h_money_pay, goldenBeans = result.h_money, name = name, imgUrl = imgeUrl, freeRedPacket = result.h_money_free, is_news_user = result.is_news_user };
                }
                else
                {
                    resultData.status = "1";
                    resultData.message = "获取数据失败";
                }
            }
            else
            {
                resultData.status = "1";
                resultData.message = "token不能为空！";
            }
            return ToJsonUtil.toJson(resultData);
        }

        private object getUserInfo(int userId)
        {
            if (userId > 0)
            {
                var result = game_RecordDAL.getUserInfoById(userId);
                if (result != null)
                {
                    if ("1".Equals(result.is_locked_login))
                    {
                        return null;
                    }

                    if ("1".Equals(result.is_locked_trade))
                    {
                        return null;
                    }
                    string imgeUrl = "";
                    if (!result.head_pic.Equals("0") && result.head_pic.Length > 0)
                    {
                        imgeUrl = "https://e-shop.rrlsz.com.cn/WebApi/Public/picture/" + result.head_pic;
                    }
                    else
                    {
                        imgeUrl = "https://e-shop.rrlsz.com.cn/Assets/Source/img/%E9%BB%98%E8%AE%A4%E5%9B%BE%E5%83%8F.png";//modify by zetee
                    }

                    string name = result.real_name;
                    if (string.IsNullOrWhiteSpace(name))
                    {
                        string username = result.username;
                        name = System.Text.RegularExpressions.Regex.Replace(username, "(\\d{3})\\d{4}(\\d{4})", "$1****$2");
                    }
                    else
                    {
                        name = result.real_name;
                    }
                    return new { uid = result.id, redPacket = result.h_money_pay, goldenBeans = result.h_money, name = name, imgUrl = imgeUrl, freeRedPacket = result.h_money_free, is_news_user = result.is_news_user };
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }


        // 获取个人游戏数据
        [HttpGet]
        [ActionName("get_game_userinfo")]
        public HttpResponseMessage get_game_userinfo(string token)
        {
            ResultData resultData = new ResultData();
            if (token != null && token.Length > 0)
            {
                var result = game_RecordDAL.getUserInfoByToken(token);
                if (result != null)
                {
                    if ("1".Equals(result.is_locked_login))
                    {
                        resultData.status = "1";
                        resultData.message = "登录被锁,不能玩游戏,请用注册手机号码联系客服!";
                        return ToJsonUtil.toJson(resultData);
                    }
                    string imgeUrl = "https://e-shop.rrlsz.com.cn/Assets/Source/img/%E9%BB%98%E8%AE%A4%E5%9B%BE%E5%83%8F.png";
                    //if (!result.head_pic.Equals("0") && result.head_pic.Length > 0)
                    //{
                    //    imgeUrl = "https://e-shop.rrlsz.com.cn/WebApi/Public/picture/" + result.head_pic;
                    //}
                    //else
                    //{
                    //    imgeUrl = "https://e-shop.rrlsz.com.cn/Assets/Source/img/%E9%BB%98%E8%AE%A4%E5%9B%BE%E5%83%8F.png";//modify by zetee
                    //}
                    resultData.status = "0";
                    resultData.message = "获取数据成功";
                    //string name = System.Text.RegularExpressions.Regex.Replace(result.username, "(\\d{3})\\d{8}", "$1********");//result.real_name;
                    string name = result.username.Remove(3, 4).Insert(3, "****");
                    //if (name == null || name.Length < 1)
                    //{
                    //    string username = result.username;
                    //    name = System.Text.RegularExpressions.Regex.Replace(username, "(\\d{3})\\d{8}", "$1********");
                    //}
                    //else
                    //{
                    //    name = result.real_name;
                    //}
                    SqlDataBase db = new SqlDataBase();
                    string today = DateTime.Now.ToString("yyyy-MM-dd");
                    List<SettleAccounts> l = db.Select<SettleAccounts>("select count from game_user_daily_count where uid=@uid and date=@today and active=1",
                                    new { uid = result.id, today = today });
                    int PlayToday = 0;//今天游戏局数
                    if (l != null)
                    {
                        if (l.Count > 0) PlayToday = l[0].count;
                    }
                    //int need_count = Convert.ToInt32(Convert.ToDouble(result.h_money_pay) / 1.5);
                    resultData.data = new
                    {
                        uid = result.id,
                        redPacket = result.h_money_pay,
                        goldenBeans = result.h_money,
                        name = name,
                        imgUrl = imgeUrl,
                        freeRedPacket = result.h_money_free,
                        isLockedTrade = result.is_locked_trade,
                        isLockedTradeGameType = result.is_locked_trade_game_type,
                        is_news_user = result.is_news_user,
                        head_pic = result.head_pic,
                        real_name = result.real_name,
                        play_count = PlayToday,
                        need_count = result.need_play_conut

                    };
                }
                else
                {
                    resultData.status = "1";
                    resultData.message = "获取数据失败";
                }
            }
            else
            {
                resultData.status = "1";
                resultData.message = "token不能为空！";
            }
            return ToJsonUtil.toJson(resultData);
        }
        //每玩完一局更新账户数据
        [HttpPost]
        [ActionName("update_play_new")]
        public HttpResponseMessage update_play_new([FromBody] UpdatePlay updatePlay)
        {
            ResultData resultData = new ResultData();
            //if (updatePlay.freeRedPacket < 0)
            //{
            //    resultData.status = "1";
            //    resultData.message = "暂时不支持免费红包押注";
            //    return ToJsonUtil.toJson(resultData);
            //}
            if (updatePlay.type == null && updatePlay.type.Length == 0)
            {
                resultData.status = "1";
                resultData.message = "游戏类型必传";
                return ToJsonUtil.toJson(resultData);
            }
            if (updatePlay.timestamp <= 0)
            {
                resultData.status = "1";
                resultData.message = "时间戳必传";
                return ToJsonUtil.toJson(resultData);
            }
            DateTime dateTime = GetDateTimeUtil.GetDateTime(updatePlay.timestamp);
            string year = dateTime.Year.ToString();
            if (updatePlay.playId != null && updatePlay.playId.Length > 0)
            {
                int count = game_RecordDAL.getPlayCount(updatePlay.playId, updatePlay.type, year);
                {
                    if (count != 1)
                    {
                        resultData.status = "1";
                        resultData.message = "当前局游戏不存在";
                        return ToJsonUtil.toJson(resultData);
                    }
                }
            }

            GameWinRecord gameRecord = new GameWinRecord();
            gameRecord.uid = updatePlay.uid;
            gameRecord.goldenBeans = updatePlay.goldenBeans;
            gameRecord.redPacket = updatePlay.redPacket;
            gameRecord.resultType = updatePlay.resultType;
            gameRecord.timeStamp = dateTime;
            gameRecord.goldenCoin = updatePlay.goldenCoin;
            gameRecord.freeRedPacket = updatePlay.freeRedPacket;
            //押注
            if (updatePlay.goldenBeans < 0 || updatePlay.redPacket < 0 || updatePlay.goldenCoin < 0 || updatePlay.freeRedPacket < 0)
            {
                int rs = game_RecordDAL.updateUserGoldenBeans(updatePlay.uid, updatePlay.goldenBeans, updatePlay.redPacket, updatePlay.freeRedPacket);
                if (rs > 0)
                {
                    string resultMessage = "";
                    if (updatePlay.goldenBeans < 0)
                    {
                        resultMessage = "押了" + updatePlay.goldenBeans + "豆,";
                    }
                    if (updatePlay.redPacket < 0)
                    {
                        resultMessage = resultMessage + "押了" + updatePlay.redPacket + "小红包,";
                    }
                    if (updatePlay.freeRedPacket < 0)
                    {
                        resultMessage = resultMessage + "押了" + updatePlay.freeRedPacket + "免费红包,";
                    }
                    if (updatePlay.goldenCoin < 0)
                    {
                        resultMessage = resultMessage + "押了" + updatePlay.goldenCoin + "金币,";
                    }
                    gameRecord.playId = Guid.NewGuid().ToString();
                    gameRecord.resultMessage = resultMessage;
                    game_RecordDAL.saveLoseRecord(gameRecord, updatePlay.type, year);
                    //统计玩的次数
                    UpateGamePlayTimes(updatePlay.uid, updatePlay.type, updatePlay.freeRedPacket, updatePlay.redPacket);

                    resultData.status = "0";
                    resultData.message = "更新数据成功";
                    resultData.data = new { playId = gameRecord.playId };

                }
                else
                {
                    resultData.status = "1";
                    resultData.message = "个人账户金豆或红包不足或帐号锁定";
                }
            }//赢豆
            else if (updatePlay.goldenBeans > 0 || updatePlay.redPacket > 0 || updatePlay.goldenCoin > 0 || updatePlay.freeRedPacket > 0)
            {

                string playId = updatePlay.playId;
                if (playId == null)
                {
                    resultData.status = "1";
                    resultData.message = "playId不能为null";
                    return ToJsonUtil.toJson(resultData);
                }
                if (playId.Length < 1)
                {
                    resultData.status = "1";
                    resultData.message = "playId不能空值字符串";
                    return ToJsonUtil.toJson(resultData);
                }
                //判断数据里面是否添加当前这局赢的游戏记录
                string pid = game_RecordDAL.getWinPlayId(updatePlay.playId, updatePlay.type, year);
                if (string.IsNullOrWhiteSpace(pid))
                {
                    int rs = game_RecordDAL.updateUserGoldenBeans(updatePlay.uid, updatePlay.goldenBeans, updatePlay.redPacket, updatePlay.freeRedPacket);
                    if (rs > 0)
                    {
                        string resultMessage = "";
                        if (updatePlay.goldenBeans > 0)
                        {
                            resultMessage = "赢了" + updatePlay.goldenBeans + "豆,";
                        }
                        if (updatePlay.redPacket > 0)
                        {
                            resultMessage = resultMessage + "赢了" + updatePlay.redPacket + "小红包,";
                        }
                        if (updatePlay.freeRedPacket > 0)
                        {
                            //resultMessage = resultMessage + "赢了" + updatePlay.freeRedPacket + "免费红包,";

                            // lcl 2018-07-13 Modify 免费红包直接转为了金豆，因此提示为赢了豆
                            resultMessage = resultMessage + "赢了" + updatePlay.freeRedPacket + "豆,";
                        }
                        if (updatePlay.goldenCoin > 0)
                        {
                            resultMessage = resultMessage + "赢了" + updatePlay.goldenCoin + "金币,";
                        }
                        gameRecord.resultMessage = resultMessage;
                        gameRecord.playId = updatePlay.playId;
                        game_RecordDAL.saveWinRecord(gameRecord, updatePlay.type, year);
                        resultData.status = "0";
                        resultData.message = "更新数据成功";

                        // 根据账户信息清除领红包的次数记录
                        //ClearRedpackageRecord(updatePlay.uid);
                    }
                    else
                    {
                        resultData.status = "1";
                        resultData.message = "更新数据还失败";
                    }
                }
                else
                {
                    resultData.status = "1";
                    resultData.message = "更新数据还失败";
                }
            }

            // lcl 2018-07-13 Insert
            if (!string.IsNullOrWhiteSpace(updatePlay.playId))
            {
                resultData.redpackageData = GetAutoRedpackageData(updatePlay.uid);
            }

            return ToJsonUtil.toJson(resultData);
        }

        //每玩完一局更新账户数据
        [HttpPost]
        [ActionName("update_play")]
        public HttpResponseMessage update_play([FromBody] UpdatePlay updatePlay)
        {
            ResultData resultData = new ResultData();
            //if (updatePlay.freeRedPacket<0)
            //{
            //    resultData.status = "1";
            //    resultData.message = "暂时不支持免费红包押注";
            //        return ToJsonUtil.toJson(resultData);
            //}



            if (updatePlay.type == null && updatePlay.type.Length == 0)
            {
                resultData.status = "1";
                resultData.message = "游戏类型必传";
                return ToJsonUtil.toJson(resultData);
            }
            if (updatePlay.timestamp <= 0)
            {
                resultData.status = "1";
                resultData.message = "时间戳必传";
                return ToJsonUtil.toJson(resultData);
            }
            int rs = game_RecordDAL.updateUserGoldenBeans(updatePlay.uid, updatePlay.goldenBeans, updatePlay.redPacket, updatePlay.freeRedPacket);
            if (rs > 0)
            {
                DateTime dateTime = GetDateTimeUtil.GetDateTime(updatePlay.timestamp);
                string year = dateTime.Year.ToString();
                GameWinRecord gameWinRecord = new GameWinRecord();
                gameWinRecord.playId = Guid.NewGuid().ToString();
                gameWinRecord.uid = updatePlay.uid;
                gameWinRecord.goldenBeans = updatePlay.goldenBeans;
                gameWinRecord.redPacket = updatePlay.redPacket;
                gameWinRecord.resultType = updatePlay.resultType;
                gameWinRecord.timeStamp = dateTime;
                gameWinRecord.goldenCoin = updatePlay.goldenCoin;
                gameWinRecord.freeRedPacket = updatePlay.freeRedPacket;
                if (updatePlay.goldenBeans > 0 || updatePlay.redPacket > 0 || updatePlay.goldenCoin > 0 || updatePlay.freeRedPacket > 0)
                {
                    string resultMessage = "";
                    if (updatePlay.goldenBeans > 0)
                    {
                        resultMessage = "赢了" + updatePlay.goldenBeans + "豆,";
                    }
                    if (updatePlay.redPacket > 0)
                    {
                        resultMessage = resultMessage + "赢了" + updatePlay.redPacket + "小红包,";
                    }
                    if (updatePlay.freeRedPacket > 0)
                    {
                        //resultMessage = resultMessage + "赢了" + updatePlay.redPacket + "免费红包,";

                        // lcl 2018-07-13 Modify 免费红包直接转为了金豆，因此提示为赢了豆
                        resultMessage = resultMessage + "赢了" + updatePlay.freeRedPacket + "豆,";
                    }
                    if (updatePlay.goldenCoin > 0)
                    {
                        resultMessage = resultMessage + "赢了" + updatePlay.goldenCoin + "金币,";
                    }
                    gameWinRecord.resultMessage = resultMessage;
                    game_RecordDAL.saveWinRecord(gameWinRecord, updatePlay.type, year);

                    // 根据账户信息清除领红包的次数记录
                    //ClearRedpackageRecord(updatePlay.uid);
                }
                else if (updatePlay.goldenBeans < 0 || updatePlay.redPacket < 0 || updatePlay.goldenCoin < 0 || updatePlay.freeRedPacket < 0)
                {
                    string resultMessage = "";
                    if (updatePlay.goldenBeans < 0)
                    {
                        resultMessage = "押了" + updatePlay.goldenBeans + "豆,";
                    }
                    if (updatePlay.redPacket < 0)
                    {
                        resultMessage = resultMessage + "押了" + updatePlay.redPacket + "小红包,";
                    }
                    if (updatePlay.freeRedPacket < 0)
                    {
                        resultMessage = resultMessage + "押了" + updatePlay.freeRedPacket + "免费红包,";
                    }
                    if (updatePlay.goldenCoin < 0)
                    {
                        resultMessage = resultMessage + "押了" + updatePlay.goldenCoin + "金币,";
                    }
                    gameWinRecord.resultMessage = resultMessage;
                    game_RecordDAL.saveLoseRecord(gameWinRecord, updatePlay.type, year);
                    //统计玩的次数
                    UpateGamePlayTimes(updatePlay.uid, updatePlay.type, updatePlay.freeRedPacket, updatePlay.redPacket);
                }
                resultData.status = "0";
                resultData.message = "更新数据成功";
            }
            else
            {
                resultData.status = "1";
                resultData.message = "个人账户金豆或红包不足";
            }

            // lcl 2018-07-13 Insert
            if (!string.IsNullOrWhiteSpace(updatePlay.playId))
            {
                resultData.redpackageData = GetAutoRedpackageData(updatePlay.uid);
            }

            return ToJsonUtil.toJson(resultData);
        }
        //支付门票，锁定个人账户
        [HttpPost]
        [ActionName("pay_ticket")]
        public HttpResponseMessage pay_ticket([FromBody] UpdatePlay updatePlay)
        {
            ResultData resultData = new ResultData();

            if (updatePlay.type == null && updatePlay.type.Length == 0)
            {
                resultData.status = "1";
                resultData.message = "游戏类型必传";
                return ToJsonUtil.toJson(resultData);
            }
            if (updatePlay.timestamp <= 0)
            {
                resultData.status = "1";
                resultData.message = "时间戳必传";
                return ToJsonUtil.toJson(resultData);
            }
            DateTime dateTime = GetDateTimeUtil.GetDateTime(updatePlay.timestamp);
            string year = dateTime.Year.ToString();
            string type = updatePlay.type;
            string playId = Guid.NewGuid().ToString();

            foreach (UpdateUser item in updatePlay.listUser)
            {
                int rs = game_RecordDAL._updateUserGoldenBeans(item.uid, item.goldenBeans, item.redPacket, item.freeRedPacket, 1);
                if (rs > 0)
                {   //记录锁定交易 游戏类型和时间
                    game_RecordDAL.updateUser(item.uid, updatePlay.type, DateTime.Now);
                    GameWinRecord gameWinRecord = new GameWinRecord();
                    gameWinRecord.playId = playId;
                    gameWinRecord.uid = item.uid;
                    gameWinRecord.goldenBeans = item.goldenBeans;
                    gameWinRecord.redPacket = item.redPacket;
                    gameWinRecord.resultType = item.resultType;
                    gameWinRecord.timeStamp = dateTime;
                    gameWinRecord.goldenCoin = item.goldenCoin;
                    gameWinRecord.freeRedPacket = item.freeRedPacket;
                    if (item.goldenBeans < 0 || item.redPacket < 0 || item.goldenCoin < 0 || item.freeRedPacket < 0)
                    {
                        string resultMessage = "";
                        if (item.goldenBeans < 0)
                        {
                            resultMessage = "门票扣了" + item.goldenBeans + "豆,";
                        }
                        if (item.redPacket < 0)
                        {
                            resultMessage = resultMessage + "门票扣了" + item.redPacket + "小红包,";
                        }
                        if (item.freeRedPacket < 0)
                        {
                            resultMessage = resultMessage + "门票扣了" + item.freeRedPacket + "免费红包,";
                        }
                        if (item.goldenCoin < 0)
                        {
                            resultMessage = resultMessage + "门票扣了" + item.goldenCoin + "金币,";
                        }
                        gameWinRecord.resultMessage = resultMessage;
                        game_RecordDAL.saveLoseRecord(gameWinRecord, updatePlay.type, year);
                        //统计玩的次数
                        UpateGamePlayTimes(item.uid, updatePlay.type, item.freeRedPacket, item.redPacket);

                        resultData.data = new { playId = playId };
                    }
                    resultData.status = "0";
                    resultData.message = "更新数据成功";
                }
                else
                {
                    resultData.status = "1";
                    resultData.message = "账户被冻结或余额不足";
                }
            }
            return ToJsonUtil.toJson(resultData);
        }

        private void UpateGamePlayTimes(int uid, string gameType, decimal freeRedPacket, decimal redPacket)
        {
            if (freeRedPacket < 0)
            {
                game_RecordDAL.upate_game_play_times(uid, gameType, "freeRedPacket");
            }
            if (redPacket < 0)
            {
                game_RecordDAL.upate_game_play_times(uid, gameType, "redPacket");
            }
        }

        /// <summary>
        /// 强行解锁
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("force_unlock")]
        public HttpResponseMessage force_unlock(int uid)
        {
            ResultData resultData = new ResultData();
            int isOk = game_RecordDAL.force_unlock(uid);
            resultData.status = (isOk > 0) ? "0" : "1";
            resultData.message = "执行完成!";
            return ToJsonUtil.toJson(resultData);

        }

        /// <summary>
        /// 强行加锁
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("force_lock")]
        public HttpResponseMessage force_lock(int uid, string type)
        {
            ResultData resultData = new ResultData();
            int isOk = game_RecordDAL.lockUser(uid, type, DateTime.Now);
            resultData.status = (isOk > 0) ? "0" : "1";
            resultData.message = "执行完成!";
            return ToJsonUtil.toJson(resultData);

        }



        [HttpPost]
        [ActionName("update_play_clear")]
        public HttpResponseMessage update_play_clear([FromBody] UpdatePlay updatePlay)
        {
            ResultData resultData = new ResultData();
            if (string.IsNullOrWhiteSpace(updatePlay.type))
            {
                resultData.status = "1";
                resultData.message = "游戏类型必传";
                return ToJsonUtil.toJson(resultData);
            }
            if (updatePlay.timestamp <= 0)
            {
                resultData.status = "1";
                resultData.message = "时间戳必传";
                return ToJsonUtil.toJson(resultData);
            }
            string playId = updatePlay.playId;
            if (string.IsNullOrWhiteSpace(playId))
            {
                resultData.status = "1";
                resultData.message = "playId不能为null或不能空值字符串";
                return ToJsonUtil.toJson(resultData);
            }


            DateTime dateTime = GetDateTimeUtil.GetDateTime(updatePlay.timestamp);
            string year = dateTime.Year.ToString();
            string type = updatePlay.type;
            int count = game_RecordDAL.getPlayCount(playId, type, year);
            {
                if (count < 1)
                {
                    resultData.status = "1";
                    resultData.message = "当前局游戏不存在";
                    return ToJsonUtil.toJson(resultData);
                }
            }
            foreach (UpdateUser item in updatePlay.listUser)
            {
                GameWinRecord gameRecord = new GameWinRecord();
                gameRecord.playId = playId;
                gameRecord.uid = item.uid;
                gameRecord.goldenBeans = item.goldenBeans;
                gameRecord.redPacket = item.redPacket;
                gameRecord.resultType = item.resultType;
                gameRecord.timeStamp = dateTime;
                gameRecord.goldenCoin = item.goldenCoin;
                gameRecord.freeRedPacket = item.freeRedPacket;
                //输
                if (item.goldenBeans < 0 || item.redPacket < 0 || item.goldenCoin < 0 || item.freeRedPacket < 0)
                {
                    int rs = game_RecordDAL._updateUserGoldenBeans(item.uid, item.goldenBeans, item.redPacket, item.freeRedPacket, 2);
                    if (rs > 0)
                    {
                        //解锁交易、时间
                        game_RecordDAL.updateUser(item.uid, null, DateTime.Now);
                        string resultMessage = "";
                        if (item.goldenBeans < 0)
                        {
                            resultMessage = "输了" + item.goldenBeans + "豆,";
                        }
                        if (item.redPacket < 0)
                        {
                            resultMessage = resultMessage + "输了" + item.redPacket + "小红包,";
                        }
                        if (item.freeRedPacket < 0)
                        {
                            resultMessage = resultMessage + "输了" + item.freeRedPacket + "免费红包,";
                        }
                        if (item.goldenCoin < 0)
                        {
                            resultMessage = resultMessage + "输了" + item.goldenCoin + "金币,";
                        }
                        gameRecord.resultMessage = resultMessage;
                        game_RecordDAL.saveLoseRecord(gameRecord, updatePlay.type, year);
                        resultData.status = "0";
                        resultData.message = "更新数据成功";
                    }
                    else
                    {
                        resultData.status = "1";
                        resultData.message = "个人账户金豆或红包不足";
                    }
                }//赢
                else if (item.goldenBeans > 0 || item.redPacket > 0 || item.goldenCoin > 0 || item.freeRedPacket > 0)
                {

                    //判断数据里面是否添加当前这局赢的游戏记录
                    string pid = game_RecordDAL._getWinPlayId(updatePlay.playId, updatePlay.type, year, item.uid);
                    if (string.IsNullOrWhiteSpace(pid))
                    {
                        int rs = game_RecordDAL._updateUserGoldenBeans(item.uid, item.goldenBeans, item.redPacket, item.freeRedPacket, 2);
                        if (rs > 0)
                        {
                            //解锁交易、时间
                            game_RecordDAL.updateUser(item.uid, null, DateTime.Now);
                            string resultMessage = "";
                            if (item.goldenBeans > 0)
                            {
                                resultMessage = "赢了" + item.goldenBeans + "豆,";
                            }
                            if (item.redPacket > 0)
                            {
                                resultMessage = resultMessage + "赢了" + item.redPacket + "小红包,";
                            }
                            if (item.freeRedPacket > 0)
                            {
                                //resultMessage = resultMessage + "赢了" + item.freeRedPacket + "免费红包,";

                                // lcl 2018-07-13 Modify 免费红包直接转为了金豆，因此提示为赢了豆
                                resultMessage = resultMessage + "赢了" + item.freeRedPacket + "豆,";
                            }
                            if (item.goldenCoin > 0)
                            {
                                resultMessage = resultMessage + "赢了" + item.goldenCoin + "金币,";
                            }
                            gameRecord.resultMessage = resultMessage;
                            game_RecordDAL.saveWinRecord(gameRecord, updatePlay.type, year);
                            resultData.status = "0";
                            resultData.message = "更新数据成功";
                        }
                        else
                        {
                            resultData.status = "1";
                            resultData.message = "更新数据还失败";
                        }
                    }
                    else
                    {
                        resultData.status = "1";
                        resultData.message = "更新数据还失败";
                    }
                }

                // 根据账户信息清除领红包的次数记录
                //ClearRedpackageRecord(item.uid);

                // 注：多用户一起游戏时，自动发红包的接口调用放在游戏中，针对每个用户调用一次。接口参看本页面的auto_redpackage_daily方法
            }
            return ToJsonUtil.toJson(resultData);
        }
        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }
        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }

        /// <summary>
        /// 疯狂转转赚
        /// </summary>
        /// <param name="updatePlay"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("update_result_after_play")]
        public HttpResponseMessage update_result_after_play([FromBody] UpdatePlayAfter updatePlay)
        {
            ResultData resultData = new ResultData();
            updatePlay.uid = UserTokenUtils.getIdByTokenString(updatePlay.token);
            if (updatePlay.uid == -1)
            {
                resultData.status = "1";
                resultData.message = "token解析失败";
                return ToJsonUtil.toJson(resultData);
            }

            if (updatePlay.type == null && updatePlay.type.Length == 0)
            {
                resultData.status = "1";
                resultData.message = "游戏类型必传";
                return ToJsonUtil.toJson(resultData);
            }
            if (updatePlay.timestamp <= 0)
            {
                resultData.status = "1";
                resultData.message = "时间戳必传";
                return ToJsonUtil.toJson(resultData);
            }
            DateTime dateTime = GetDateTimeUtil.GetDateTime(updatePlay.timestamp);
            string year = dateTime.Year.ToString();
            if (updatePlay.playId != null && updatePlay.playId.Length > 0)
            {
                int count = game_RecordDAL.getPlayCount(updatePlay.playId, updatePlay.type, year);
                {
                    if (count != 1)
                    {
                        resultData.status = "1";
                        resultData.message = "当前局游戏不存在";
                        return ToJsonUtil.toJson(resultData);
                    }
                }
            }

            GameWinRecord gameRecord = new GameWinRecord();
            gameRecord.uid = updatePlay.uid;
            gameRecord.goldenBeans = updatePlay.goldenBeans;
            gameRecord.redPacket = updatePlay.redPacket;
            gameRecord.resultType = updatePlay.resultType;
            gameRecord.timeStamp = dateTime;
            gameRecord.goldenCoin = updatePlay.goldenCoin;
            gameRecord.freeRedPacket = updatePlay.freeRedPacket;
            string playId = updatePlay.playId;
            //押注
            if (string.IsNullOrWhiteSpace(playId)/*updatePlay.goldenBeans < 0 || updatePlay.redPacket < 0 || updatePlay.goldenCoin < 0 || updatePlay.freeRedPacket < 0*/)
            {
                int rs = game_RecordDAL.updateUserGoldenBeans(updatePlay.uid, updatePlay.goldenBeans, updatePlay.redPacket, updatePlay.freeRedPacket);
                if (rs > 0)
                {
                    string resultMessage = "";
                    if (updatePlay.goldenBeans < 0)
                    {
                        resultMessage = "押了" + updatePlay.goldenBeans + "豆,";
                    }
                    if (updatePlay.redPacket < 0)
                    {
                        resultMessage = resultMessage + "押了" + updatePlay.redPacket + "小红包,";
                    }
                    if (updatePlay.freeRedPacket < 0)
                    {
                        resultMessage = resultMessage + "押了" + updatePlay.freeRedPacket + "红包,";
                    }
                    if (updatePlay.goldenCoin < 0)
                    {
                        resultMessage = resultMessage + "押了" + updatePlay.goldenCoin + "金币,";
                    }
                    gameRecord.playId = Guid.NewGuid().ToString();
                    gameRecord.resultMessage = resultMessage;
                    game_RecordDAL.saveLoseRecord(gameRecord, updatePlay.type, year);
                    //统计玩的次数
                    UpateGamePlayTimes(updatePlay.uid, updatePlay.type, updatePlay.freeRedPacket, updatePlay.redPacket);
                    resultData.status = "0";
                    resultData.message = "更新数据成功";
                    resultData.data = new { playId = gameRecord.playId, winLoseResult = updatePlay, userinfo = getUserInfo(updatePlay.uid) };
                }
                else
                {
                    resultData.status = "1";
                    resultData.message = "个人账户金豆或红包不足或帐号锁定";
                }
            }//赢
            else // if (updatePlay.goldenBeans > 0 || updatePlay.redPacket > 0 || updatePlay.goldenCoin > 0 || updatePlay.freeRedPacket > 0)
            {

                //string playId = updatePlay.playId;
                if (playId == null)
                {
                    resultData.status = "1";
                    resultData.message = "playId不能为null";
                    return ToJsonUtil.toJson(resultData);
                }
                if (playId.Length < 1)
                {
                    resultData.status = "1";
                    resultData.message = "playId不能空值字符串";
                    return ToJsonUtil.toJson(resultData);
                }
                //判断数据里面是否添加当前这局赢的游戏记录
                //string pid = game_RecordDAL.getWinPlayId(updatePlay.playId, updatePlay.type, year);
                var t_Record = game_RecordDAL.getWinAllByPlayId(updatePlay.playId, updatePlay.type, year);
                if (!string.IsNullOrWhiteSpace(t_Record.play_id))
                {
                    decimal total = t_Record.freeRedPacket + t_Record.goldenBeans;
                    GameMoney realAccount = game_RecordDAL.updateUserGoldenBeansV2(updatePlay.uid, updatePlay.goldenBeans, updatePlay.redPacket, updatePlay.freeRedPacket, total);
                    if (realAccount != null)
                    {
                        StringBuilder sbResultMessage = new StringBuilder();
                        if (realAccount.goldenBeans != 0m)
                        {
                            sbResultMessage.Append($@"{Convert.ToInt32(Math.Abs(realAccount.goldenBeans))}豆,");
                        }
                        if (realAccount.redPacket != 0m)
                        {
                            sbResultMessage.Append($@"{Convert.ToInt32(Math.Abs(realAccount.redPacket))}小红包,");
                        }
                        if (realAccount.freeRedPacket != 0m)
                        {
                            sbResultMessage.Append($@"{Convert.ToInt32(Math.Abs(realAccount.freeRedPacket))}红包,");
                        }
                        if (realAccount.goldenCoin != 0m)
                        {
                            sbResultMessage.Append($@"{Convert.ToInt32(Math.Abs(realAccount.goldenCoin))}金币,");
                        }

                        decimal mMoney = realAccount.goldenBeans + realAccount.redPacket + realAccount.goldenCoin + realAccount.freeRedPacket;
                        if (mMoney > 0m)
                        {
                            sbResultMessage.Insert(0, "赢了");
                        }
                        else if (mMoney < 0m)
                        {
                            sbResultMessage.Insert(0, "输了");
                        }
                        else
                        {
                            sbResultMessage.Append("无金豆和红包可扣,");
                        }

                        gameRecord.goldenBeans = realAccount.goldenBeans;
                        gameRecord.redPacket = realAccount.redPacket;
                        gameRecord.goldenCoin = realAccount.goldenCoin;
                        gameRecord.freeRedPacket = realAccount.freeRedPacket;
                        gameRecord.resultMessage = sbResultMessage.Length > 0 ? sbResultMessage.ToString(0, sbResultMessage.Length - 1) : string.Empty;
                        gameRecord.playId = updatePlay.playId;

                        // 写输或者赢的记录
                        game_RecordDAL.saveWinRecord(gameRecord, updatePlay.type, year);

                        resultData.status = "0";
                        resultData.message = "更新数据成功";
                        // lcl 2018-07-13 Insert
                        resultData.redpackageData = GetAutoRedpackageData(updatePlay.uid);
                        resultData.data = new { playId = updatePlay.playId, winLoseResult = realAccount, userinfo = getUserInfo(updatePlay.uid) };
                    }
                    else
                    {
                        resultData.status = "1";
                        resultData.message = "更新数据还失败";
                    }
                }
                else
                {
                    resultData.status = "1";
                    resultData.message = "更新数据还失败";
                }

                // 根据账户信息清除领红包的次数记录
                ClearRedpackageRecord(updatePlay.uid);
            }
            return ToJsonUtil.toJson(resultData);
        }

        // lcl 2018-07-13 Insert
        [HttpGet]
        [ActionName("auto_redpackage_daily")]
        public HttpResponseMessage auto_redpackage_daily([FromUri] string token)
        {
            ResultData resultData = new ResultData();
            int uid = UserTokenUtils.getIdByTokenString(token);
            if (uid == -1)
            {
                resultData.status = "1";
                resultData.message = "token解析失败";
                return ToJsonUtil.toJson(resultData);
            }

            resultData.status = "0";
            resultData.redpackageData = GetAutoRedpackageData(uid);
            return ToJsonUtil.toJson(resultData);
        }

        private void ClearRedpackageRecord(int uid)
        {
            ClearRedpackageTimes(uid);
            ClearGameTimes(uid);
        }

        /// <summary>
        /// 清除领取的红包次数
        /// </summary>
        /// <param name="uid"></param>
        private void ClearRedpackageTimes(int uid)
        {
            // rrl_random_redpackage_times表ID字段值结构：用户ID + 8位年月日 + 1位资金类型（1：红包；2：V红包）+ spreader_queue表对应的ID（用于区分时段）
            // 构造领取记录表中ID前缀
            string strIdPrefix = $@"{uid}{DateTime.Now.ToString("yyyyMMdd")}";
            string strSql = $@"delete from rrl_random_redpackage_times
                                where ((id like '{strIdPrefix}1%') and exists (select id from rrl_user(nolock)
                                                                                where (id = {uid}) and (h_money_free = 0)))
                                   or ((id like '{strIdPrefix}2%') and exists (select id from rrl_user(nolock)
                                                                                where (id = {uid}) and (h_money_pay = 0)))";
            new SqlDataBase().Execute(strSql, null);
        }

        /// <summary>
        /// 清除用户当天玩游戏的总局数
        /// </summary>
        /// <param name="uid"></param>
        private void ClearGameTimes(int uid)
        {
            SqlDataBase sqlDB = new SqlDataBase();

            string strSql = $@"update game_user_play_times
                                  set play_times = 0
                                where (uid = {uid})
                                  and (((money_type = 'freeRedPacket') and exists (select id from rrl_user(nolock)
                                                                                    where (id = {uid}) and (h_money_free = 0)))
                                      or 
                                      ((money_type = 'redPacket') and exists (select id from rrl_user(nolock)
                                                                               where (id = {uid}) and (h_money_pay = 0))))";
            sqlDB.Execute(strSql, null);

        }

        // lcl 2018-07-13 Insert
        private dynamic GetAutoRedpackageData(int uid)
        {
            // 执行领取自动发红包的存储过程
            List<dynamic> resultWithSP = new SqlDataBase().ExecuteStoredProcedure<dynamic>(@"sp_V3_GameAutoRedpackageDaily",
                new
                {
                    uid = uid
                });

            if (resultWithSP == null || resultWithSP.Count < 1)
            {
                // 存储过程执行失败
                return new { isGotRedpackage = 0, hasGotTimes = 0, isToTask = 0, moneyFree = 0 };
            }

            // 存储过程执行成功
            return resultWithSP.First();
        }
        //[HttpPost]
        //[ActionName("apply_ticket")]
        //public ResultData apply_ticket([FromBody] ApplyTicket obj)
        //{
        //    SqlDataBase sqlDB = new SqlDataBase();
        //    string sql = $@"delete from game_ticket where uid={obj.uid}";
        //    sqlDB.Execute(sql,null);
        //}
        /// <summary>
        /// 结算游戏
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("settle_accounts")]
        public ResultData settle_accounts([FromBody] SettleAccounts obj)
        {
            SqlDataBase db = new SqlDataBase();
            string sql = @"UPDATE rrl_user SET 
                            h_money = h_money + @goldenBeans ,h_money_free = h_money_free + @freeRedPacket,h_money_pay = h_money_pay + @redPacket 
                            WHERE id =@id and h_money+@goldenBeans>=0 and h_money_free+@freeRedPacket >=0 and isnull(h_money_pay,0) + @redPacket >= 0 and 
                            isnull(is_locked_login,'0')='0'";
            int rs = db.Execute(sql, new { id = obj.uid, goldenBeans = obj.goldenBeans, redPacket = obj.redPacket, freeRedPacket = obj.freeRedPacket });
            ResultData data_res = new ResultData();
            if (rs <= 0)
            {

                data_res.status = "1";
                data_res.message = "更新账户失败";
                return data_res;
            }
            else
            {
                if(obj.game_record!=null)
                {
                    string detail = JsonConvert.SerializeObject(obj.game_record.detail);
                    int res = db.Execute(@"insert into game_record(detail,total_bean,total_v_money,total_free,result,result_odds,win,income,uid,start_time,end_time,game_type,game_id) 
values(@detail,@total_bean,@total_v_money,@total_free,@result,@result_odds,@win,@income,@uid,@start_time,@end_time,@game_type,@game_id)",
new { detail=detail, total_bean =obj.game_record.total_bean, total_v_money =obj.game_record.total_v_money, total_free =obj.game_record.total_free,
result=obj.game_record.result,result_odds=obj.game_record.result_odds,win=obj.game_record.win,income=obj.game_record.income,uid=obj.game_record.uid,
start_time=obj.game_record.start_time,end_time=obj.game_record.end_time,game_type=obj.game_record.game_type,game_id=obj.game_record.game_id});
                }
                if(obj.valType==-1) //在这里更新用户每日玩游戏次数，表：game_user_daily_count
                {
                    string today = DateTime.Now.ToString("yyyy-MM-dd");
                    int res = db.Execute("update game_user_daily_count set count=@count where date=@today and uid=@uid", new { uid = obj.uid, today = today,count=obj.count });
                    if (res == 0)
                    {
                        res = db.Execute("insert into game_user_daily_count(uid,date,count) values(@uid,@date,@count)", new { uid = obj.uid, date = today, count = obj.count });
                        if (res <= 0)
                        {
                            data_res = new ResultData();
                            data_res.status = "1";
                            data_res.message = "结算成功，更新游戏局数失败";
                            return data_res;
                        }
                        else
                        {
                            data_res = new ResultData();
                            data_res.status = "0";
                            data_res.message = "成功";
                            return data_res;
                        }
                    }
                    else if (res > 0)
                    {
                        data_res = new ResultData();
                        data_res.status = "0";
                        data_res.message = "成功";
                        return data_res;
                    }
                    else
                    {
                        data_res = new ResultData();
                        data_res.status = "1";
                        data_res.message = "结算成功，更新游戏局数失败";
                        return data_res;
                    }
                }
                data_res = new ResultData();
                data_res.status = "0";
                data_res.message = "成功";
                return data_res;

            }

        }
        /// <summary>
        /// 获取用户当天游戏局数
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("get_user_play_count_today")]
        public ResultData get_user_play_count_today([FromBody] SettleAccounts obj)
        {
            SqlDataBase db = new SqlDataBase();
            string today = DateTime.Now.ToString("yyyy-MM-dd");
            List<SettleAccounts> l = db.Select<SettleAccounts>("select count from game_user_daily_count where uid=@uid and date=@today and active=1",
                new { uid = obj.uid, today = today });

            ResultData data_res = new ResultData();
            data_res.status = "0";
            data_res.message = "成功";

            if (l == null)
            {
                data_res.status = "1";
                data_res.message = "执行语句失败";
                data_res.data = -1;
            }
            if (l.Count == 0)
            {
                data_res.data = 0;
            }
            else
            {
                data_res.data = l[0].count;
            }
            return data_res;
        }
        /// <summary>
        /// 获取用户V红包兑换金豆所需游戏局数
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("get_user_need_play_count")]
        public ResultData get_user_need_play_count([FromBody] SettleAccounts obj)
        {
            SqlDataBase db = new SqlDataBase();

            List<SettleAccounts> l = db.Select<SettleAccounts>("select need_play_conut  from rrl_user where id=@uid",
                new { uid = obj.uid });

            ResultData data_res = new ResultData();
            data_res.status = "0";
            data_res.message = "成功";
            if (l == null)
            {
                data_res.status = "1";
                data_res.message = "执行语句失败";
                data_res.data = -1;
                return data_res;
            }
            if (l.Count == 0)
            {
                data_res.status = "1";
                data_res.message = "用户不存在";
                data_res.data = -1;
                return data_res;
            }
            //if (l[0].redPacket == 0)
            //{
            //    data_res.status = "2";
            //    data_res.message = "用户V红包数量为0";
            //    data_res.data = -1;
            //    return data_res;
            //}
            else
            {
                int need_play_conut = Convert.ToInt32(l[0].need_play_conut);
                data_res.data = need_play_conut;
            }
            return data_res;
        }
        /// <summary>
        /// 每天清零时间
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("get_clear_time")]
        public ResultData get_clear_time()
        {
            ResultData data_res = new ResultData();
            data_res.status = "0";
            data_res.message = "成功";
            data_res.data = "23:30";
            return data_res;
        }
        [HttpPost]
        [ActionName("v_money_to_bean")]
        public ResultData v_money_to_bean([FromBody] SettleAccounts obj)
        {
            ResultData data_res = new ResultData();
            data_res.status = "0";
            data_res.message = "成功";

            SqlDataBase db = new SqlDataBase();
            string today = DateTime.Now.ToString("yyyy-MM-dd");
            List<SettleAccounts> l = db.Select<SettleAccounts>("select count from game_user_daily_count where uid=@uid and date=@today and active=1",
                new { uid = obj.uid, today = today });
            if (l == null)
            {
                data_res.status = "1";
                data_res.message = "执行语句失败";
                data_res.data = -1;
                return data_res;
            }
            int PlayToday = 0;//今天游戏局数
            if (l.Count > 0) PlayToday = l[0].count;
            int NeedPlayCount = 99999;//需要的局数
            double h_money_pay = 0;
            l = db.Select<SettleAccounts>("select h_money_pay as redPacket,need_play_conut from rrl_user where id=@uid",
                new { uid = obj.uid });

            if (l == null)
            {
                data_res.status = "1";
                data_res.message = "执行语句失败";
                return data_res;
            }
            if (l.Count == 0)
            {
                data_res.status = "1";
                data_res.message = "用户不存在";
                return data_res;
            }
            if (l[0].redPacket == 0)
            {
                data_res.status = "2";
                data_res.message = "用户V红包数量为0";
                return data_res;
            }
            else
            {
                h_money_pay = Convert.ToDouble(l[0].redPacket);
                NeedPlayCount = l[0].need_play_conut;//所需局数转
            }
            if(PlayToday< NeedPlayCount)
            {
                data_res.status = "1";
                data_res.message = $@"用户今天游戏局数{PlayToday}小于兑换需要的局数{NeedPlayCount}";
                return data_res;
            }
            int res = db.Execute("update rrl_user set h_money_pay=0,h_money=h_money+@h_money_pay,need_play_conut=20 where id=@uid",
                new { uid = obj.uid, h_money_pay = h_money_pay });
            if(res<=0)
            {
                data_res.status = "1";
                data_res.message = "执行更新账户失败";
                return data_res;
            }
            else
            {
                db.Execute("update game_user_daily_count set count=0 where uid=@uid and date=@today", new { uid = obj.uid, today = today });
                data_res.status = "0";
                data_res.data = h_money_pay;
                data_res.message = "成功";
                return data_res;
            }
            
        }
        [HttpPost]
        [ActionName("clear_v_money")]
        public ResultData clear_v_money()
        {
            ResultData data_res = new ResultData();
            data_res.status = "0";
            data_res.message = "成功";


            SqlDataBase db = new SqlDataBase();
            string today = DateTime.Now.ToString("yyyy-MM-dd");
            
            List<SettleAccounts> users = db.Select<SettleAccounts>("select id as uid,h_money_pay as redPacket,need_play_conut from rrl_user where h_money_pay>0", null);
            //List<ClearUser> list = new List<ClearUser>();
            int length = users.Count;
            //int count = 0;
            for (int i = 0; i < length; i++)
            {
                SettleAccounts user = users[i];

                //ClearUser u = new ClearUser();

                //u.uid = user.uid;
                //u.to_bean = 0;
                //u.to_clear = 0;

                double h_money_pay = Convert.ToDouble(user.redPacket);
                int NeedPlayCount = user.need_play_conut;//所需局数转换条件为V红包金额/1.5
                List<SettleAccounts> l = db.Select<SettleAccounts>("select count from game_user_daily_count where uid=@uid and date=@today and active=1",
                new { uid = user.uid, today = today });
                int PlayToday = 0;//今天游戏局数
                if (l.Count > 0) PlayToday = l[0].count;
                int res = 0;
                //u.PlayToday = PlayToday;
                //u.NeedPlayCount = NeedPlayCount;
                if (PlayToday>= NeedPlayCount)
                {
                    res = db.Execute("update rrl_user set h_money_pay=0,h_money=h_money+@h_money_pay,need_play_conut=20 where id=@uid",
                    new { uid = user.uid, h_money_pay = h_money_pay });
                    //u.to_bean = h_money_pay;
                    //u.to_clear = h_money_pay;
                }
                else
                {
                    res = db.Execute("update rrl_user set h_money_pay=0,need_play_conut=20  where id=@uid",
                    new { uid = user.uid});
                    //u.to_bean = 0;
                    //u.to_clear = h_money_pay;
                }
                if (res > 0)
                {
                    db.Execute("update game_user_daily_count set active=0 where date=@today", new { today = today });
                    //list.Add(u);
                }
               
            }
            data_res.data = length;
            return data_res;

        }
    }


}