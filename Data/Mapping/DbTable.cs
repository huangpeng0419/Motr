using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Motr.Data.Mapping
{
    /// <summary>
    /// 映射表
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class DbTable : Attribute
    {
        public DbTable() { }
        public DbTable(String name)
        {
            Name = name;
        }
        public String Name { get; set; }
    }
}
