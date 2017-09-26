using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;
using System.Web.Security.AntiXss;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using Npgsql;
using NpgsqlTypes;
using static PgUtils;


public class TreeNodeTag : TreeNode
{
    public TreeNodeTag()
        :base()
    {
        Tag = null;
    }

    public TreeNodeTag(string Text)
        : base(Text)
    {
        Tag = null;
    }

    public TreeNodeTag(string Text, object tag)
        : base(Text)
    {
        Tag = tag;
    }

    public object Tag;
}

/// <summary>
/// Сводное описание для ASP_Utils
/// </summary>
public static class ASP_Utils
{
    /// <summary>
    /// Найти контрол 
    /// </summary>
    /// <param name="c">Где искать</param>
    /// <param name="n">ID искомого контрола</param>
    /// <returns></returns>
    public static Control FindControl(Control c, string n)
    {
        var r = c.FindControl(n);
        if (r != null)
            return r;

        foreach (Control o in c.Controls)
        {
            r = FindControl(o, n);
            if (r != null)
                return r;
        }

        return null;
    }

    public static void OpenTrByAdr(TreeView tv, string[] adr)
    {
        var nc = tv.Nodes;

        foreach (string o in adr)
        {
            nc = selinn(nc, o);
        }
    }

    static TreeNodeCollection selinn(TreeNodeCollection c, string name)
    {
        foreach (TreeNode o in c)
        {
            if (o.Text == name)
            {
                o.Selected = true;
                o.Expanded = true;
                return o.ChildNodes;
            }
        }

        return null;
    }

    /// <summary>
    /// Заполнить дерево из запроса
    /// </summary>
    /// <param name="conn">Соединение с БД</param>
    /// <param name="sql">Запрос для формирования дерева</param>
    /// <param name="tv">Дерево</param>
    /// <param name="splitter">Разделитель</param>
    public static void SqlToTreeView(DbConnection conn, string sql, TreeView tv, char? splitter = null)
    {
        if (conn.State != ConnectionState.Open)
            conn.Open();

        tv.Nodes.Clear();

        using (var com = conn.CreateCommand())
        {
            com.CommandText = sql;
            com.CommandType = CommandType.Text;

            using (var reader = com.ExecuteReader())
            {
                string[] tmpSS = new string[reader.FieldCount];

                while (reader.Read())
                {
                    var curNot = tv.Nodes;

                    for (int i = 0; i < tmpSS.Length; i++)
                    {
                        if (tmpSS[i] != reader[i].ToString())
                        {
                            tmpSS[i] = reader[i].ToString();
                            for (int j = i + 1; j < tmpSS.Length; j++)
                                tmpSS[j] = null;

                            if(splitter == null)
                                curNot.Add(new TreeNode(tmpSS[i]));
                            else
                            {
                                string tag = null;

                                var ss = tmpSS[i].Split(splitter.Value);
                                var text = ss[0];
                                if (ss.Length > 1)
                                    tag = ss[1];

                                curNot.Add(new TreeNode(text, tag));
                            }
                        }
                        curNot = NodByName(curNot, tmpSS[i]);
                    }
                }
            }
        }

        conn.Close();
    }

    /// <summary>
    /// Получить ноду по имени
    /// </summary>
    /// <param name="c">Коллеция Нод</param>
    /// <param name="name">Искомое наименование</param>
    /// <returns>найденная нода</returns>
    static TreeNodeCollection NodByName(TreeNodeCollection c, string name)
    {
        return (from TreeNode n in c where n.Text == name select n.ChildNodes).FirstOrDefault();
    }

    

    public static TreeNode HParent(TreeNode t)
    {
        TreeNode r = null;
        while (t.Parent != null)
        {
            r = t.Parent;
            t = r;
        }
        return r;

    }


    static readonly string[] months = new string[] { "", "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь" };

    /// <summary>
    /// Получить таблицу с месяцами
    /// </summary>
    /// <returns>Таблица с месяцами</returns>
    public static DataTable GetMonthsTable()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("id", typeof(int));
        dt.Columns.Add("months", typeof(string));

        for (int i = 0; i < months.Length; i++)
        {
            var r = dt.Rows.Add();
            r[0] = i;
            r[1] = months[i];
        }

