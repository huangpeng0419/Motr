using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Motr.Data.Builder
{
    public enum DbAction
    {
        /// <summary>
        /// 插入
        /// </summary>
        Insert,
        /// <summary>
        /// 更新
        /// </summary>
        Update,
        /// <summary>
        /// 删除
        /// </summary>
        Delete,
        /// <summary>
        /// 计数
        /// </summary>
        Count,
        /// <summary>
        /// 默认(查询)
        /// </summary>
        Default,
        /// <summary>
        /// 分页
        /// </summary>
        Pager,
    }
}
