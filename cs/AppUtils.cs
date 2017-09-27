using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
// using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nedra_app.Utils
{
    public static class AppUtils
    {
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
                            string nm = tmpSS[i];

                            if (splitter != null && nm != null && nm.IndexOf(splitter.ToString()) > 0)
                                nm = nm.Split(splitter.Value)[0];

                            if (tmpSS[i] != reader[i].ToString())
                            {
                                tmpSS[i] = reader[i].ToString();
                                for (int j = i + 1; j < tmpSS.Length; j++)
                                    tmpSS[j] = null;

                                nm = tmpSS[i];
                                if (splitter == null)
                                    curNot.Add(new TreeNode(tmpSS[i]));
                                else
                                {
                                    string tag = "";

                                    var ss = tmpSS[i].Split(splitter.Value);
                                    var text = ss[0];
                                    for (int j = 1; j < ss.Length; j++)
                                        tag += ss[j] + splitter.Value;

                                    tag = tag.TrimEnd(splitter.Value);

                                    nm = text;
                                    curNot.Add(new TreeNode(text) { Tag = tag });
                                }
                            }
                            curNot = NodByName(curNot, nm);
                        }
                    }
                }
            }

            //  conn.Close();

            //  HideNodes(tv);
            RemNodes(tv);
        }

        /// <summary>
        /// Получить ноду по имени
        /// </summary>
        /// <param name="c">Коллеция Нод</param>
        /// <param name="name">Искомое наименование</param>
        /// <returns>найденная нода</returns>
        static TreeNodeCollection NodByName(TreeNodeCollection c, string name)
        {
            foreach (TreeNode n in c)
            {
                if (n.Text == name)
                {
                    TreeNodeCollection collection = n.Nodes;
                    return collection;
                }
            }
            return null;
        }

        public static List<TreeNode> GetAllNodes(TreeNodeCollection c)
        {
            List<TreeNode> ss = new List<TreeNode>();
            foreach (TreeNode n in c)
            {
                ss.Add(n);
                ss.AddRange(GetAllNodes(n.Nodes ));
            }
            return ss;
        }

        private static void RemNodes(TreeView tn, string txt = "Пусто")
        {
            RemNodes(tn.Nodes, txt);
        }

        private static void RemNodes(TreeNodeCollection c, string txt)
        {
            foreach (TreeNode n in c)
            {
                if (n != null)
                    if (string.IsNullOrWhiteSpace(n.Text))
                    {
                        n.Text = txt;
                        HideNodes(n.Nodes);
                    }
                    else
                        RemNodes(n.Nodes, txt);
            }
        }

        private static void HideNodes(TreeView tn)
        {
            HideNodes(tn.Nodes);
        }

        private static void HideNodes(TreeNodeCollection c)
        {
            foreach (TreeNode n in c)
            {
                if (n != null)
                    if (string.IsNullOrWhiteSpace(n.Text))
                        c.Remove(n);
                    else
                        HideNodes(n.Nodes);
            }
        }

        public static IEnumerable<Control> GetAllControls2(Control control)
        {
            return GetAllControls2<Control>(control);
        }

        public static IEnumerable<T> GetAllControls2<T>(Control control) where T : Control
        {
            List<T> cc = new List<T>();

            foreach (Control o in control.Controls)
            {
                if (o is T)
                    cc.Add(o as T);
                cc.AddRange(GetAllControls2<T>(o));
            }


            return cc;
        }

        public static IEnumerable<Control> GetAllControls(Control control, Type type)
        {
            var controls = control.Controls.Cast<Control>();
            var enumerable = controls as Control[] ?? controls.ToArray();
            return enumerable.SelectMany(ctrl => GetAllControls(ctrl, type))
                .Concat(enumerable)
                .Where(c => c.GetType() == type);
        }

        public static DataTable GetYearsDT()
        {
            //  List<string> yearDataSource = Enumerable.Range(1950, DateTime.Now.Year - 1948).OrderByDescending(i => i).ToList().ConvertAll<string>(delegate (int k) { return k.ToString(); });
            DataTable dt = new DataTable("years");
            dt.Columns.Add("years", typeof(string));
            dt.Rows.Add();
            dt.Rows[0][0] = "";
            for (int i = DateTime.Now.Year + 1; i >= 1948; i--)
            {
                var r = dt.Rows.Add();
                r[0] = i;
            }

            return dt;
        }

        public static void RowSetValue(this DataRow r,string f, object v)
        {
            if (r != null)
                r[f] = v;
        }

        public static void RowSetValue(this DataRow r, int f, object v)
        {
            if (r != null)
                r[f] = v;
        }
        
    }
}
