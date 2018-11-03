using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameWebApi.Models
{
    public class GameRecord
    {
        public List<GameDetail> detail { get; set; }
        public decimal total_bean { get; set; }
        public decimal total_v_money { get; set; }
        public decimal total_free { get; set; }
        public string result { get; set; }
        public decimal result_odds { get; set; }
        public decimal win { get; set; }
        public decimal income { get; set; }
        public int uid { get; set; }
        public string start_time { get; set; }
        public string end_time { get; set; }
        public string game_type { get; set; }
        public string game_id { get; set; }
    }
    public class GameDetail
    {
        public string name { get; set; }
        public decimal bean { get; set; }
        public decimal v_money { get; set; }
        public decimal free { get; set; }
    }
}
/*
 {
  "uid": 0,
  "goldenBeans": 0,
  "freeRedPacket": 0,
  "redPacket": 0,
  "count": 0,
  "valType": 0,
  "need_play_conut": 0,
  "game_record": {
    "detail": [
      {
        "name": "string",//押注名称
        "bean": 0,//押注金豆
        "v_money": 0,//押注V红包
        "free": 0//押注免费红包
      }
    ],
    "total_bean": 0,//累计金豆
    "total_v_money": 0,//累计V红包
    "total_free": 0,//累计免费红包
    "result": "string",//中奖号码
    "result_odds": 0,//对应赔率
    "win": 0,//赢多少
    "income": 0,//总输赢
    "uid": 0,//uid
    "start_time": "string",//开始时间
    "end_time": "string",//结束时间
    "game_type": "string",//游戏类型
    "game_id": "string"//平台ID
  }
}
     */
