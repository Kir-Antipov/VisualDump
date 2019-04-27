using System;

namespace VisualDump.HTMLProviders
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class ExportHTMLProviderAttribute : Attribute
    {
        public Type ExportType { get; }

        public ExportHTMLProviderAttribute(Type ExportType) => this.ExportType = ExportType;
    }
}
