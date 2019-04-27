using System;
using System.Numerics;
using System.Collections.Generic;

namespace VisualDump.HTMLProviders.DefaultProviders
{
    public class NumberProvider : HTMLProvider
    {
        private static readonly HashSet<Type> _available = new HashSet<Type> {
            typeof(sbyte), typeof(byte),
            typeof(short), typeof(ushort),
            typeof(int), typeof(uint),
            typeof(long), typeof(ulong),
            typeof(float), typeof(double),
            typeof(decimal), typeof(BigInteger)
        };

        public override string ToHTML(object Obj, params object[] Args) => _available.Contains(Obj?.GetType()) ? $"<div class='number'>{Obj}</div>" : throw new NotSupportedException();
    }
}
