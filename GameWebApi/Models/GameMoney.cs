using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace GameWebApi.Models
{
    public class GameMoney
    { 
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
        /// 金币数量
        /// </summary>
        public decimal goldenCoin { get; set; }

        /// <summary>
        /// 强充id  
        /// </summary>
        public string  forcePay { get; set; }

        /// <summary>
        /// 需要冲的豆数量
        /// </summary>
        public decimal needPayBeans { get; set; }

        /// <summary>
        /// 额外送的豆的数量
        /// </summary>
        public decimal extraGiveBeans { get; set; }

    }
}