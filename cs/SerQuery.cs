using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Npgsql;
using NpgsqlTypes;

/// <summary>
/// Сводное описание для SerQuery
/// </summary>
public class SerQuery
{
    public SerQuery()
    {
    
    }

    public string Sql;
    public List<SerQueryParametr> Parametrs = new List<SerQueryParametr>();

    public NpgsqlCommand ToNpgsqlCommand()
    {
        NpgsqlCommand ret = new NpgsqlCommand(Sql);
        foreach (var p in Parametrs)
        {
            ret.Parameters.Add(p.parName, p.parType);
        }
        return ret;
    }
}


public class SerQueryParametr
{
    public NpgsqlDbType parType;
    public int parId;
    public string parName;
}