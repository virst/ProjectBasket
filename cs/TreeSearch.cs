using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
// using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nedra_app.Utils
{
    public class TreeSearch
    {
        private TextBox tbSearc;
        private TreeView treeView1;
        private string lastSearchText = null;
        private TreeNode[] findedNodes = new TreeNode[0];
        private int curFindNode = 0;

        public string NoRezult = "Поиск недал результатов ";
        public string SearchEnd = "Поиск завершен ";

        public TreeSearch(TreeView tv, TextBox tb)
        {
            tbSearc = tb;
            treeView1 = tv;

            tb.KeyDown += Tb_KeyDown;
            treeView1.KeyDown += Tb_KeyDown;
        }

        private void Tb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                e.Handled = true;
            }


            if (e.KeyData == Keys.Delete)
                tbSearc.Text = "";

            if (e.KeyData != Keys.Enter || tbSearc.Text == "")
                return;

            if (lastSearchText != tbSearc.Text)
            {
                lastSearchText = tbSearc.Text;
                findedNodes = FindNodes(lastSearchText);
                curFindNode = -1;
            }

            if (findedNodes.Length < 1)
            {
                MessageBox.Show(NoRezult);
                return;
            }

            curFindNode++;
            if (findedNodes.Length == curFindNode)
            {
                MessageBox.Show(SearchEnd);
                curFindNode = -1;
                return;
            }


            treeView1.SelectedNode = findedNodes[curFindNode];
            treeView1.Focus();
        }



        TreeNode[] FindNodes(string txt, TreeNode tn = null)
        {
            List<TreeNode> ret = new List<TreeNode>();
            TreeNodeCollection tnc;
            if (tn == null)
                tnc = treeView1.Nodes;
            else
                tnc = tn.Nodes;

            foreach (TreeNode o in tnc)
            {
                if (o.Text.ToLower().Contains(txt.ToLower()))
                    ret.Add(o);

                ret.AddRange(FindNodes(txt, o));
            }

            return ret.ToArray();
        }
    }
}
