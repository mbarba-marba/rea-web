namespace REA.Web.Components.Data;

public class ColumnDef<T>
{
    public string Field { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public Func<T, object> Value { get; set; } = _ => "";
    public Func<T, IComparable>? Comparer { get; set; }
}
