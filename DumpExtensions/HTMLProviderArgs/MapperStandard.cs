#if NETSTANDARD
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace VisualDump.HTMLProviderArgs
{
    public abstract class Mapper
    {
        #region Var
        private static readonly Dictionary<Type, Mapper> Mappers = new Dictionary<Type, Mapper>();
        #endregion

        #region Abstract
        public abstract object Map(object[] Args);
        #endregion

        #region Functions
        public static T Map<T>(params object[] Args) where T : new()
        {
            Type type = typeof(T);
            if (Mappers.TryGetValue(type, out Mapper m))
                return ((Mapper<T>)m).DirectMap(Args);
            Mapper<T> mapper = CreateMapper<T>();
            Mappers[type] = mapper;
            return mapper.DirectMap(Args);
        }

        private static Mapper<T> CreateMapper<T>() where T : new() => new Mapper<T>();
        #endregion
    }

    public class Mapper<T> : Mapper where T : new()
    {
        internal Mapper() { }

        public virtual T DirectMap(object[] Args)
        {
            if (Args == null || Args.Length == 0)
                return new T();
            if (Args.Length == 1 && Args[0] is T t)
                return t;
            ConstructorInfo c = typeof(T).GetConstructors().Where(x => x.GetParameters().Length == Args.Length).FirstOrDefault(x => {
                ParameterInfo[] parameters = x.GetParameters();
                for (int i = 0; i < parameters.Length; ++i)
                {
                    if (Args[i] == null)
                        if (parameters[i].ParameterType.IsValueType)
                            return false;
                        else
                            continue;
                    if (Args[i].GetType() != parameters[i].ParameterType)
                        return false;
                }
                return true;
            });
            if (c == null)
                return new T();
            return (T)c.Invoke(Args);
        }
        public override object Map(object[] Args) => DirectMap(Args);
    }
}
#endif