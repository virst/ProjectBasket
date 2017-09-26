using System;
using System.Collections.Generic;
using System.Text;
using LabGenerator;

namespace LabGen17
{
    public static class LabConverot
    {
        public static int[][] Converct(LabGen lg)
        {
            int[][] mass;
            int n = lg.h * lg.w;
            Rcv rcv = new Rcv(lg.w);
            Rcv rcv2 = new Rcv(lg.w);

            mass = new int[n][];
            for (int i = 0; i < n; i++)
            {
                mass[i] = new int[n];
                for (int j = 0; j < n; j++)
                {
                    if (i != j)
                        mass[i][j] = int.MaxValue;
                    else
                        mass[i][j] = 0; // каждый связан с самим собой 
                }
            }

            for(int i=0;i<n;i++)
            {
                rcv.Val = i;

                for(int j=0;j<n;j++)
                {
                    if (EmumMedods.HasFlag(lg.map[rcv.Row][rcv.Col], CellOptions.EXIT_NORTH) &&
                        LabGen.InMazeBorders(rcv.Row - 1, rcv.Col, lg.h, lg.w))
                    {
                        rcv2.SetRC(rcv.Row - 1, rcv.Col);
                        mass[i][rcv2.Val] = 1;
                    }

                    if (EmumMedods.HasFlag(lg.map[rcv.Row][rcv.Col], CellOptions.EXIT_SOUTH) &&
                            LabGen.InMazeBorders(rcv.Row + 1, rcv.Col, lg.h, lg.w))
                    {
                        rcv2.SetRC(rcv.Row + 1, rcv.Col);
                        mass[i][rcv2.Val] = 1;
                    }

                    if (EmumMedods.HasFlag(lg.map[rcv.Row][rcv.Col], CellOptions.EXIT_EAST) &&
                         LabGen.InMazeBorders(rcv.Row , rcv.Col +1, lg.h, lg.w))
                    {
                        rcv2.SetRC(rcv.Row , rcv.Col + 1);
                        mass[i][rcv2.Val] = 1;
                    }

                    if (EmumMedods.HasFlag(lg.map[rcv.Row][rcv.Col], CellOptions.EXIT_WEST) &&
                         LabGen.InMazeBorders(rcv.Row, rcv.Col - 1, lg.h, lg.w))
                    {
                        rcv2.SetRC(rcv.Row, rcv.Col - 1);
                        mass[i][rcv2.Val] = 1;
                    }
                }
            }

            return mass;
        }
    }
}
