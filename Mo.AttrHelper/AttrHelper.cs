using System;
using System.Collections;
using System.Linq;
using System.Reflection;

namespace Mo.AttrHelper
{
    public static class AttrHelper
    {
        // 缓存采用哈希表，相比于Dictionary 线程的安全性更好一些。
        // 可以考虑用MemoryCache 替代，来实现过期时间
        private static Hashtable _cache = new Hashtable();

        /// <summary>
        /// 获取属性数组
        /// </summary>
        /// <typeparam name="T">Attribute 类型</typeparam>
        /// <param name="obj">对象</param>
        /// <returns>Attribute[]</returns>
        public static T[] GetAttrs<T>(object obj) where T : Attribute
        {
            // Attribute 特征字符串：全名
            // 没有想到更高效的替代方法
            string clzT = typeof(T).FullName;
            return GetPropertyAttrs<T>(obj, clzT);
        }

        /// <summary>
        /// 获取属性
        /// </summary>
        /// <typeparam name="T">Attribute 类型</typeparam>
        /// <param name="obj">对象</param>
        /// <returns>Attribute</returns>
        public static T GetAttr<T>(object obj) where T : Attribute
        {
            return GetAttrs<T>(obj).FirstOrDefault();
        }

        /// <summary>
        /// 如果obj FieldInfo 类型
        /// </summary>
        /// <typeparam name="T">Attribute 类型</typeparam>
        /// <param name="obj">对象</param>
        /// <param name="clazT">Attribute 特征字符串：全名</param>
        /// <returns>Attribute[]</returns>
        private static T[] GetFieldAttrs<T>(object obj, string clazT) where T : Attribute
        {
            if (obj is FieldInfo)
            {
                var temp = obj as FieldInfo;
                // 这里拼接字符串可以不用空格的，因为空格也会降低程序效率，影响还是比较明显的
                var key = $"{clazT} {temp.DeclaringType.FullName} {temp.Name}";
                T[] result = new T[] { };
                if (_cache.ContainsKey(key))
                {
                    result = (T[]) _cache[key];
                }
                else
                {
                    if (temp.IsDefined(typeof(T)))
                    {
                        result = (T[]) temp.GetCustomAttributes(typeof(T), false);
                    }

                    _cache.Add(key, result);
                }

                return result;
            }
            else
            {
                // 如果不是PropertyInfo 则识别类的Attributes
                return GetPropertyAttrs<T>(obj, clazT);
            }
        }

        /// <summary>
        /// 如果obj 是PropertyInfo 类型
        /// </summary>
        /// <typeparam name="T">Attribute 类型</typeparam>
        /// <param name="obj">对象</param>
        /// <param name="clazT">Attribute 特征字符串：全名</param>
        /// <returns>Attribute[]</returns>
        private static T[] GetPropertyAttrs<T>(object obj, string clazT) where T : Attribute
        {
            if (obj is PropertyInfo)
            {
                var temp = obj as PropertyInfo;
                // 这里拼接字符串可以不用空格的，因为空格也会降低程序效率，影响还是比较明显的
                var key = $"{clazT} {temp.DeclaringType.FullName} {temp.Name}";
                T[] result = new T[] { };
                if (_cache.ContainsKey(key))
                {
                    result = (T[]) _cache[key];
                }
                else
                {
                    if (temp.IsDefined(typeof(T)))
                    {
                        result = (T[]) temp.GetCustomAttributes(typeof(T), false);
                    }

                    _cache.Add(key, result);
                }

                return result;
            }
            else
            {
                // 如果不是PropertyInfo 则识别类的Attributes
                return GetMethodAttrs<T>(obj, clazT);
            }
        }

        /// <summary>
        /// 如果obj MethodInfo 类型
        /// </summary>
        /// <typeparam name="T">Attribute 类型</typeparam>
        /// <param name="obj">对象</param>
        /// <param name="clazT">Attribute 特征字符串：全名</param>
        /// <returns>Attribute[]</returns>
        private static T[] GetMethodAttrs<T>(object obj, string clazT) where T : Attribute
        {
            if (obj is MethodInfo)
            {
                var temp = obj as MethodInfo;
                // 这里拼接字符串可以不用空格的，因为空格也会降低程序效率，影响还是比较明显的
                var key = $"{clazT} {temp.DeclaringType.FullName} {temp.Name}";
                T[] result = new T[] { };
                if (_cache.ContainsKey(key))
                {
                    result = (T[]) _cache[key];
                }
                else
                {
                    if (temp.IsDefined(typeof(T)))
                    {
                        result = (T[]) temp.GetCustomAttributes(typeof(T), false);
                    }

                    _cache.Add(key, result);
                }

                return result;
            }
            else
            {
                // 如果不是PropertyInfo 则识别类的Attributes
                return GetClassAttrs<T>(obj, clazT);
            }
        }

        /// <summary>
        /// 识别类的Attributes
        /// </summary>
        /// <typeparam name="T">Attribute 类型</typeparam>
        /// <param name="obj">对象</param>
        /// <param name="clazT">Attribute 特征字符串：全名</param>
        /// <returns>Attribute[]</returns>
        private static T[] GetClassAttrs<T>(object obj, string clazT) where T : Attribute
        {
            string key = $"{clazT} {obj.GetType().FullName}";
            T[] result = new T[] { };
            if (_cache.ContainsKey(key))
            {
                result = (T[]) _cache[key];
            }
            else
            {
                if (obj.GetType().IsDefined(typeof(T)))
                {
                    result = (T[]) obj.GetType().GetCustomAttributes(typeof(T), false);
                }

                _cache.Add(key, result);
            }

            return result;
        }
    }
}