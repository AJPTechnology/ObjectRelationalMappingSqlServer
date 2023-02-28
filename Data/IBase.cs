using System;
using System.Collections.Generic;

public interface IBase
{

    string PrimaryKey { get; }
    void FillData(ref IBase obj, SqlDataReader reader);
    List<IBase> GetAllTableData();
    List<IBase> GetTableDataByPk(string idValue);
    void Save();
    void Delete();
}