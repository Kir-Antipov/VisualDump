namespace VisualDump.Models.HTMLBuilders
{
    internal class BodyHTMLBuilder : HTMLBuilder
    {
        #region Var
        public bool IsDarkTheme { get; set; }
        #endregion

        #region Init
        public BodyHTMLBuilder() : this(true) { }
        public BodyHTMLBuilder(bool IsDarkTheme) => this.IsDarkTheme = IsDarkTheme;
        #endregion

        #region Functions
        public override string BuildHTML() => $"<body class='theme-{(IsDarkTheme ? "dark" : "light")}'><a class='button-clear'>Clear</a><content></content></body>";
        #endregion
    }
}
