using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;

public class CrossTable<T> : CrossTable<Double, T>
{

}

public class CrossTable<TRow, T> : System.Collections.Generic.IEnumerable<T>
{
    public List<TRow> Rows;     //строки
    public List<string> Columns;  //столбцы

    public Dictionary<TRow, Dictionary<string, T>> Data;

    public static CrossTable<TRow, T> GetCrossTableFromSql(DbCommand sqlDbCommand)
    {
        CrossTable<TRow, T> ret = new CrossTable<TRow, T>();
        if(sqlDbCommand.Connection.State != ConnectionState.Open)
            sqlDbCommand.Connection.Open();

        using (var reader = sqlDbCommand.ExecuteReader())
        {
            while (reader.Read())
            {
                ret[ConvertTo<TRow>(reader[0]), reader[1].ToString()] = ConvertTo<T>(reader[2]);
            }
        }

        return ret;
    }

    static T ConvertTo<T>(object o, T def = default(T))
    {
        try
        {
            return (T)Convert.ChangeType(o, typeof(T));
        }
        catch (Exception)
        {
            return def;
        }
    }

    public CrossTable()
    {
        Rows = new List<TRow>();
        Columns = new List<string>();
        Data = new Dictionary<TRow, Dictionary<string, T>>();
    }

    private void Set(TRow row, string col, T d)
    {
        if (Rows.IndexOf(row) < 0)
        {
            Rows.Add(row);
            Data[row] = new Dictionary<string, T>();
        }

        if (Columns.IndexOf(col) < 0)
            Columns.Add(col);

        Data[row][col] = d;
    }

    public void Dst()
    {
        Rows = Rows.Distinct().ToList();
        Columns = Columns.Distinct().ToList();
    }


    private T Get(TRow row, string col)
    {
        //try
        //{

        if (Data.ContainsKey(row) && Data[row].ContainsKey(col))
            return Data[row][col];

        return default(T);
        //}
        //catch (Exception)
        //{
        //   return default(T);
        //}
    }

    public T this[TRow row, string col]
    {
        get
        {
            return Get(row, col);
        }
        set
        {
            Set(row, col, value);
        }
    }

    public void AddRow(TRow row)
    {
        if (Rows.IndexOf(row) < 0)
        {
            Rows.Add(row);
        }
    }

    public void Sort()
    {
        Rows.Sort();
        Columns.Sort();
    }

    public System.Collections.Generic.IEnumerator<T> GetEnumerator()
    {
        /*foreach (var r in Rows)
            foreach (var c in Columns)
                yield return Data[r][c];*/

        /* foreach (var col in Data)
             foreach (var r in col.Value)
                 yield return r.Value;*/

        return (from col in Data from r in col.Value select r.Value).GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void SaveToFile(string fn)
    {
        StreamWriter sw = new StreamWriter(fn);
        var dt = this;

        dt.Sort();
        sw.Write("\t");
        foreach (string c in dt.Columns)
            sw.Write(c + "\t");
        sw.WriteLine();

        foreach (var r in dt.Rows)
        {
            sw.Write(r + "\t");
            foreach (var c in dt.Columns)
                sw.Write(dt[r, c] + "\t");
            sw.WriteLine();
        }
        sw.Close();

    }

    public DataTable ToDataTable(string RowsColumnName)
    {
        DataTable dt = new DataTable();



        dt.Columns.Add(RowsColumnName, typeof(TRow));
        foreach (var column in this.Columns)
        {
            dt.Columns.Add(column, typeof(T));
        }

        var ind = 0;
        foreach (var row in this.Rows)
        {
            dt.Rows.Add();
            dt.Rows[ind][RowsColumnName] = row;
            foreach (var column in Data[row].Keys)
            {
                dt.Rows[ind][column] = this[row, column];
            }
            ind++;
        }

        return dt;
    }
}
