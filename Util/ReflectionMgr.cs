using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Motr.Util
{
    public class ReflectionMgr
    {
        private ReflectionMgr() { }
        public ReflectionMgr(String dllPath, String classFullName) : this(dllPath, classFullName, String.Empty) { }
        public ReflectionMgr(String dllPath, String classFullName, String methodName) : this(dllPath, classFullName, methodName, new Type[] { }, null) { }
        public ReflectionMgr(String dllPath, String classFullName, String methodName, Type[] paramType, Object[] paramValue)
        {
            this.DllPath = dllPath;
            this.ClassFullName = classFullName;
            this.MethodName = methodName;
            this.ParamType = paramType;
            this.ParamValue = paramValue;
            this._assembly = Assembly.LoadFrom(this.DllPath);
        }
        private Assembly _assembly { get; set; }
        public String DllPath { get; set; }
        public String ClassFullName { get; set; }
        public String MethodName { get; set; }
        public Type[] ParamType { get; set; }
        public Object[] ParamValue { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T CreateInstance<T>() { return (T)CreateInstance(); }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Object CreateInstance() { return this._assembly.CreateInstance(this.ClassFullName); }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T InvokeMethod<T>() { return (T)InvokeMethod(); }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Object InvokeMethod()
        {
            if (this.ParamType != null && this.ParamValue != null && this.ParamType.Length != this.ParamValue.Length)
                throw new ArgumentException("Param unequal length!");
            var type = CreateInstance();
            var method = type.GetType().GetMethod(MethodName, this.ParamType);
            return method.Invoke(type, this.ParamValue);
        }
    }
}
