using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace GameWebApi.Models
{
    public class UserAccount
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 用户真实名字或者昵称
        /// </summary>
        public string real_name { get; set; }
        /// <summary>
        /// 登录账号
        /// </summary>
        public string username { get; set; }

        /// <summary>
        /// 头像id
        /// </summary>
        public string head_pic { get; set; }
        /// <summary>
        /// 用户金币
        /// </summary>
        public decimal plate_to_return_money { get; set; }
        /// <summary>
        /// 不参与分配的金币
        /// </summary>
        public decimal ex_plate_to_return_money { get; set; }
        /// <summary>
        /// 用户历史积分
        /// </summary>
        public int point { get; set; }
        /// <summary>
        /// 用户消费账户
        /// </summary>
        public decimal r_money { get; set; }
        /// <summary>
        /// 用户现金账户
        /// </summary>
        public decimal x_money { get; set; }
        /// <summary>
        /// 用户推荐账户
        /// </summary>
        public decimal y_money { get; set; }
        /// <summary>
        /// 用户推荐账户冻结
        /// </summary>
        public decimal y_money_frz { get; set; }
        /// <summary>
        /// 金豆账户
        /// </summary>
        public decimal h_money { get; set; }
        /// <summary>
        /// 免费红包
        /// </summary>
        public decimal h_money_free { get; set; }
        /// 小红包
        /// </summary>
        public decimal h_money_pay { get; set; }
        /// <summary>
        /// 用户分配权重
        /// </summary>
        public int plate_to_return_weight { get; set; }

        /// <summary>
        /// 交易总额
        /// </summary>
        public decimal total_trans { get; set; }
        /// <summary>
        /// 总返还额
        /// </summary>
        public decimal total_return { get; set; }
        /// <summary>
        /// 总权重
        /// </summary>
        public int total_weight { get; set; }
        /// <summary>
        /// 每权平均返还
        /// </summary>
        public decimal return_ave_weight { get; set; }
        /// <summary>
        /// 信息是否已阅读
        /// </summary>
        public bool if_info_has_read { get; set; }
        /// <summary>
        /// 账户登录是否冻结 0 未冻结 1 已冻结
        /// </summary>
        public string is_locked_login { get; set; }
        /// <summary>
        /// 账户交易是否被冻结 0 未冻结 1 已冻结
        /// </summary>
        public string is_locked_trade { get; set; }
        /// <summary>
        /// 使用那个游戏锁交易 0007 斗地主
        /// </summary>
        public string is_locked_trade_game_type { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime? addtime { get; set; }

        /// <summary>
        /// 是否是新用户 1=新用户,0=老用户
        /// </summary>
        public string is_news_user { get; set; }

    }
}