        return dt;
    }

    /// <summary>
    /// Получить таблицу с годами
    /// </summary>
    /// <returns>Таблица с годами</returns>
    public static DataTable GetYearTable()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("year", typeof(string));

        dt.Rows.Add();
        dt.Rows[0][0] = "";

        foreach (var s in yList())
        {
            var r = dt.Rows.Add();
            r[0] = s;
        }

        return dt;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static DataTable GetYearTable2(int a,int b)
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("year", typeof(string));

        dt.Rows.Add();
        dt.Rows[0][0] = "";

        foreach (var s in yList(a,b))
        {
            var r = dt.Rows.Add();
            r[0] = s;
        }

        return dt;
    }

    /// <summary>
    /// Получить набор годов
    /// </summary>
    /// <param name="n1">Год с</param>
    /// <param name="n2">Год по</param>
    /// <returns>Список годов</returns>
    public static List<string> yList(int n1, int n2)
    {
        List<string> ret = new List<string>();
        for (int i = n2; i >= n1; i--)
        {
            ret.Add(i.ToString());
        }
        return ret;
    }

    /// <summary>
    /// Получить набор Годов от 1940 до текущего
    /// </summary>
    /// <returns>набор годов</returns>
    public static List<string> yList()
    {
        return yList(1940, DateTime.Now.Year + 1);
    }

    /// <summary>
    /// Копирование коллекции нод
    /// </summary>
    /// <param name="sourcCollection">Коллекция из </param>
    /// <param name="innerCollection">Коллекция в</param>
    public static void Collapsed2Collapsed(TreeNodeCollection sourcCollection, TreeNodeCollection innerCollection)
    {
        for (int i = 0; i < sourcCollection.Count && i < innerCollection.Count; i++)
        {
            innerCollection[i].Expanded = sourcCollection[i].Expanded;
            innerCollection[i].Selected = sourcCollection[i].Selected;
            Collapsed2Collapsed(sourcCollection[i].ChildNodes,
                innerCollection[i].ChildNodes);
        }
    }

    /// <summary>
    /// Получить имя страници
    /// </summary>
    /// <returns>имя страници</returns>
    public static string GetPageName()
    {
        string path = System.Web.HttpContext.Current.Request.Url.AbsolutePath;
        System.IO.FileInfo fi = new System.IO.FileInfo(path);
        return fi.Name;
    }

    /// <summary>
    /// Получить наименование полное страници
    /// </summary>
    /// <returns>Полное имя страници</returns>
    public static string GetPageFullName()
    {
        string path = System.Web.HttpContext.Current.Request.Url.AbsolutePath;
        System.IO.FileInfo fi = new System.IO.FileInfo(path);
        return fi.FullName;
    }

    /// <summary>
    /// Получить все вложенные элименты
    /// </summary>
    /// <param name="cont">Где искать</param>
    /// <returns></returns>
    public static List<Control> GetAllControls(Control cont)
    {
        List<Control> cc = new List<Control>();

        foreach (Control c in cont.Controls)
        {
            cc.Add(c);
            cc.AddRange(GetAllControls(c));
        }

        return cc;
    }

    /// <summary>
    /// Получить все вложенные элементы
    /// </summary>
    /// <typeparam name="T">Тип контрола</typeparam>
    /// <param name="cont">Где искать</param>
    /// <returns></returns>
    public static List<T> GetAllControls<T>(Control cont) where T : Control
    {
        return GetAllControls(cont).OfType<T>().Select(c => c as T).ToList();
    }

    /// <summary>
    /// Поиск в дереве
    /// </summary>
    /// <param name="tr">Дерево</param>
    /// <param name="txt">текст для поиска</param>
    /// <returns></returns>
    public static bool SeachInTree(TreeView tr, string txt)
    {
        if (tr.Nodes.Count == 0)
            return false;

        bool StartAtNull = tr.SelectedNode == null;

        if (tr.SelectedNode == null)
            tr.Nodes[0].Selected = true;

        var startNode = tr.SelectedNode;

        nextNode(tr);

        if (tr.SelectedNode != null && tr.SelectedNode.Text.ToUpper().IndexOf(txt.ToUpper()) > -1)
            return true;

        if (tr.SelectedNode != startNode)
            return SeachInTree(tr, txt);
        else
            return false;
    }

    /// <summary>
    /// Переместится на следующуб ноду в дереве
    /// </summary>
    /// <param name="tr">Девево</param>
    public static void nextNode(TreeView tr)
    {
        var nd = nextNode(tr.SelectedNode, tr);        
        nd.Selected = true;        
        nd.Expanded = true;
    }

    /// <summary>
    /// Получить следующую ноду
    /// </summary>
    /// <param name="node">Нода</param>
    /// <param name="tr">Колекция нодов</param>
    /// <param name="appLevel"></param>
    /// <returns></returns>
    public static TreeNode nextNode(TreeNode node, TreeView tr, bool appLevel = false)
    {
        if (node == null)
            return tr.Nodes[0];

        if (!appLevel && node.ChildNodes.Count != 0)
            return node.ChildNodes[0];

        var parCol = node.Parent?.ChildNodes ?? tr.Nodes;

        int n = NodeNum(node, parCol);

        if (n < parCol.Count - 1)
            return parCol[n + 1];

        if (node.Parent == null)
            return parCol[0];

        return nextNode(node.Parent, tr, true);
    }

    /// <summary>
    /// Полечить номер ноды
    /// </summary>
    /// <param name="node">Нода</param>
    /// <param name="col">Коллекция с нодами</param>
    /// <returns></returns>
    public static int NodeNum(TreeNode node, TreeNodeCollection col)
    {
        for (int i = 0; i < col.Count; i++)
            if (col[i] == node)
                return i;

        return -1;
    }

    /// <summary>
    /// Вывод текстового сообщения
    /// </summary>
    /// <param name="msg">Текст сообщения</param>
    /// <param name="p">Страница</param>
    public static void ShowMessage(string msg, Page p)
    {
        ScriptManager.RegisterClientScriptBlock(p, p.GetType(), "alertmsg", "setTimeout('alert(\"" + msg + "\")', 100);", true);
    }
}