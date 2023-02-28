using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public class Base : IBase
{
    /// <summary>
    /// Property to fetch the name of the table for the search in the query, through reflection by the properties of the class inherited from the base
    /// </summary>
    private string GetTableName
    {
        get
        {
            AttributeDataBase[] dbAttributes = (AttributeDataBase[])this.GetType().GetCustomAttributes(typeof(AttributeDataBase), false);
            if (dbAttributes != null && dbAttributes.Length > 0)
            {
                return string.Concat("[", dbAttributes[0].DataBaseName, "].[dbo].[", dbAttributes[0].TableName, "]") ?? string.Empty;
            }
            else
            { return string.Empty; }
        }

    }

    /// <summary>
    /// Property to fetch the name of the connection string for the search in the query, through reflection by the properties of the class inherited from the base
    /// </summary>
    public string ConnectionString
    {
        get
        {
            AttributeDataBase[] dbAttributes = (AttributeDataBase[])this.GetType().GetCustomAttributes(typeof(AttributeDataBase), false);
            if (dbAttributes != null && dbAttributes.Length > 0)
            {
                return ConfigurationManager.ConnectionStrings[dbAttributes[0].DataBaseName].ConnectionString ?? string.Empty;
            }
            else
            { return string.Empty; }
        }
    }

    /// <summary>
    /// Property to fetch the table's primary key for the search in the query, through reflection by class properties inherited from the base
    /// </summary>
    public string PrimaryKey
    {
        get
        {
            foreach (PropertyInfo pi in this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                #pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                AttributeTable optionBase = (AttributeTable)pi.GetCustomAttribute(typeof(AttributeTable), false);
                #pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

                if (optionBase != null && optionBase.IsPrimaryKey)
                {
                    return Convert.ToString(pi.Name) ?? string.Empty;
                }
            }
            return string.Empty;
        }
    }

    /// <summary>
    /// Property to fetch name and value values
    /// </summary>
    public Dictionary<string, string> DataFields
    {
        get
        {
            Dictionary<string, string> filesNamesValues = new Dictionary<string, string>();
            foreach (PropertyInfo pi in this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                #pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                AttributeTable atributeTable = (AttributeTable)pi.GetCustomAttribute(typeof(AttributeTable));
                #pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                if (atributeTable != null && atributeTable.IsTableDataField && !atributeTable.IsComputed)
                {
                    filesNamesValues.Add(pi.Name, "'" + pi.GetValue(this) + "'");
                }
            }
            return filesNamesValues;
        }
    }
    /// <summary>
    /// Method to return all records from a table
    /// </summary>
    public List<IBase> GetAllTableData()
    {
        var tableData = new List<IBase>();
        SqlExecuteComand(string.Concat("Select * from ", GetTableName), tableData);
        return tableData;
    }

    /// <summary>
    /// Method to return all records from a table by primary key
    /// </summary>
    public List<IBase> GetTableDataByPk(string idValue)
    {
        List<IBase> tableData = new List<IBase>();
        string where = string.Empty;

        foreach (PropertyInfo pi in this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            #pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            AttributeTable optionBase = (AttributeTable)pi.GetCustomAttribute(typeof(AttributeTable));
            #pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            {
                if (optionBase != null)
                {
                    if (optionBase.IsPrimaryKey)
                    {
                        where = (string.Concat(pi.Name, " = '", idValue, "'"));
                    }
                }
            }
        }

        SqlExecuteComand(string.Concat("Select * from ", GetTableName, " Where ", where), tableData);

        return tableData;
    }

    /// <summary>
    /// Method to save data to table
    /// </summary>
    public virtual void Save()
    {
        string queryString = "insert into " + GetTableName + " (" + string.Join(", ", DataFields.Keys.ToArray()) + ")values(" + string.Join(", ", DataFields.Values.ToArray()) + ")";

        using (SqlConnection connection = new SqlConnection(ConnectionString))
        {
            SqlCommand command = new SqlCommand(queryString, connection);
            command.Connection.Open();

            command.ExecuteNonQuery();
        }
    }

    /// <summary>
    /// Method to delete data from table
    /// </summary>
    public virtual void Delete()
    {
        string where=string.Empty;
        foreach (PropertyInfo pi in this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            #pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            AttributeTable fieldBase = (AttributeTable)pi.GetCustomAttribute(typeof(AttributeTable));
            #pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            {
                if (fieldBase != null)
                {
                    if (fieldBase.IsPrimaryKey)
                    {
                        where = (string.Concat(pi.Name, " = '", idValue, "'"));
                    }
                }
            }
        }

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = string.Concat("delete from ",GetTableName, " Where =", where); 

            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
