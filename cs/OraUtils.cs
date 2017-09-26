using System;
using System.Collections.Generic;
using System.Data.OracleClient;


public static class OraUtils
{
    /// <summary>
    /// Получает единственное значение из SQL запроса
    /// </summary>
    /// <param name="connection">Подключение к БД</param>
    /// <param name="sqlString">Строка запроса</param>
    /// <returns>Результирующий массив</returns>
    public static object GetSinglaValue(OracleConnection connection, string sqlString)
    {
        object obj = null;

        using (var selectCommand = new OracleCommand())
        {
            selectCommand.Connection = connection;
            selectCommand.CommandText = sqlString;
            using (OracleDataReader oraRead = selectCommand.ExecuteReader())
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
    /// Получает массив значение первого столбца из запроса
    /// </summary>
    /// <param name="connection">Подключение к БД</param>
    /// <param name="sqlString">Строка запроса</param>
    /// <returns>Результирующий массив</returns>
    public static List<object> GetListValue(OracleConnection connection, string sqlString)
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
    public static List<object> GetListLine(OracleConnection connection, string sqlString, bool nullLine = false)
    {
        return GetListLine<object>(connection, sqlString, nullLine);
    }

    /// <summary>
    /// Получает массив значение первого столбца из запроса
    /// </summary>
    /// <param name="connection">Подключение к БД</param>
    /// <param name="sqlString">Строка запроса</param>
    /// <returns>Результирующий массив</returns>
    public static List<T> GetListValue<T>(OracleConnection connection, string sqlString)
    {
        List<T> obj = new List<T>();

        using (var SelectCommand = new OracleCommand())
        {
            SelectCommand.Connection = connection;
            SelectCommand.CommandText = sqlString;
            using (OracleDataReader oraRead = SelectCommand.ExecuteReader())
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
    public static List<T> GetListLine<T>(OracleConnection connection, string sqlString, bool nullLine = false)
    {
        List<T> obj = new List<T>();

        using (var SelectCommand = new OracleCommand())
        {
            SelectCommand.Connection = connection;
            SelectCommand.CommandText = sqlString;
            using (OracleDataReader oraRead = SelectCommand.ExecuteReader())
            {
                if (oraRead.Read())
                {
                    for (int i = 0; i < oraRead.FieldCount; i++)
                        obj.Add((T)Convert.ChangeType(oraRead[i], typeof(T)));
                }
                else
                {
                    if (nullLine)
                        for (int i = 0; i < oraRead.FieldCount; i++)
                            obj.Add(default(T));
                }
            }
        }

        return obj;
    }
}

