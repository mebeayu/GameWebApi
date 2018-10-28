using RRL.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameWebApi.util
{
    public class UserTokenUtils
    {
        public static  int getIdByTokenString(string tokenString)
        {
            tokenString = DES.DecryptDES(tokenString);
             
            string[] tokenArray;
            try
            {
                tokenArray = tokenString.Split(',');
            }
            catch
            {
                return -1;
            }

            if (tokenArray.Length == 4)
            {
                
                int id = Convert.ToInt32(tokenArray[1]);
                return id;
            }
            else
            {
                return -1;
            }
             
        }
    }
}