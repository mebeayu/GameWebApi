using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameWebApi.Models
{
    public class ClearUser
    {
        public int uid { get; set; }
        public int NeedPlayCount { get; set; }
        public int PlayToday { get; set; }
        public double to_bean { get; set; }
        public double to_clear { get; set; }
    }
}