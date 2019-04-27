namespace VisualDump.Models.HTMLBuilders
{
    internal class DefaultScriptHTMLBuilder : ScriptHTMLBuilder
    {
        public DefaultScriptHTMLBuilder()
        {
            Appends.Add(DefaultScripts.JQuery);
            Appends.Add(DefaultScripts.Render);
            Appends.Add(DefaultScripts.ChangeTheme);
            Appends.Add(DefaultScripts.Clear);
        }
    }
}
