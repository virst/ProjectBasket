/*             ---PostgreSQL---
__________________$$$$$$$$$
_______________$$$$$$$$$$$$$$$$$$$$
___$$$$$$______$$_______________$$$$$$$$$$
___$$$_$$$____$$_____________$$________$$$$$$
_____$$_$$____$$_$____$$______$$$___________$$$$
____$$$_$$____$$_$____$$_________$$$_________$$$$$$$$
__$$$$_$$____$$___________________$$$________$$_$$$$$$
_$$$__$$_____$$____________________$$________$$__$$
$$$__$$______$_________$$_________$$_________$$
_$$__$$_____$$_________$$$_______$$__________$$
_$$___$$$$$$$____$$$_____$$___$$$$$__________$$
__$$___$$$$$___$$$$_______$$$$$$$____________$$
___$$$______$$$$_$$$$________________________$$
____$$$$$$$$$$_____$$_________________$$$____$$
___________________$$__$$$____$$$$$$__$$$$___$$
__________________$$___$$$$__$$$$$$$__$$_$___$$
___________________$$$$$_$$__$$___$$$$$$_$$__$$
____________________$$$$_$$$$$$____$$$$$_$$$$$$
*/



using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Web.Configuration;
using Npgsql;
using NpgsqlTypes;

/// <summary>
/// Реализует методы для удобного взаимодействия с БД PostgreSQL
/// </summary>
public static class PgUtils
{
    /// <summary>
    /// Получает единственное значение из SQL запроса
    /// </summary>
    /// <param name="connection">Подключение к БД</param>
    /// <param name="sqlString">Строка запроса</param>
    /// <param name="parSql">Массив параметров</param>
    /// <returns>Результирующий массив</returns>
    public static object GetSinglaValue(NpgsqlConnection connection, string sqlString, NpgsqlParameter[] parSql = null)
    {
        object obj = null;

        using (var selectCommand = new NpgsqlCommand())
        {
            selectCommand.Connection = connection;
            selectCommand.CommandText = sqlString;
            if (parSql != null)
                selectCommand.Parameters.AddRange(parSql);

            using (var oraRead = selectCommand.ExecuteReader())
            {
                if (oraRead.Read())
                {
                    obj = oraRead[0];
                }
            }
        }

        return obj;
    }

    /// <summary>
    /// Получает Reader из запроса
    /// </summary>
    /// <param name="connection">Подключение к БД</param>
    /// <param name="sqlString">Строка запроса</param>
    /// <returns>Reader</returns>
    public static NpgsqlDataReader GetReader(NpgsqlConnection connection, string sqlString)
    {
        var selectCommand = new NpgsqlCommand
        {
            Connection = connection,
            CommandText = sqlString
        };

        return selectCommand.ExecuteReader();
    }

    /// <summary>
    /// Выполняет запрос
    /// </summary>
    /// <param name="connection">Подключение к БД</param>
    /// <param name="sqlString">Строка запроса</param>
    /// <param name="parSql">Массив параметров</param>
    /// <returns>Количество затронутых строк</returns>
    public static int ExecuteNonQuery(NpgsqlConnection connection, string sqlString, NpgsqlParameter[] parSql = null)
    {
        var selectCommand = new NpgsqlCommand
        {
            Connection = connection,
            CommandText = sqlString
        };

        if (parSql != null)
            selectCommand.Parameters.AddRange(parSql);

        return selectCommand.ExecuteNonQuery();
    }

    /// <summary>
    /// Выполняет запрос
    /// </summary>
    /// <param name="sqlString">Строка запроса</param>
    /// <param name="parSql">Массив параметров</param>
    /// <returns>Количество затронутых строк</returns>
    public static int ExecuteNonQuery(string sqlString, NpgsqlParameter[] parSql = null)
    {
        var c = GetDefaultConnection();
        try
        {
            return parSql != null ? ExecuteNonQuery(c, sqlString, parSql) : ExecuteNonQuery(c, sqlString);
        }
        finally
        {
            c.Close();
        }
    }

    /// <summary>
    /// Получает массив значение первого столбца из запроса
    /// </summary>
    /// <param name="connection">Подключение к БД</param>
    /// <param name="sqlString">Строка запроса</param>
    /// <returns>Результирующий массив</returns>
    public static List<object> GetListValue(NpgsqlConnection connection, string sqlString)
    {
        return GetListValue<object>(connection, sqlString);
    }

    /// <summary>
    /// Получает массив значение первой строки из запроса
    /// </summary>
    /// <param name="connection">Подключение к БД</param>
    /// <param name="sqlString">Строка запроса</param>
    /// <param name="nullLine">Вернуть массив пустых значение если запрос ничего не вернул</param>
    /// <returns>Результирующий массив</returns>
    public static List<object> GetListLine(NpgsqlConnection connection, string sqlString, bool nullLine = false)
    {
        return GetListLine<object>(connection, sqlString, nullLine);
    }

