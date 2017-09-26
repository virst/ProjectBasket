using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using LabGenerator;
using DijkstraAlgorithm;

namespace LabGen17
{
    public partial class Form1 : Form
    {
        LabGen lg;

        int lineWidth, cellWidth;
        SolidBrush b ;
        Pen p ;
        Pen WayPen;
        CellData selectedCell;
        CellData finCell;
        SolidBrush selCellBr;
        SolidBrush finCellBr;
        DijkstraRez dr;
        int[][] mas;
        Rcv rcv;
        
        public Form1()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            if (lg == null)
                return;
            GraphicsPath GP = new GraphicsPath();

            e.Graphics.FillRectangle(b, 0, 0, lg.w * cellWidth, lg.h * cellWidth);
            if (selectedCell != null)
                e.Graphics.FillRectangle(selCellBr, selectedCell.GetC() * cellWidth + lineWidth
                    , selectedCell.GetR() * cellWidth + lineWidth,
                    cellWidth - lineWidth*2, cellWidth - lineWidth*2);
            if (finCell != null)
                e.Graphics.FillRectangle(finCellBr, finCell.GetC() * cellWidth + lineWidth
                    , finCell.GetR() * cellWidth + lineWidth,
                    cellWidth - lineWidth * 2, cellWidth - lineWidth * 2);

            for (int i = 0; i < lg.h; i++)
            {
                for (int j = 0; j < lg.w; j++)
                {
                    GP.StartFigure();
                    if (!EmumMedods.HasFlag(lg.map[i][j], CellOptions.EXIT_NORTH))
                        GP.AddLine(j * cellWidth, i * cellWidth,
                            (j + 1) * cellWidth, i * cellWidth);
                    GP.StartFigure();
                    if (!EmumMedods.HasFlag(lg.map[i][j], CellOptions.EXIT_EAST))
                        GP.AddLine((j + 1) * cellWidth, i * cellWidth,
                            (j + 1) * cellWidth, (i+1) * cellWidth);
                    GP.StartFigure();
                    if (!EmumMedods.HasFlag(lg.map[i][j], CellOptions.EXIT_SOUTH))
                        GP.AddLine((j + 1) * cellWidth, (i + 1) * cellWidth,
                            j  * cellWidth, (i+1) * cellWidth);
                    GP.StartFigure();
                    if (!EmumMedods.HasFlag(lg.map[i][j], CellOptions.EXIT_WEST))
                        GP.AddLine(j * cellWidth, i * cellWidth,
                            j * cellWidth, (i + 1) * cellWidth);
                }
            }

            e.Graphics.DrawPath(p, GP);

            if(selectedCell != null && finCell != null)
                DrawWay(e);
        }

        private void DrawWay(PaintEventArgs e)
        {
            Rcv r1 = new Rcv(lg.w), r2 = new Rcv(lg.w);
            
            r1.SetRC(selectedCell.GetR(), selectedCell.GetC());
            int start = r1.Val ;
            r2.SetRC(finCell.GetR(), finCell.GetC());
            int wayTo = r2.Val;

            if (wayTo > -1 && start > -1 && dr.dist[wayTo] != int.MaxValue)
            {
                int now = wayTo;
                int next;

                while (now != start)
                {
                    next = dr.parent[now];
                    //Pen p = new Pen(Color.Red, 4);
                    r1.Val = now;
                    r2.Val = next;

                    e.Graphics.DrawLine(WayPen, r1.Col * cellWidth + cellWidth / 2, r1.Row * cellWidth + cellWidth / 2,
                        r2.Col * cellWidth + cellWidth / 2, r2.Row * cellWidth + cellWidth / 2);
                    now = next;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            selectedCell = null;
            finCell = null;
            
            lg = new LabGen((int)nudH.Value ,(int)nudW.Value );
            lg.Generate();
            mas = LabConverot.Converct(lg);
            rcv = new Rcv(lg.w);

            lineWidth = 2;
            cellWidth = 20;
            b = new SolidBrush(Color.Aqua);
            p = new Pen(Color.Blue, lineWidth);
            WayPen = new Pen(Color.Red, lineWidth); 
            selCellBr = new SolidBrush(Color.Orange);
            finCellBr = new SolidBrush(Color.Green);

            panel1.Refresh();
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            if (cellWidth == 0)
                return;
            if (selectedCell == null)
            {
                selectedCell = new CellData(e.Y / cellWidth, e.X / cellWidth);
                Dijkstra t = new Dijkstra(mas);
                rcv.SetRC(selectedCell.GetR(), selectedCell.GetC());
                dr = t.GetDijkstraRez(rcv.Val);

            }
            else
            {
                finCell = new CellData(e.Y / cellWidth, e.X / cellWidth);
            }
            panel1.Refresh();
            this.Text = selectedCell.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            
            for (int i = 0; i < dr.dist.Length; i++)
                listBox1.Items.Add(i.ToString() + " - " + dr.dist[i].ToString());

            for (int i = 0; i < dr.parent.Length; i++)
                listBox2.Items.Add(i.ToString() + " - " + dr.parent[i].ToString());

            
        }        

        private void panel1_DoubleClick(object sender, EventArgs e)
        {
            if (MessageBox.Show("Удалить точки ?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                selectedCell = null;
                finCell = null;
                panel1.Refresh();
            }
        }
    }
}
