#if NETFRAMEWORK
using System.Drawing;

namespace VisualDump.HTMLProviderArgs
{
    public class ImageArgs
    {
        #region Var
        public Size MaxSize { get; }
        #endregion

        #region Init
        public ImageArgs() : this(Size.Empty) { }
        public ImageArgs(Size MaxSize) => this.MaxSize = MaxSize;
        #endregion
    }
}
#endif