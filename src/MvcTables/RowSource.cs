namespace MvcTables
{
    public class RowSource<TRow>
    {
        public RowSource(TRow source, int index)
        {
            Index = index;
            DataObject = source;
        }

        public int Index { get; set; }
        public TRow DataObject { get; set; }
    }
}