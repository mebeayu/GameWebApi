using System;
using System.Net.Http;
using System.Text;
using System.Web.Script.Serialization;

namespace GameWebApi.util
{
    public class ToJsonUtil
    {
        public static HttpResponseMessage toJson(Object obj)
        {
            String str;
            if (obj is String || obj is Char)
            {
                str = obj.ToString();
            }
            else
            {
                //JavaScriptSerializer serializer = new JavaScriptSerializer();
                  str=Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                //str = serializer.Serialize(obj);
            }
            HttpResponseMessage result = new HttpResponseMessage { Content = new StringContent(str, Encoding.GetEncoding("UTF-8"), "application/json") };
            return result;
        }
    }
}