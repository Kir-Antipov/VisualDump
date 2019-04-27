using System;
using VisualDump.ExtraTypes;
using VisualDump.HTMLProviderArgs;

namespace VisualDump.HTMLProviders
{
    public abstract class HTMLProvider
    {
        #region Var
        private class CustomHTMLProvider<T> : HTMLProvider
        {
            private Func<T, object[], string> HTMLCreator { get; }

            public CustomHTMLProvider(Func<T, string> HTMLCreator, bool NeedToRegister)
            {
                if (HTMLCreator is null)
                    throw new ArgumentNullException();
                this.HTMLCreator = (x, o) => HTMLCreator(x);
                if (NeedToRegister)
                    Register();
            }
            public CustomHTMLProvider(Func<T, string> HTMLCreator) : this(HTMLCreator, true) { }
            public CustomHTMLProvider(Func<T, object[], string> HTMLCreator, bool NeedToRegister)
            {
                this.HTMLCreator = HTMLCreator ?? throw new ArgumentNullException();
                if (NeedToRegister)
                    Register();
            }
            public CustomHTMLProvider(Func<T, object[], string> HTMLCreator) : this(HTMLCreator, true) { }

            public bool Register() => DumpExtensions.Register(this, typeof(T));
            public override string ToHTML(object Obj, params object[] Args) => Obj is T t ? HTMLCreator(t, Args) : throw new NotSupportedException();
        }
        #endregion

        #region Functions
        public abstract string ToHTML(object Obj, params object[] Args);
        protected string ToHTML<T>(object Obj, Func<T, string> Creator) => Obj is null ? GetProvider<NullReference>().ToHTML(Obj) : Obj is T t ? Creator(t) : throw new NotSupportedException();
        protected string ToHTML<T>(object Obj, object[] Args, Func<T, object[], string> Creator) => Obj is null ? GetProvider<NullReference>().ToHTML(Obj, Args) : Obj is T t ? Creator(t, Args) : throw new NotSupportedException();
        protected string ToHTML<T, U>(object Obj, object[] Args, Func<T, U, string> Creator) where U : new() => Obj is null ? GetProvider<NullReference>().ToHTML(Obj, Args) : Obj is T t ? Creator(t, Mapper.Map<U>(Args)) : throw new NotSupportedException();

        public static HTMLProvider GetProvider<T>() => DumpExtensions.GetProvider(typeof(T));
        public static HTMLProvider GetProvider(Type Type) => DumpExtensions.GetProvider(Type);
        public static HTMLProvider GetProvider(object Obj) => DumpExtensions.GetProvider(Obj?.GetType() ?? typeof(NullReference));

        public static HTMLProvider CreateProvider<T>(Func<T, string> HTMLCreator) => new CustomHTMLProvider<T>(HTMLCreator);
        public static HTMLProvider CreateProvider<T>(Func<T, string> HTMLCreator, bool NeedToRegister) => new CustomHTMLProvider<T>(HTMLCreator, NeedToRegister);
        public static HTMLProvider CreateProvider<T>(Func<T, object[], string> HTMLCreator) => new CustomHTMLProvider<T>(HTMLCreator);
        public static HTMLProvider CreateProvider<T>(Func<T, object[], string> HTMLCreator, bool NeedToRegister) => new CustomHTMLProvider<T>(HTMLCreator, NeedToRegister);
        #endregion
    }
}
