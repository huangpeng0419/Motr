using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Motr.Util
{
    public sealed class ValidatyMgr
    {
        public ValidatyMgr() { }
        /// <summary>
        /// 使用正则表达式验证内容
        /// </summary>
        /// <param name="rp">正则表达式模式</param>
        /// <param name="s">要验证的内容</param>
        /// <returns>true,匹配;false,不匹配</returns>
        public static Boolean IsValid(RegexPattern rp, String content)
        {
            String regexExpr = String.Empty;
            switch (rp)
            {
                case RegexPattern.EMAIL: regexExpr = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"; break;
                case RegexPattern.URL: regexExpr = "^http[s]?:\\/\\/([\\w-]+\\.)+[\\w-]+([\\w-./?%&=]*)?$"; break;
                case RegexPattern.INT: regexExpr = "^([+-]?)\\d+$"; break;
                case RegexPattern.INTPLUS: regexExpr = "^([+]?)\\d+$"; break;
                case RegexPattern.INTNEGATIVE: regexExpr = "^-\\d+$"; break;
                case RegexPattern.NUMBER: regexExpr = "^([+-]?)\\d*\\.?\\d+$"; break;
                case RegexPattern.NUMBERPLUS: regexExpr = "^([+]?)\\d*\\.?\\d+$"; break;
                case RegexPattern.NUMBERNEGATIVE: regexExpr = "^-\\d*\\.?\\d+$"; break;
                case RegexPattern.FLOAT: regexExpr = "^([+-]?)\\d*\\.\\d+$"; break;
                case RegexPattern.FLOATPLUS: regexExpr = "^([+]?)\\d*\\.\\d+$"; break;
                case RegexPattern.FLOATNEGATIVE: regexExpr = "^-\\d*\\.\\d+$"; break;
                case RegexPattern.COLOR: regexExpr = "^#[a-fA-F0-9]{6}"; break;
                case RegexPattern.CHINESE: regexExpr = "^[\\u4E00-\\u9FA5\\uF900-\\uFA2D]+$"; break;
                case RegexPattern.ASCII: regexExpr = "^[\\x00-\\xFF]+$"; break;
                case RegexPattern.ZIPCODE: regexExpr = "^\\d{6}$"; break;
                case RegexPattern.MOBILE: regexExpr = "^0{0,1}13[0-9]{9}$"; break;
                case RegexPattern.IP4: regexExpr = @"^\(([0-1]?\d{0,2})|(2[0-5]{0,2}))\.(([0-1]?\d{0,2})|(2[0-5]{0,2}))\.(([0-1]?\d{0,2})|(2[0-5]{0,2}))\.(([0-1]?\d{0,2})|(2[0-5]{0,2}))$"; break;
                case RegexPattern.NOEMPTY: regexExpr = "^[^ ]+$"; break;
                case RegexPattern.PICTURE: regexExpr = "(.*)\\.(jpg|bmp|gif|ico|pcx|jpeg|tif|png|raw|tga)$"; break;
                case RegexPattern.RAR: regexExpr = "(.*)\\.(rar|zip|7zip|tgz)$"; break;
                case RegexPattern.DATE: regexExpr = @"^\d{4}(\-|\/|\.)\d{1,2}\1\d{1,2}$"; break;
            }
            return IsValid(regexExpr, content);
        }
        public static Boolean IsValid(String pattern, String content)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(content, pattern);
        }
    }
    /// <summary>
    /// 正则表达式模式
    /// </summary>
    public enum RegexPattern : byte
    {
        /// <summary>
        /// 电子邮件 ^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$。
        /// </summary>
        EMAIL = 1,
        /// <summary>
        /// 网址 ^http[s]?:\/\/([\w-]+\.)+[\w-]+([\w-./?%＆=]*)?$。
        /// </summary>
        URL = 2,
        /// <summary>
        /// 整数 ^([+-]?)\d+$。
        /// </summary>
        INT = 3,
        /// <summary>
        /// 正整数 ^([+]?)\d+$。
        /// </summary>
        INTPLUS = 4,
        /// <summary>
        /// 负整数 ^-\d+$。
        /// </summary>
        INTNEGATIVE = 5,
        /// <summary>
        /// 数字 ^([+-]?)\d*\.?\d+$。
        /// </summary>
        NUMBER = 6,
        /// <summary>
        /// 正数 ^([+]?)\d*\.?\d+$。
        /// </summary>
        NUMBERPLUS = 7,
        /// <summary>
        /// 负数 ^-\d*\.?\d+$。
        /// </summary>
        NUMBERNEGATIVE = 8,
        /// <summary>
        /// 浮点数 ^([+-]?)\d*\.\d+$。
        /// </summary>
        FLOAT = 9,
        /// <summary>
        /// 正浮点数 ^([+]?)\d*\.\d+$。
        /// </summary>
        FLOATPLUS = 10,
        /// <summary>
        /// 负浮点数 ^-\d*\.\d+$。
        /// </summary>
        FLOATNEGATIVE = 11,
        /// <summary>
        /// 颜色 ^#[a-fA-F0-9]{6}。
        /// </summary>
        COLOR = 12,
        /// <summary>
        /// 仅中文 ^[\u4E00-\u9FA5\uF900-\uFA2D]+$。
        /// </summary>
        CHINESE = 13,
        /// <summary>
        /// 仅ACSII字符 ^[\x00-\xFF]+$。
        /// </summary>
        ASCII = 14,
        /// <summary>
        /// 邮编 ^\d{6}$。
        /// </summary>
        ZIPCODE = 15,
        /// <summary>
        /// 手机 ^0{0,1}13[0-9]{9}$。
        /// </summary>
        MOBILE = 16,
        /// <summary>
        /// ip地址 ^\(([0-1]?\d{0,2})|(2[0-5]{0,2}))\.(([0-1]?\d{0,2})|(2[0-5]{0,2}))\.(([0-1]?\d{0,2})|(2[0-5]{0,2}))\.(([0-1]?\d{0,2})|(2[0-5]{0,2}))$。
        /// </summary>
        IP4 = 17,
        /// <summary>
        /// 非空 ^[^ ]+$。
        /// </summary>
        NOEMPTY = 18,
        /// <summary>
        /// 图片 (.*)\\.(jpg|bmp|gif|ico|pcx|jpeg|tif|png|raw|tga)$。
        /// </summary>
        PICTURE = 19,
        /// <summary>
        /// 压缩文件 (.*)\\.(rar|zip|7zip|tgz)$。
        /// </summary>
        RAR = 20,
        /// <summary>
        /// 日期 @"^\d{4}(\-|\/|\.)\d{1,2}\1\d{1,2}$";//^[\d]{4}-[01]?[\d]-[0123]?[\d]$。
        /// </summary>
        DATE = 21
    }

}
