using System;

public class AttributeDataBase : Attribute
{
    public string? DataBaseName { get; set; }
    public string? TableName { get; set; }
}
