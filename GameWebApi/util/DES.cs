using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace RRL.Core.Utility
{
    public class DES
    {
        private static string Key = "@rrl!$%l";
        private static byte[] Keys = { 0x33, 0x18, 0xA0, 0xF6, 0x66, 0xED, 0xBC, 0xEF };

        public static string DecryptDES(string decryptString)
        {
            return DecryptDES(decryptString, Key);
        }

        ///
        /// DES解密字符串
        /// 待解密的字符串
        /// 解密密钥,要求为8位,和加密密钥相同
        /// 解密成功返回解密后的字符串，失败返源串
        public static string DecryptDES(string decryptString, string decryptKey)
        {
            string res = decryptString;
            if (string.IsNullOrWhiteSpace(res))
            {
                return "";
            }
            res = res.Replace("JIAHAO", "+");
            res = res.Replace("JIANHAO", "-");
            res = res.Replace("XIEGANG", "/");
            res = res.Replace("FANXIEGANG", "\\");
            res = res.Replace("DENGHAO", "=");
            decryptString = res;
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey);
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch
            {
                return decryptString;
            }
        }

        public static string EncryptDES(string encryptString)
        {
            return EncryptDES(encryptString, Key);
        }

        /// DES加密字符串
        /// 待加密的字符串
        /// 加密密钥,要求为8位
        /// 加密成功返回加密后的字符串，失败返回源串
        public static string EncryptDES(string encryptString, string encryptKey)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                string res = Convert.ToBase64String(mStream.ToArray());
                res = res.Replace("+", "JIAHAO");
                res = res.Replace("-", "JIANHAO");
                res = res.Replace("/", "XIEGANG");
                res = res.Replace("\\", "FANXIEGANG");
                res = res.Replace("=", "DENGHAO");
                return res;
            }
            catch
            {
                return encryptString;
            }
        }
    }
}