    /// <summary>
    /// Получает массив значение первого столбца из запроса
    /// </summary>
    /// <param name="connection">Подключение к БД</param>
    /// <param name="sqlString">Строка запроса</param>
    /// <returns>Результирующий массив</returns>
    public static List<T> GetListValue<T>(NpgsqlConnection connection, string sqlString)
    {
        var obj = new List<T>();

        using (var selectCommand = new NpgsqlCommand())
        {
            selectCommand.Connection = connection;
            selectCommand.CommandText = sqlString;
            using (var oraRead = selectCommand.ExecuteReader())
            {
                while (oraRead.Read())
                {
                    obj.Add((T)Convert.ChangeType(oraRead[0], typeof(T)));
                }
            }
        }

        return obj;
    }

    /// <summary>
    /// Получает массив значение первой строки из запроса
    /// </summary>
    /// <param name="connection">Подключение к БД</param>
    /// <param name="sqlString">Строка запроса</param>
    /// <param name="nullLine">Вернуть массив пустых значение если запрос ничего не вернул</param>
    /// <returns>Результирующий массив</returns>
    public static List<T> GetListLine<T>(NpgsqlConnection connection, string sqlString, bool nullLine = false)
    {
        var obj = new List<T>();

        using (var selectCommand = new NpgsqlCommand())
        {
            selectCommand.Connection = connection;
            selectCommand.CommandText = sqlString;
            using (var oraRead = selectCommand.ExecuteReader())
            {
                if (oraRead.Read())
                {
                    for (var i = 0; i < oraRead.FieldCount; i++)
                        obj.Add((T)Convert.ChangeType(oraRead[i], typeof(T)));
                }
                else
                {
                    if (nullLine)
                        for (var i = 0; i < oraRead.FieldCount; i++)
                            obj.Add(default(T));
                }
            }
        }

        return obj;
    }

    public static NpgsqlConnection GetDefaultConnection(bool open = true)
    {
        var cco = new NpgsqlConnection(WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        if (open)
            cco.Open();
        return cco;
    }

    /// <summary>
    /// Получает единственное значение из SQL запроса
    /// </summary>
    /// <param name="sqlString">Строка запроса</param>
    /// <returns>Результирующий массив</returns>
    public static object GetSinglaValue(string sqlString, NpgsqlParameter[] parSql = null)
    {
        var c = GetDefaultConnection();
        try
        {
            return parSql != null ? GetSinglaValue(c, sqlString, parSql) : GetSinglaValue(c, sqlString);            
        }
        finally
        {
            c.Close();
        }
    }

    /// <summary>
    /// Получает массив значение первого столбца из запроса
    /// </summary>

    /// <param name="sqlString">Строка запроса</param>
    /// <returns>Результирующий массив</returns>
    public static List<object> GetListValue(string sqlString)
    {
        var c = GetDefaultConnection();
        try
        {
            return GetListValue(c, sqlString);
        }
        finally
        {
            c.Close();
        }
    }

    /// <summary>
    /// Получает массив значение первой строки из запроса
    /// </summary>
    /// <param name="sqlString">Строка запроса</param>
    /// <param name="nullLine">Вернуть массив пустых значение если запрос ничего не вернул</param>
    /// <returns>Результирующий массив</returns>
    public static List<object> GetListLine(string sqlString, bool nullLine = false)
    {
        var c = GetDefaultConnection();
        try
        {
            return GetListLine(c, sqlString, nullLine);
        }
        finally
        {
            c.Close();
        }
    }

    /// <summary>
    /// Получает массив значение первого столбца из запроса
    /// </summary>
    /// <param name="sqlString">Строка запроса</param>
    /// <returns>Результирующий массив</returns>
    public static List<T> GetListValue<T>(string sqlString)
    {
        var c = GetDefaultConnection();
        try
        {
            return GetListValue<T>(c, sqlString);
        }
        finally
        {
            c.Close();
        }
    }

    /// <summary>
    /// Получает массив значение первой строки из запроса
    /// </summary>
    /// <param name="sqlString">Строка запроса</param>
    /// <param name="nullLine">Вернуть массив пустых значение если запрос ничего не вернул</param>
    /// <returns>Результирующий массив</returns>
    public static List<T> GetListLine<T>(string sqlString, bool nullLine = false)
    {
        var c = GetDefaultConnection();
        try
        {
            return GetListLine<T>(c, sqlString, nullLine);
        }
        finally
        {
            c.Close();
        }
    }

    /// <summary>
    /// Выполняет запрос
    /// </summary>
    /// <param name="strSql">Запрос</param>
    /// <param name="parSql">Параметры запроса</param>
    /// <param name="close">Закрывать соединение</param>
    /// <param name="reder">Получить данные</param>
    /// <returns>NpgsqlDataReader</returns>
    public static NpgsqlDataReader pgExecute(string strSql, NpgsqlParameter[] parSql = null, bool close = true, bool reder = true)
    {
        var comm = new NpgsqlCommand
        {
            Connection = GetDefaultConnection(),
            CommandText = strSql
        };
        try
        {
            //NpgsqlParameter[] ff = {new NpgsqlParameter("id", NpgsqlDbType.Integer){Value = 10} };

            if (parSql != null)
                comm.Parameters.AddRange(parSql);

            if (reder)
                return comm.ExecuteReader();
            else
            {
                comm.ExecuteNonQuery();
                return null;
            }
        }
        catch (Exception)
        {
            return null;
        }
        finally
        {
            if (close)
                comm.Connection.Close();
        }
    }

}