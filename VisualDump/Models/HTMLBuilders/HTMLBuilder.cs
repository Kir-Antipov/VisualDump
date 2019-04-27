namespace VisualDump.Models.HTMLBuilders
{
    internal abstract class HTMLBuilder
    {
        public abstract string BuildHTML();

        public override string ToString() => BuildHTML();
    }
}
