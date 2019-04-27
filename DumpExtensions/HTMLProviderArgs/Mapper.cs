#if !NETSTANDARD
using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Collections.Generic;

namespace VisualDump.HTMLProviderArgs
{
    public abstract class Mapper
    {
        #region Var
        private static readonly ModuleBuilder ModuleBuilder;
        private static readonly MethodInfo GetTypeMethod;
        private static readonly MethodInfo TypeEqualsMethod;
        private static readonly MethodInfo GetTypeFromHandleMethod;

        private static readonly Dictionary<Type, Mapper> Mappers = new Dictionary<Type, Mapper>();
        #endregion

        #region Init
        static Mapper()
        {
            AssemblyName aName = new AssemblyName("__args_mappers__");
            ModuleBuilder = AssemblyBuilder.DefineDynamicAssembly(aName, AssemblyBuilderAccess.Run).DefineDynamicModule(aName.Name);
            GetTypeMethod = typeof(object).GetMethod("GetType", Type.EmptyTypes);
            GetTypeFromHandleMethod = typeof(Type).GetMethod("GetTypeFromHandle");
            TypeEqualsMethod = typeof(Type).GetMethod("op_Equality");
        }
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

        private static Mapper<T> CreateMapper<T>() where T : new()
        {
            Type t = typeof(T);
            Type baseT = typeof(Mapper<T>);
            Type obj = typeof(object[]);
            Type[] objs = new[] { obj };

            TypeBuilder tb = ModuleBuilder.DefineType($"_{t.Name}_", TypeAttributes.Public, baseT);

            FieldBuilder def = tb.DefineField("_default", t, FieldAttributes.Private | FieldAttributes.Static);

            MethodBuilder builder = tb.DefineMethod("DirectMap", MethodAttributes.Public | MethodAttributes.Virtual, t, objs);

            // Method gen
            ILGenerator il = builder.GetILGenerator();
            Label end = il.DefineLabel();
            Label ret = il.DefineLabel();
            Label endTestTypeIf = il.DefineLabel();
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Brfalse, end);

            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Ldlen);
            il.Emit(OpCodes.Ldc_I4_1);
            il.Emit(OpCodes.Bne_Un, endTestTypeIf);

            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Ldc_I4_0);
            il.Emit(OpCodes.Ldelem_Ref);

            il.Emit(OpCodes.Brfalse, endTestTypeIf);

            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Ldc_I4_0);
            il.Emit(OpCodes.Ldelem_Ref);
            il.EmitCall(OpCodes.Callvirt, GetTypeMethod, null);

            il.Emit(OpCodes.Ldtoken, t);
            il.EmitCall(OpCodes.Call, GetTypeFromHandleMethod, null);

            il.EmitCall(OpCodes.Call, TypeEqualsMethod, null);

            il.Emit(OpCodes.Brfalse, endTestTypeIf);

            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Ldc_I4_0);
            il.Emit(OpCodes.Ldelem_Ref);
            il.Emit(t.IsValueType ? OpCodes.Unbox_Any : OpCodes.Castclass, t);
            il.Emit(OpCodes.Br, ret);

            il.MarkLabel(endTestTypeIf);
            foreach (var group in t.GetConstructors().Select(x => new { x, p = x.GetParameters() }).Where(x => x.p.Length != 0).GroupBy(x => x.p.Length).OrderBy(x => x.Key))
            {
                Label endLengthIf = il.DefineLabel();

                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Ldlen);
                il.Emit(OpCodes.Ldc_I4, group.Key);
                il.Emit(OpCodes.Bne_Un, endLengthIf);

                foreach (var c in group.Select(x => x))
                {
                    Label endConstructorIf = il.DefineLabel();

                    for (int i = 0; i < c.p.Length; ++i)
                    {
                        ParameterInfo p = c.p[i];
                        Label endParamIf = il.DefineLabel();

                        il.Emit(OpCodes.Ldarg_1);
                        il.Emit(OpCodes.Ldc_I4, i);
                        il.Emit(OpCodes.Ldelem_Ref);

                        // struct can't be null
                        // We can't recognize type of null
                        il.Emit(OpCodes.Brfalse, p.ParameterType.IsValueType ? endConstructorIf : endParamIf);

                        il.Emit(OpCodes.Ldarg_1);
                        il.Emit(OpCodes.Ldc_I4, i);
                        il.Emit(OpCodes.Ldelem_Ref);
                        il.EmitCall(OpCodes.Callvirt, GetTypeMethod, null);

                        il.Emit(OpCodes.Ldtoken, p.ParameterType);
                        il.EmitCall(OpCodes.Call, GetTypeFromHandleMethod, null);

                        il.EmitCall(OpCodes.Call, TypeEqualsMethod, null);

                        il.Emit(OpCodes.Brfalse, endConstructorIf);

                        il.MarkLabel(endParamIf);
                    }

                    // return new T(...);
                    for (int i = 0; i < c.p.Length; ++i)
                    {
                        ParameterInfo p = c.p[i];
                        il.Emit(OpCodes.Ldarg_1);
                        il.Emit(OpCodes.Ldc_I4, i);
                        il.Emit(OpCodes.Ldelem_Ref);
                        il.Emit(p.ParameterType.IsValueType ? OpCodes.Unbox_Any : OpCodes.Castclass, p.ParameterType);
                    }
                    il.Emit(OpCodes.Newobj, c.x);
                    il.Emit(OpCodes.Br, ret);

                    il.MarkLabel(endConstructorIf);
                }

                il.MarkLabel(endLengthIf);
            }

            il.MarkLabel(end);
            il.Emit(OpCodes.Ldsfld, def);
            il.MarkLabel(ret);
            il.Emit(OpCodes.Ret);
            // End of method gen

            tb.DefineMethodOverride(builder, baseT.GetMethod("DirectMap"));

            Type resultType = tb.CreateTypeInfo();
            resultType.GetField("_default", BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, new T());
            return (Mapper<T>)Activator.CreateInstance(resultType);
        }
        #endregion
    }

    public abstract class Mapper<T> : Mapper where T : new()
    {
        public abstract T DirectMap(object[] Args);
        public override object Map(object[] Args) => DirectMap(Args);
    }
}
#endif