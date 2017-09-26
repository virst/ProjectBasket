using System;
using System.Collections.Generic;
using System.Text;


namespace LabGenerator
{
    

    public class CellData
    {
        int row;
        int col;   

        public CellData(int r, int c)
        {            
            row = r;
            col = c;
        }

        public int GetR() { return row; }
        public int GetC() { return col; }

        public override string ToString() 
        {
            return "Row = " + row.ToString() + "; Col = " + col.ToString();
        }
    }


    public class LabGen
    {
        public int w;
        public int h;

        public CellOptions[][] map;

        private int[] dr = { -1, 0, 1, 0 };
        private int[] dc = { 0, 1, 0, -1 };
        private Random rnd;
        private List<CellData> CellList;

        public LabGen(int H, int W)
        {
            w = W; h = H;
            map = new CellOptions[h][];
            for (int i = 0; i < h; i++)
            {
                map[i] = new CellOptions[w];
                for (int j = 0; j < w; j++)
                    map[i][j] = CellOptions.NONE;
            }
            rnd = new Random();
            CellList = new List<CellData>();

        }

        private Double random()
        {
            return rnd.NextDouble();
        }

        public int MazeHeight { get { return h; } }

        public int MazeWidth { get { return w; } }

        public static bool InMazeBorders(int r, int c, int Height, int Width) //– возвращает значение true, если ячейка заданная своими координатами находится в границах лабиринта.
        {
            return r >= 0 && r < Height && c >= 0 && c < Width;
        }

        private void RemoveFrontierCell(int r, int c)  //– удаляет из списка обрабатываемых вершин элемент, заданный номером строки и столбца
        {
            EmumMedods.RemoveFlag(ref map[r][c], CellOptions.CELL_FRONTIER);

            for (int i = 0; i < CellList.Count; i++)
            {
                if (CellList[i].GetC() == c && CellList[i].GetR() == r)
                    CellList.RemoveAt(i);
            }
        }

        private void RemoveWall(int r, int c, int d) //  – удаляет стенку в карте лабиринта в указанной ячейке и указанном направлении, а также в смежной ячейке в противоположном направлени
        {
            switch (d)
            {
                case 0:
                    EmumMedods.AddFlag(ref map[r][c], CellOptions.EXIT_NORTH);
                    if (InMazeBorders(r - 1, c, h, w))
                        EmumMedods.AddFlag(ref map[r - 1][c], CellOptions.EXIT_SOUTH);
                    break;
                case 1:
                    EmumMedods.AddFlag(ref map[r][c], CellOptions.EXIT_EAST);
                    if (InMazeBorders(r, c + 1, h, w))
                        EmumMedods.AddFlag(ref map[r][c + 1], CellOptions.EXIT_WEST);
                    break;
                case 2:
                    EmumMedods.AddFlag(ref map[r][c], CellOptions.EXIT_SOUTH);
                    if (InMazeBorders(r + 1, c, h, w))
                        EmumMedods.AddFlag(ref map[r + 1][c], CellOptions.EXIT_NORTH);
                    break;
                case 3:
                    EmumMedods.AddFlag(ref map[r][c], CellOptions.EXIT_WEST);
                    if (InMazeBorders(r, c - 1, h, w))
                        EmumMedods.AddFlag(ref map[r][c - 1], CellOptions.EXIT_EAST);
                    break;

            }
        }

