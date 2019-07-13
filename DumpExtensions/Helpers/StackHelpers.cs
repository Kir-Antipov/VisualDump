using System.Linq;
using System.Collections.Generic;

namespace VisualDump.Helpers
{
    internal static class StackHelpers
    {
        public static Stack<T> CloneAndPush<T>(this Stack<T> Stack, T Value)
        {
            Stack<T> result = new Stack<T>(Stack.Reverse());
            result.Push(Value);
            return result;
        }
    }
}
