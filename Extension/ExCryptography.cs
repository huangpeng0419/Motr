using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Motr
{
    public static class ExCryptography
    {
        /// <summary>
        /// 与ASP兼容的MD5加密算法
        /// </summary>
        /// <param name="s">加密明文</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public static String ToMD5(this String s, Encoding encoding)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] hashByte = md5.ComputeHash(encoding.GetBytes(s));
            StringBuilder sb = new StringBuilder();
            for (Int32 i = 0; i < hashByte.Length; i++)
            {
                sb.Append(hashByte[i].ToString("x").PadLeft(2, '0'));
            }
            return sb.ToString();
        }
        /// <summary>
        /// 与ASP兼容的MD5加密算法
        /// </summary>
        /// <param name="s">加密明文</param>
        /// <returns></returns>
        public static String ToMD5(this String s)
        {
            return s.ToMD5(Encoding.UTF8);
        }
    }
}