        private bool Has_Frontier()
        {
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    if (EmumMedods.HasFlag(map[i][j], CellOptions.CELL_FRONTIER))
                        return true;
                }
            }
            return false;
        }

        private void MarkFrontierCells(int row, int col)
        {
            int i, r, c;
            //перебираем все 4 направления
            for (i = 0; i < 4; i++)
            {
                r = row + dr[i];
                c = col + dc[i];
               /* if (InMazeBorders(r, c, MazeHeight, MazeWidth) &&
                   ((map[r][c] & CellOptions.CELL_VISITED) == 0x00) &&
                   ((map[r][c] & CellOptions.CELL_FRONTIER) == 0x00))*/
                if (InMazeBorders(r, c, MazeHeight, MazeWidth) &&
                   (!EmumMedods.HasFlag(map[r][c],CellOptions.CELL_VISITED)) &&
                   (!EmumMedods.HasFlag(map[r][c],CellOptions.CELL_FRONTIER)) )
                { //если ячейка в границах лабиринта и
                    //не является посещенной или граничной

                    //то помечаем ее как граничную
                    map[r][c] |= CellOptions.CELL_FRONTIER;

                    //добавляем ее в список граничных
                    CellList.Add(new CellData(r, c));
                    /*var tmp:Cell=new Cell(r, c);
                    CellList.push(tmp);*/
                }
            }
        }

        //добавление ячейки к пути, в напрравлении dir
        private void AttachCellToTree(int row, int col, int dir)
        {
            int i, r, c, dirOffset, direct;
            RemoveFrontierCell(row, col); //удаление из списка граничных
            map[row][col] |= CellOptions.CELL_VISITED; //пометка ячейки как посещенной

            if (dir == -1)
            { //если направление не выбрано, то

                //ищем случаное возможное направление
                dirOffset = (int)(random() * 4);

                //перебираем возможные направления
                for (i = 0; i < 4; i++)
                {
                    direct = (dirOffset + i) % 4;
                    r = row + dr[direct];
                    c = col + dc[direct];
                    if (InMazeBorders(r, c, MazeHeight, MazeWidth) &&
                       ((map[r][c] & CellOptions.CELL_VISITED) != 0x00))
                    { //если не посещено и в пределах лабиринта, то

                        // удаляем стенку между двумя ячейками
                        RemoveWall(row, col, direct);
                        break;
                    }
                }
            }
            else
            {
                //если направление задано, то удаляем стенку в
                //заданном направлении
                RemoveWall(row, col, dir);
            }
        }

        private void CreateBranch(int row, int col)
        {
            int curRow, curCol;
            bool bMoved;
            int i, r, c, dirOffset, dir;

            //начинаем путь в случайном направоении
            AttachCellToTree(row, col, -1);

            //[row, col] перестает быть граничной
            MarkFrontierCells(row, col);
            curRow = row;
            curCol = col;
            do
            {
                bMoved = false; //пометка - движения нового не было

                //выбираем случайное направление
                dirOffset = (int)(random() * 4);
                for (i = 0; i < 4; i++)
                {
                    dir = (dirOffset + i) % 4;
                    r = curRow + dr[dir];
                    c = curCol + dc[dir];
                    if (InMazeBorders(r, c, MazeHeight, MazeWidth) &&
                       ((map[r][c] & CellOptions.CELL_FRONTIER) != 0x00))
                    { //если ячейка в лабиринте и не граничная

                        //добавляем ее в путь и связываем с предыдущей
                        AttachCellToTree(r, c, (dir + 2) % 4);

                        //новую ячеку делаем граничной
                        MarkFrontierCells(r, c);
                        bMoved = true; //пометка, что было движение
                        curRow = r;
                        curCol = c;
                        break; //работа с новой точкой
                    }
                }
            } while (bMoved); //двигаемся пока можем
        }

        //запуск генерации лабиринта
        public void Generate()
        {
            int n, r, c;

            //помечаем весь лабиринт, как полностью без проходов
            for (int i = 0; i < MazeHeight; i++)
                for (int j = 0; j < MazeWidth; j++)
                    map[i][j] = 0;

            //выбираем случайную ячейку в лабиринте
            r = (int)(random() * MazeHeight);
            c = (int)(random() * MazeWidth);

            //помечаем данную ячейку как посещенную
            map[r][c] |= CellOptions.CELL_VISITED;

            //добавляем в список все граничные ячейки
            MarkFrontierCells(r, c);

            while (CellList.Count != 0)
            { //пока есть граничные ячейки выполняем цикл

                //выбираем случаную граничную ячейку из доступного списка
                n = (int)(random() * CellList.Count);

                //"прорубаем" проход в данном направлении
                CreateBranch(CellList[n].GetR(), CellList[n].GetC());
            }
        }
    }
}
