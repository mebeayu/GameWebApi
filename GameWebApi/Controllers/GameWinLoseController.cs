using GameWebApi.DAL;
using GameWebApi.Models;
using GameWebApi.util;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using System.Linq;

namespace GameWebApi.Controllers
{
    public class GameWinLoseController : ApiController, IDisposable
    {
        Game_RecordDAL game_RecordDAL = new Game_RecordDAL();

        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // 获取用户游戏赢的数据
        [HttpGet]
        [ActionName("WinDataByUser")]
        public HttpResponseMessage WinDataByUser([FromUri] GameWinDataRequest request)
        {
            ResultData resultData = new ResultData();

            int intUid = UserTokenUtils.getIdByTokenString(request.token);
            if (intUid == -1)
            {
                resultData.status = "1";
                resultData.message = "token解析失败";
                return ToJsonUtil.toJson(resultData);
            }

            int intGameId = 0;
            if (string.IsNullOrWhiteSpace(request.gameId) || !int.TryParse(request.gameId, out intGameId))
            {
                resultData.status = "1";
                resultData.message = "游戏ID不正确";
                return ToJsonUtil.toJson(resultData);
            }

            resultData.status = "0";
            var dataList = game_RecordDAL.GetWinDataByUser(intUid, request.gameId, request.topNumber);
            if (dataList != null && dataList.Count > 0)
            {
                resultData.data = dataList.Select(p => new { time = p.time_stamp.ToString("yyyy-MM-dd HH:mm:ss"), remark = p.result_message.TrimEnd(',') });
            }

            return ToJsonUtil.toJson(resultData);
        }

        // 获取用户游戏赢的数据
        [HttpGet]
        [ActionName("AllUserWinData")]
        public HttpResponseMessage AllUserWinData([FromUri] GameWinDataRequest request)
        {
            ResultData resultData = new ResultData();

            int intGameId = 0;
            if (string.IsNullOrWhiteSpace(request.gameId) || !int.TryParse(request.gameId, out intGameId))
            {
                resultData.status = "1";
                resultData.message = "游戏ID不正确";
                return ToJsonUtil.toJson(resultData);
            }

            resultData.status = "0";
            var dataList = game_RecordDAL.GetAllUserWinData(request.gameId, request.topNumber);
            if (dataList != null && dataList.Count > 0)
            {
                resultData.data = dataList.Select(p => new { username = p.username.Remove(3, 4).Insert(3, "****"), remark = p.result_message.TrimEnd(',') });
            }

            return ToJsonUtil.toJson(resultData);
        }
    }
}