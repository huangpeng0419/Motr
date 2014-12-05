using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Motr.Util
{
    public sealed class DateMgr
    {
        private DateMgr() { }
        /// <summary>
        /// 星期数组
        /// </summary>
        public static String[] Week = new String[] { "日", "一", "二", "三", "四", "五", "六" };
        /// <summary>
        /// 根据输入日期获取星期
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static String GetWeekText(Object dateTime)
        {
            DateTime tmp;
            if (DateTime.TryParse(dateTime + "", out tmp))
                return String.Format("星期{0}", Week[(Int32)tmp.DayOfWeek]);
            return String.Empty;
        }
        /// <summary>
        /// 获取上日的起始日期
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public static void LastDay(out String start, out String end)
        {
            DateTime tmp = DateTime.Now.AddDays(-1);
            start = tmp.ToString("yyyy-MM-dd");
            end = tmp.ToString("yyyy-MM-dd");
        }
        /// <summary>
        /// 获取本日的起始日期
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public static void ThisDay(out String start, out String end)
        {
            DateTime tmp = DateTime.Now;
            start = tmp.ToString("yyyy-MM-dd");
            end = tmp.ToString("yyyy-MM-dd");
        }
        /// <summary>
        /// 获取下日的起始日期
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public static void NextDay(out String start, out String end)
        {
            DateTime tmp = DateTime.Now.AddDays(1);
            start = tmp.ToString("yyyy-MM-dd");
            end = tmp.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 获取上周的起始日期
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public static void LastWeek(out String start, out String end)
        {
            DateTime tmp = DateTime.Now;
            Int32 thisWeek = (Int32)tmp.DayOfWeek;
            start = tmp.AddDays(-(thisWeek + 7)).ToString("yyyy-MM-dd");
            end = tmp.AddDays(-(thisWeek + 1)).ToString("yyyy-MM-dd");
        }
        /// <summary>
        /// 获取本周的起始日期
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public static void ThisWeek(out String start, out String end)
        {
            DateTime tmp = DateTime.Now;
            Int32 thisWeek = (Int32)tmp.DayOfWeek;
            start = tmp.AddDays(-thisWeek).ToString("yyyy-MM-dd");
            end = tmp.AddDays(6 - thisWeek).ToString("yyyy-MM-dd");
        }
        /// <summary>
        /// 获取下周的起始日期
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public static void NextWeek(out String start, out String end)
        {
            DateTime tmp = DateTime.Now;
            Int32 thisWeek = (Int32)tmp.DayOfWeek;
            start = tmp.AddDays(7 - thisWeek).ToString("yyyy-MM-dd");
            end = tmp.AddDays(6 - thisWeek + 7).ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 获取上月份的起始日期
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public static void LastMonth(out String start, out String end)
        {
            DateTime tmp = DateTime.Now;
            start = tmp.AddMonths(-1).ToString("yyyy-MM-01");
            end = tmp.AddDays(-tmp.Day).ToString("yyyy-MM-dd");
        }
        /// <summary>
        /// 获取本月份的起始日期
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public static void ThisMonth(out String start, out String end)
        {
            DateTime tmp = DateTime.Now;
            start = tmp.ToString("yyyy-MM-01");
            end = tmp.AddMonths(1).AddDays(-tmp.Day).ToString("yyyy-MM-dd");
        }
        /// <summary>
        /// 获取下月份的起始日期
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public static void NextMonth(out String start, out String end)
        {
            DateTime tmp = DateTime.Now;
            start = tmp.AddMonths(1).ToString("yyyy-MM-01");
            end = tmp.AddMonths(2).AddDays(-tmp.Day).ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 获取上季度的起始日期
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public static void LastQuarter(out String start, out String end)
        {
            DateTime tmp = DateTime.Now;
            tmp = tmp.AddMonths(-3 - (tmp.Month - 1) % 3);
            start = tmp.ToString("yyyy-MM-01");
            end = tmp.AddMonths(3).AddDays(-tmp.Day).ToString("yyyy-MM-dd");
        }
        /// <summary>
        /// 获取本季度的起始日期
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public static void ThisQuarter(out String start, out String end)
        {
            DateTime tmp = DateTime.Now;
            tmp = tmp.AddMonths(-(tmp.Month - 1) % 3);
            start = tmp.ToString("yyyy-MM-01");
            end = tmp.AddMonths(3).AddDays(-tmp.Day).ToString("yyyy-MM-dd");
        }
        /// <summary>
        /// 获取下季度的起始日期
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public static void NextQuarter(out String start, out String end)
        {
            DateTime tmp = DateTime.Now;
            tmp = tmp.AddMonths(3 - (tmp.Month - 1) % 3);
            start = tmp.ToString("yyyy-MM-01");
            end = tmp.AddMonths(3).AddDays(-tmp.Day).ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 获取上年份的起始日期
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public static void LastYear(out String start, out String end)
        {
            DateTime tmp = new DateTime(DateTime.Now.Year - 1, 1, 1);
            start = tmp.ToString("yyyy-MM-dd");
            end = tmp.AddYears(1).AddDays(-1).ToString("yyyy-MM-dd");
        }
        /// <summary>
        /// 获取本年份的起始日期
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public static void ThisYear(out String start, out String end)
        {
            DateTime tmp = new DateTime(DateTime.Now.Year, 1, 1);
            start = tmp.ToString("yyyy-MM-dd");
            end = tmp.AddYears(1).AddDays(-1).ToString("yyyy-MM-dd");
        }
        /// <summary>
        /// 获取下年份的起始日期
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public static void NextYear(out String start, out String end)
        {
            DateTime tmp = new DateTime(DateTime.Now.Year + 1, 1, 1);
            start = tmp.ToString("yyyy-MM-dd");
            end = tmp.AddYears(1).AddDays(-1).ToString("yyyy-MM-dd");
        }
    }

}
