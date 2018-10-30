using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameWebApi.Models
{
    public class SettleAccounts
    {
        public int uid { get; set; }
        public decimal goldenBeans { get; set; }//h_money
        public decimal freeRedPacket { get; set; }//h_money_free
        public decimal redPacket { get; set; }//h_money_pay
        public int count { get; set; }
        public int valType { get; set; }
        public int need_play_conut { get; set; }
    }
}