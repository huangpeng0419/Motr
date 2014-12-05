using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Motr.Data.Mapping
{
    /// <summary>
    /// 映射列
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class DbColumn : Attribute
    {
        public DbColumn() { }
        public DbColumn(Boolean isPrimaryKey, Boolean isIdentity, Boolean isExtensionProperty)
        {
            this.IsExtensionProperty = isExtensionProperty;
            this.IsIdentity = isIdentity;
            this.IsPrimaryKey = isPrimaryKey;
        }
        public Boolean IsPrimaryKey { get; set; }
        public Boolean IsIdentity { get; set; }
        public Boolean IsExtensionProperty { get; set; }
    }
}
