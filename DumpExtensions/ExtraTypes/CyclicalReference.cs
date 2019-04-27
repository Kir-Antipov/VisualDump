namespace VisualDump.ExtraTypes
{
    public class CyclicalReference
    {
        public object CyclicalObject { get; }
        public CyclicalReference(object CyclicalObject) => this.CyclicalObject = CyclicalObject;
    }
}
