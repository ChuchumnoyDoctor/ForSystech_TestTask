using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace CommonDll.Helps
{
    public static class DelegateToMethod
    {
        /// <summary>
        /// Создание generic-делегата
        /// </summary>
        /// <param name="MethodName"></param>
        /// <param name="ClassName"></param>
        /// <returns></returns>
        public static Tuple<Delegate, string> GetTypeOfDelegate(string MethodName, string ClassName)
        {
            Delegate @delegate = default(Delegate);
            string ExceptionMessage = "";

            if (!string.IsNullOrEmpty(MethodName) & !string.IsNullOrEmpty(ClassName))
                try
                {
                    Type type = Type.GetType(ClassName);
                    MethodInfo method = type.GetMethod(MethodName, BindingFlags.Public | BindingFlags.Static);

                    if (!(method is null))
                    {
                        Type t = Expression.GetDelegateType((from parameter in method.GetParameters() select parameter.ParameterType).Concat(new[] { method.ReturnType }).ToArray());
                        @delegate = method.CreateDelegate(t);
                    }
                }
                catch (Exception ex)
                {
                    ExceptionMessage = ex.Message.ToString();
                }

            return new Tuple<Delegate, string>(@delegate, ExceptionMessage);
        }
    }
}
