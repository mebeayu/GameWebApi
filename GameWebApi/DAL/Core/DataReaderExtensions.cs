using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GameWebApi.DAL
{
    [DebuggerStepThrough]
    public static class DataReaderExtensions
    {
        #region Private Methods
        private static readonly Dictionary<Type, Delegate> cache = new Dictionary<Type, Delegate>();
        private static readonly object cacheLocker = new object();
        #endregion

        #region Business Methods
        /// <summary>
        /// 返回指定字段的值, 并执行 ConvertTo 函数转换。
        /// </summary>
        /// <typeparam name="T">返回值类型。</typeparam>
        /// <param name="reader">一个实现了 IDataReader 接口的实例对象。</param>
        /// <param name="name">要查找的字段的名称。</param>
        /// <returns>转换完毕的 T 类型的结果。</returns>
        public static T Field<T>(this IDataReader reader, string name)
        {
            return reader[name].ConvertTo<T>(default(T), true);
        }

        /// <summary>
        /// 返回指定字段的值, 并执行 ConvertTo 函数转换。
        /// </summary>
        /// <typeparam name="T">返回值类型。</typeparam>
        /// <param name="reader">一个实现了 IDataReader 接口的实例对象。</param>
        /// <param name="index">要查找的字段的索引。</param>
        /// <returns>转换完毕的 T 类型的结果。</returns>
        public static T Field<T>(this IDataReader reader, int index)
        {
            return reader[index].ConvertTo<T>(default(T), true);
        }

        /// <summary>
        /// 解析当前 IDataReader 类型的实例对象并提取一个 T 类型的列表。
        /// </summary>
        /// <typeparam name="T">待解析的元素类型, 该类型必须包含一个默认的构造函数。</typeparam>
        /// <param name="reader">一个实现了 IDataReader 接口的实例对象。</param>
        /// <returns>一个 T 类型的列表。</returns>
        public static List<T> ToList<T>(this IDataReader reader) where T : class, new()
        {
            return Fill<T>(reader, DynamicCreateEntity<T>(reader)).ToList();
        }

        /// <summary>
        /// 解析当前 IDataReader 类型的实例对象并提取一个 T 类型的列表。
        /// </summary>
        /// <typeparam name="T">待解析的元素类型, 该类型必须包含一个默认的构造函数。</typeparam>
        /// <param name="reader">一个实现了 IDataReader 接口的实例对象。</param>
        /// <param name="predicate">映射委托。</param>
        /// <returns>一个 T 类型的列表。</returns>
        public static List<T> ToList<T>(this IDataReader reader, Func<IDataReader, T> predicate)
            where T : class, new()
        {
            return Fill<T>(reader, predicate).ToList();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// 创建一个 构造函数 委托。
        /// </summary>
        /// <typeparam name="T">构造目标类型。</typeparam>
        /// <returns>构造完毕的 Func 委托。</returns>
        private static Func<IDataReader, T> DynamicCreateEntity<T>(IDataReader reader) where T : class, new()
        {
            var type = typeof(T);
            if (cache.ContainsKey(type))
                return (Func<IDataReader, T>)cache[type];

            lock (cacheLocker)
            {
                if (cache.ContainsKey(type))
                    return (Func<IDataReader, T>)cache[type];

                var result = DynamicCreateEntityLogic<T>(  reader);
                cache.Add(type, result);
                return result;
            }
        }

        /// <summary>
        /// 创建一个 构造函数 委托（逻辑实现）。
        /// </summary>
        /// <typeparam name="T">构造目标类型。</typeparam>
        /// <returns>构造完毕的 Func 委托。</returns>
        private static Func<IDataReader, T> DynamicCreateEntityLogic<T>(IDataReader rdr) where T : class, new()
        {
            // Compiles a delegate of the form (IDataReader r) => new T { Prop1 = r.Field<Prop1Type>("Prop1"), ... }
            ParameterExpression r = Expression.Parameter(typeof(IDataReader), "r");
            HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (rdr != null)
            {
                for (int i = 0; i < rdr.FieldCount; i++)
                {
                    hashSet.Add(rdr.GetName(i));
                }
            }

            // Get Properties of the property can read and write
            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead && p.CanWrite)
                .ToArray();

            // Create property bindings for all writable properties
            List<MemberBinding> bindings = new List<MemberBinding>(props.Length);

            // Get the binding method
            var method = typeof(DataReaderExtensions).GetMethods()
                .First(p => p.Name == "Field" &&
                            p.GetParameters().Length == 2 &&
                            p.GetParameters()[1].ParameterType == typeof(string));

            foreach (PropertyInfo property in (typeof(T).GetProperties()))
            {
                if (!hashSet.Contains(property.Name))
                {
                    continue;
                }
                // Create expression representing r.Field<property.PropertyType>(property.Name)
                MethodCallExpression propertyValue = Expression.Call(
                    method.MakeGenericMethod(property.PropertyType),
                    r, Expression.Constant(property.Name));

                // Assign the property value to property through a member binding
                MemberBinding binding = Expression.Bind(property, propertyValue);
                bindings.Add(binding);
            }
            // Create the initializer, which instantiates an instance of T and sets property values

            // using the member bindings we just created
            Expression initializer = Expression.MemberInit(Expression.New(typeof(T)), bindings);

            // Create the lambda expression, which represents the complete delegate (r => initializer)
            Expression<Func<IDataReader, T>> lambda = Expression.Lambda<Func<IDataReader, T>>(initializer, r);

            return lambda.Compile();
        }

        /// <summary>
        /// 从一个 IDataReader 的实例对象中提取一个 T 类型的列表。
        /// </summary>
        /// <typeparam name="T">结果列表中的元素类型, 该类型必须包含一个默认的构造函数。</typeparam>
        /// <param name="reader">一个实现了 IDataReader 接口的实例对象。</param>
        /// <returns>一个 T 类型的列表。</returns>
        private static IEnumerable<T> Fill<T>(IDataReader reader, Func<IDataReader, T> predicate) where T : class, new()
        {
            while (reader.Read())
                yield return predicate(reader);
        }
        #endregion

        private static string typeIConvertibleFullName = typeof(IConvertible).FullName;

        /// <summary>
        /// 将当前实例对象类型转换为 T 类型.
        /// </summary>
        /// <typeparam name="T">目标类型.</typeparam>
        /// <param name="obj">当前实例.</param>
        /// <returns>转换完成的 T 类型的一个实例对象.</returns>
        public static T ConvertTo<T>(this object obj)
        {
            return ConvertTo(obj, default(T));
        }

        /// <summary>
        /// 将当前实例对象类型转换为 T 类型.
        /// </summary>
        /// <typeparam name="T">目标类型.</typeparam>
        /// <param name="obj">当前实例.</param>
        /// <param name="defaultValue">转换失败时的返回值.</param>
        /// <returns>转换完成的 T 类型的一个实例对象.</returns>
        public static T ConvertTo<T>(this object obj, T defaultValue)
        {
            if (obj != null)
            {
                if (obj is T)
                    return (T)obj;

                var sourceType = obj.GetType();
                var targetType = typeof(T);

                if (targetType.IsEnum)
                    return (T)Enum.Parse(targetType, obj.ToString(), true);

                if (sourceType.GetInterface(typeIConvertibleFullName) != null &&
                    targetType.GetInterface(typeIConvertibleFullName) != null)
                    return (T)Convert.ChangeType(obj, targetType);

                var converter = TypeDescriptor.GetConverter(obj);
                if (converter != null && converter.CanConvertTo(targetType))
                    return (T)converter.ConvertTo(obj, targetType);

                converter = TypeDescriptor.GetConverter(targetType);
                if (converter != null && converter.CanConvertFrom(sourceType))
                    return (T)converter.ConvertFrom(obj);

                return default(T);
                //throw new ApplicationException("convert error.");
            }
             throw new ArgumentNullException("obj");
        }

        /// <summary>
        /// 将当前实例对象类型转换为 T 类型.
        /// </summary>
        /// <typeparam name="T">目标类型.</typeparam>
        /// <param name="obj">当前实例.</param>
        /// <param name="defaultValue">转换失败时的返回值.</param>
        /// <param name="ignoreException">如果设置为 <c>true</c> 表示忽略异常信息, 直接返回缺省值.</param>
        /// <returns>转换完成的 T 类型的一个实例对象.</returns>
        public static T ConvertTo<T>(this object obj, T defaultValue, bool ignoreException)
        {
            if (ignoreException)
            {
                if (obj is System.DBNull)
                {
                    return defaultValue;
                }
                else
                {

                    try
                    {
                        return obj.ConvertTo<T>(defaultValue);
                    }
                    catch
                    {
                        return defaultValue;
                    }
                }
            }
            return obj.ConvertTo<T>(defaultValue);
        }
    }


}
