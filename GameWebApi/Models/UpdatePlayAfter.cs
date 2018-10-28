using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameWebApi.Models
{
    public class UpdatePlayAfter: UpdatePlay
    {
        public string token { get; set; }

    }
}