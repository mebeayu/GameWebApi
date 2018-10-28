using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace GameWebApi.Models
{
    public class ResultData 
    {
        /// <summary>
        /// 执行结果 0 成功 1失败
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// 后台返回执行结果消息
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 返回数据
        /// </summary>
        public Object data { get; set; }

        // lcl 2018-07-13 Insert
        /// <summary>
        /// 返回数据
        /// </summary>
        public Object redpackageData { get; set; }

    }
}