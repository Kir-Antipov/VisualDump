using System;
using VisualDump.Helpers;
using VisualDump.ExtraTypes;
using System.Collections.Generic;
using VisualDump.HTMLProviderArgs;

namespace VisualDump.HTMLProviders
{
    public abstract class HTMLProvider
    {
        #region Var
        private class CustomHTMLProvider<T> : HTMLProvider
        {
            private Func<T, Stack<object>, object[], string> HTMLCreator { get; }

            public CustomHTMLProvider(Func<T, Stack<object>, string> HTMLCreator, bool NeedToRegister)
            {
                if (HTMLCreator is null)
                    throw new ArgumentNullException();
                this.HTMLCreator = (x, s, o) => HTMLCreator(x, s.CloneAndPush(x));
                if (NeedToRegister)
                    Register();
            }
            public CustomHTMLProvider(Func<T, Stack<object>, string> HTMLCreator) : this(HTMLCreator, true) { }
            public CustomHTMLProvider(Func<T, Stack<object>, object[], string> HTMLCreator, bool NeedToRegister)
            {
                this.HTMLCreator = HTMLCreator ?? throw new ArgumentNullException();
                if (NeedToRegister)
                    Register();
            }
            public CustomHTMLProvider(Func<T, Stack<object>, object[], string> HTMLCreator) : this(HTMLCreator, true) { }

            public bool Register() => DumpExtensions.Register(this, typeof(T));
            public override string ToHTML(object Obj, Stack<object> CallStack, params object[] Args) => Obj is T t ? HTMLCreator(t, CallStack, Args) : throw new NotSupportedException();
        }
        #endregion

        #region Functions
        public abstract string ToHTML(object Obj, Stack<object> CallStack, params object[] Args);
        protected string ToHTML<T>(object Obj, Stack<object> CallStack, Func<T, Stack<object>, string> Creator) => Obj is null ? GetProvider<NullReference>().ToHTML(Obj, CallStack.CloneAndPush(Obj)) : Obj is T t ? Creator(t, CallStack.CloneAndPush(t)) : throw new NotSupportedException();
        protected string ToHTML<T>(object Obj, Stack<object> CallStack, object[] Args, Func<T, Stack<object>, object[], string> Creator) => Obj is null ? GetProvider<NullReference>().ToHTML(Obj, CallStack.CloneAndPush(Obj), Args) : Obj is T t ? Creator(t, CallStack.CloneAndPush(t), Args) : throw new NotSupportedException();
        protected string ToHTML<T, U>(object Obj, Stack<object> CallStack, object[] Args, Func<T, Stack<object>, U, string> Creator) where U : new() => Obj is null ? GetProvider<NullReference>().ToHTML(Obj, CallStack.CloneAndPush(Obj), Args) : Obj is T t ? Creator(t, CallStack.CloneAndPush(t), Mapper.Map<U>(Args)) : throw new NotSupportedException();

        public static HTMLProvider GetProvider<T>() => DumpExtensions.GetProvider(typeof(T));
        public static HTMLProvider GetProvider(Type Type) => DumpExtensions.GetProvider(Type);
        public static HTMLProvider GetProvider(object Obj) => DumpExtensions.GetProvider(Obj?.GetType());

        public static HTMLProvider CreateProvider<T>(Func<T, Stack<object>, string> HTMLCreator) => new CustomHTMLProvider<T>(HTMLCreator);
        public static HTMLProvider CreateProvider<T>(Func<T, Stack<object>, object[], string> HTMLCreator) => new CustomHTMLProvider<T>(HTMLCreator);
        public static HTMLProvider CreateProvider<T>(Func<T, Stack<object>, string> HTMLCreator, bool NeedToRegister) => new CustomHTMLProvider<T>(HTMLCreator, NeedToRegister);
        public static HTMLProvider CreateProvider<T>(Func<T, Stack<object>, object[], string> HTMLCreator, bool NeedToRegister) => new CustomHTMLProvider<T>(HTMLCreator, NeedToRegister);
        #endregion
    }
}
