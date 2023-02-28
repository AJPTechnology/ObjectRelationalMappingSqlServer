using System;

public class AttributeTable : Attribute
{
    public string? Name { set; get; }
    public bool IsPrimaryKey { get; set; }
    public bool IsTableDataField { get; set; }
    public bool IsIdentityIncrement { get; set; }
    public bool IsComputed { get; set; }
    public bool AllowNulls { get; set; }
}