namespace VisualDump.Models.HTMLBuilders
{
    internal class HeadHTMLBuilder : HTMLBuilder
    {
        #region Var
        public StyleHTMLBuilder StyleBuilder { get; }
        #endregion

        #region Init
        public HeadHTMLBuilder() : this(new StyleHTMLBuilder()) { }
        public HeadHTMLBuilder(StyleHTMLBuilder StyleBuilder) => this.StyleBuilder = StyleBuilder;
        #endregion

        #region Functions
        public override string BuildHTML() => $"<head><meta charset='UTF-8'><meta http-equiv='X-UA-Compatible' content='IE=edge'><meta name='viewport' content='width=device-width, initial-scale=1'>{StyleBuilder.BuildHTML()}</head>";
        #endregion
    }
}
