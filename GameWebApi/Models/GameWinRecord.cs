using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameWebApi.Models
{
    public class GameWinRecord
    {   
        //主键id
        public int id { get; set; }
        //用户id
        public int uid { get; set; }
        //金豆数量
        public decimal goldenBeans { get; set; }
        //小红包数量
        public decimal redPacket { get; set; }
        //免费红包数量
        public decimal freeRedPacket { get; set; }
        //操作时间
        public DateTime timeStamp { get; set; }
        //操作结果
        public string resultType { get; set; }
        //GUID
        public string playId {get; set; }
        //游戏结果
        public string resultMessage { get; set; }
        //金币数量
        public decimal goldenCoin { get; set; }
    }
}