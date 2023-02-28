using System;

[AttributeDataBase(DataBaseName = "NomeBaseDados", TableName = "NomeTabele")]
public class clsPessoa : Base
{
    [AttributeTable(IsPrimaryKey = true, IsComputed = true, IsIdentityIncrement = false, IsTableDataField = true, Name = "PessoaId")]
    public int PessoaId { get; set; }

    [AttributeTable(IsPrimaryKey = false, IsComputed = false, IsIdentityIncrement = false, IsTableDataField = true, Name = "Nome")]
    public string Nome { get; set; }

    [AttributeTable(IsPrimaryKey = false, IsComputed = false, IsIdentityIncrement = false, IsTableDataField = true, Name = "Status")]
    public int Status { get; set; }

}
