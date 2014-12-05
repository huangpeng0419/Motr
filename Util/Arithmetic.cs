using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Motr.Util
{
    /// <summary>
    /// 算术类
    /// </summary>
    public sealed class Arithmetic
    {
        private Arithmetic() { }
        /// <summary>
        /// 阶加
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static decimal Dectorial(Int32 num)
        {
            return (num * (num - 1)) / 2;
        }
        /// <summary>
        /// 阶乘
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static Int32 Factorial(Int32 num)
        {
            return num <= 1 ? 1 : num * Factorial(num - 1);
        }
        /// <summary>
        /// 斐波那契数列和
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static decimal FibonacciSum(Int32 num)
        {
            decimal sum = 0;
            foreach (Int32 i in Fibonacci(num))
                sum += i;
            return sum;
        }
        /// <summary>
        /// 斐波那契数列
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static IEnumerable<Int32> Fibonacci(Int32 num)
        {
            Int32 x = 0;
            Int32 y = 1;
            for (Int32 i = 1; i <= num; i++, y = x + y, x = y - x)
                yield return y;
        }
        /// <summary>
        /// 斐波那契数列第几项的值
        /// </summary>
        /// <param name="num">第几项</param>
        /// <returns></returns>
        public static decimal FionacciMax(Int32 num)
        {
            if (num <= 2)
                return 1;
            else
                return FionacciMax(num - 1) + FionacciMax(num - 2);
        }

    }
}
