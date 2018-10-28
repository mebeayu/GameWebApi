using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameWebApi.Models
{
    public class UpdatePlay
    {
        /// <summary>
        /// 游戏类型
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public int uid { get; set; }
        /// <summary>
        /// 游戏每一局Guid
        /// </summary>
        public string playId { get; set; }
        /// <summary>
        /// 输(负数)赢豆数量
        /// </summary>
        public decimal goldenBeans { get; set; }
        /// <summary>
        /// 输(负数)赢小红包数量
        /// </summary>
        public decimal redPacket { get; set; }
        /// <summary>
        /// 输(负数)赢免费红包数量
        /// </summary>
        public decimal freeRedPacket { get; set; }
        /// <summary>
        /// 押或开奖结果
        /// </summary>
        public string resultType { get; set; }
        /// <summary>
        /// 金币数量
        /// </summary>
        public decimal goldenCoin { get; set; }
        /// <summary>
        /// 时间戳 1970-01-01 到现在的秒数  zetee
        /// </summary>
        public long timestamp { get; set; }

        public List<UpdateUser> listUser { get; set; }

    }
}