using System;
using System.Collections.Generic;
using System.Text;

namespace DijkstraAlgorithm
{
    public struct DijkstraRez
    {
        public int[] dist; // расстояния от заданной вершины

        public int[] parent; // из какой вершины пришли;
        // служит для восстановления маршрута
    }

    public class Dijkstra
    {
        /**************Описание графа*****************/

        int N; // количество вершин

        // матрица смежности: adj_matrix[i][j] == true,
        // если между вершинами i и j существует ребро
        bool[][] adj_matrix;

        int[][] cost; // веса рёбер

        /*********************************************/

        /*********Результаты работы алгоритма*********/

        int[] dist; // расстояния от заданной вершины

        int[] parent; // из какой вершины пришли;
        // служит для восстановления маршрута

        /*********************************************/

        public Dijkstra(int[][] rebra)
        {
            N = rebra.Length ;

            /*cost = new int[N][];
            for (int i = 0; i < N; i++)
                cost[i] = new int[N];*/

            adj_matrix = new bool[N][];
            for (int i = 0; i < N; i++)
                adj_matrix[i] = new bool[N];

            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                    adj_matrix[i][j] = rebra[i][j] != int.MaxValue;

            cost = rebra;
            dist = new int[N];
            parent = new int[N];

        }
        // start -- вершина, от которой считаем расстояния
        private void dijkstra(int start)
        {
            // in_tree[i] == true, если для вершины i
            // уже посчитано минимальное расстояние
            bool[] in_tree = new bool[N]; for (int i = 0; i < N; i++) in_tree[i] = false;

            for (int i = 0; i < N; i++)
                dist[i] = int.MaxValue; // машинная бесконечность,
            // т. е. любое расстояние будет меньше данного

            dist[start] = 0; // понятно почему, не так ли? ;)

            int cur = start; // вершина, с которой работаем

            // пока есть необработанная вершина
            while (!in_tree[cur])
            {
                in_tree[cur] = true;

                for (int i = 0; i < N; i++)
                {
                    // если между cur и i есть ребро
                    if (adj_matrix[cur][i])
                    {
                        // считаем расстояние до вершины i:
                        // расстояние до cur + вес ребра
                        int d = dist[cur] + cost[cur][i];
                        // если оно меньше, чем уже записанное
                        if (d < dist[i])
                        {
                            dist[i] = d;   // обновляем его
                            parent[i] = cur; // и "родителя"
                        }
                    }
                }

                // ищем нерассмотренную вершину
                // с минимальным расстоянием
                int min_dist = int.MaxValue;
                for (int i = 0; i < N; i++)
                {
                    if (!in_tree[i] && dist[i] < min_dist)
                    {
                        cur = i;
                        min_dist = dist[i];
                    }
                }
            }

            // Теперь:
            // в dist[i] минимальное расстояние от start до i
            // в parent[i] вершина, из которой лежит оптимальный путь в i
        }

        public DijkstraRez GetDijkstraRez(int start)
        {
            dijkstra(start);
            DijkstraRez rez = new DijkstraRez();
            rez.dist = this.dist;
            rez.parent = this.parent;
            return rez;
        }
    }
}
