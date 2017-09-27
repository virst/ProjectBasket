using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mover
{
    public class KeyMover
    {
        class ImmediateKeys
        {
            public Control Top;
            public Control Bottom;
            public Control Left;
            public Control Right;
            public Control Next;
            public Control Previous;
        }

        readonly Dictionary<Control, ImmediateKeys> _controls = new Dictionary<Control, ImmediateKeys>();

        public void AddContol(Control c)
        {
            _controls.Add(c, new ImmediateKeys());
            c.KeyUp += C_KeyUp;
            c.PreviewKeyDown += C_PreviewKeyDown;
        }

        public int AddSubControls(Control baseControl)
        {
            return AddSubControls<Control>(baseControl);
        }

        public int AddSubControls<T>(Control baseControl) where T : Control
        {
            int c = _controls.Count;

            if (baseControl is T)
                AddContol(baseControl);

            foreach (Control o in baseControl.Controls)
                AddSubControls<T>(o);

            return _controls.Count - c;

        }

        public bool NoTextMode { get; set; } = true;

        private void C_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
                e.IsInputKey = true;
        }

        private void C_KeyUp(object sender, KeyEventArgs e)
        {
            var c = sender as Control;

            switch (e.KeyCode)
            {
                case Keys.Up:
                    _controls[c].Top?.Focus();
                    break;
                case Keys.Down:
                    _controls[c].Bottom?.Focus();
                    break;
                case Keys.Left:
                    if (NoTextMode || (c is TextBox && (c as TextBox).SelectionLength == 0 && (c as TextBox).SelectionStart == 0))
                        _controls[c].Left?.Focus();
                    break;
                case Keys.Right:
                    if (NoTextMode || (c is TextBox && (c as TextBox).SelectionLength == 0 && (c as TextBox).SelectionStart == (c as TextBox).Text.Length))
                        _controls[c].Right?.Focus();
                    break;
                case Keys.Tab:
                    if (e.Shift)
                        _controls[c].Previous?.Focus();
                    else
                        _controls[c].Next?.Focus();
                    e.Handled = true;
                    break;
                default: break;
            }
        }

        static double? PointDist(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));

        }

        public void IntiImmediateKeys()
        {
            var p = new Point(0, 0);

            foreach (var c in _controls.Keys)
            {
                if (_controls[c].Top == null)
                {
                    var cc = _controls.Keys.Where(t => t.PointToScreen(p).Y < c.PointToScreen(p).Y);
                    if (cc.Count() != 0)
                    {
                        var mid = cc.Where(t => t != c).Min(t => PointDist(c.PointToScreen(p), t.PointToScreen(p)));
                        var rp = cc.Where(t => t != c && PointDist(c.PointToScreen(p), t.PointToScreen(p)) == mid).First();
                        _controls[c].Top = rp;
                        if (rp != null)
                            _controls[rp].Bottom = c;
                    }
                }

                if (_controls[c].Bottom == null)
                {
                    var cc = _controls.Keys.Where(t => t.PointToScreen(p).Y > c.PointToScreen(p).Y);
                    if (cc.Count() != 0)
                    {
                        var mid = cc.Where(t => t != c).Min(t => PointDist(c.PointToScreen(p), t.PointToScreen(p)));
                        var rp = cc.Where(t => t != c && PointDist(c.PointToScreen(p), t.PointToScreen(p)) == mid).First();
                        _controls[c].Bottom = rp;
                        if (rp != null)
                            _controls[rp].Top = c;
                    }
                }

                if (_controls[c].Right == null)
                {
                    var cc = _controls.Keys.Where(t => t.PointToScreen(p).X > c.PointToScreen(p).X);
                    if (cc.Count() != 0)
                    {
                        var mid = cc.Where(t => t != c).Min(t => PointDist(c.PointToScreen(p), t.PointToScreen(p)));
                        var rp = cc.Where(t => t != c && PointDist(c.PointToScreen(p), t.PointToScreen(p)) == mid).First();
                        _controls[c].Right = rp;
                        if (rp != null)
                            _controls[rp].Left = c;
                    }
                }

                if (_controls[c].Left == null)
                {
                    var cc = _controls.Keys.Where(t => t.PointToScreen(p).X < c.PointToScreen(p).X);
                    if (cc.Count() != 0)
                    {
                        var mid = cc.Where(t => t != c).Min(t => PointDist(c.PointToScreen(p), t.PointToScreen(p)));
                        var rp = cc.Where(t => t != c && PointDist(c.PointToScreen(p), t.PointToScreen(p)) == mid).First();
                        _controls[c].Left = rp;
                        if (rp != null)
                            _controls[rp].Right = c;
                    }
                }

                if (_controls[c].Next == null)
                {
                    Control rp = null;
                    if (_controls[c].Right != null)
                    {
                        rp = _controls[c].Right;
                    }
                    else
                    {
                        var cc = _controls.Keys.Where(t => t.PointToScreen(p).Y > c.PointToScreen(p).Y);
                        if (cc.Count() != 0)
                        {
                            var mid = cc.Where(t => t != c).Min(t => t.PointToScreen(p).X);
                            rp = cc.Where(t => t != c && t.PointToScreen(p).X == mid).First();
                        }
                    }

                    _controls[c].Next = rp;
                    if (rp != null)
                        _controls[rp].Previous = c;
                }

                if (_controls[c].Previous == null)
                {
                    Control rp = null;
                    if (_controls[c].Left != null)
                    {
                        rp = _controls[c].Right;
                    }
                    else
                    {
                        var cc = _controls.Keys.Where(t => t.PointToScreen(p).Y < c.PointToScreen(p).Y);
                        if (cc.Count() != 0)
                        {
                            var mid = cc.Where(t => t != c).Max(t => t.PointToScreen(p).X);
                            rp = cc.Where(t => t != c && t.PointToScreen(p).X == mid).First();
                        }
                    }

                    _controls[c].Previous = rp;
                    if (rp != null)
                        _controls[rp].Next = c;
                }
            }
        }
    }
}
