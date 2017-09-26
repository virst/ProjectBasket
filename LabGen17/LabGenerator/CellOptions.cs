using System;
using System.Collections.Generic;
using System.Text;

namespace LabGenerator
{
    [Flags]
    public enum CellOptions
    {
        EXIT_EAST = 0x01, //есть проход на восток

        EXIT_NORTH = 0x02, //есть проход на север

        EXIT_WEST = 0x04, //есть проход на запад

        EXIT_SOUTH = 0x08, //есть проход на юг

        CELL_FRONTIER = 0x20, //граничная

        CELL_VISITED = 0x10,  // посещенная 

        NONE = 0x00  // пусто

    }

    public static class EmumMedods
    {
        public static void AddFlag(ref CellOptions e, CellOptions v)
        {
            e |= v;
        }

        public static void RemoveFlag(ref CellOptions e, CellOptions v)
        {
            e &= ~v;
        }

        public static bool HasFlag(CellOptions e, CellOptions v)
        {
            return (e & v) == v;
        }

    }
}
