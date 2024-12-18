using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;

namespace Day_18
{
    internal class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine($"Part 1: {SOL1(GetPuzzleInput("input.txt", 1024))}");
            Console.WriteLine($"Part 2: {SOL2(GetPuzzleInput("input.txt", 3450))}");
            Console.ReadKey();
        }

        static (int,int)[] GetPuzzleInput(string filename, int times)
        {
            string[] lines = File.ReadAllLines(filename);
            (int, int)[] points = new (int, int)[lines.Length];
            for(int i = 0; i < times; i++)
            {
                string[] split = lines[i].Split(',');
                (int, int) temp = (int.Parse(split[0]), int.Parse(split[1]));
                points[i] = temp;
            }
            return points;
        }

        static int SOL1((int, int)[] points)
        {
            char[,] map = new char[71, 71];
            PopulateMap(map, points);
            Print(map);
            int result = BFS(map);
            return result;
        }

        static (int,int) SOL2((int, int)[] points)
        {
            List<(int, int)> pointsToUse = new List<(int, int)>();
            for(int i = 0; i < 1024; i++)
            {
                pointsToUse.Add(points[i]);
            }
            for(int i =1024; i < points.Length; i++)
            {
                char[,] map = new char[71, 71];
                pointsToUse.Add(points[i]);
                PopulateMap(map, pointsToUse.ToArray());
                int result = BFS(map);
                if (result == int.MaxValue) return points[i];
            }
            return (0,0);
        }

        static void Print(char[,] map)
        {
            for(int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++) Console.Write(map[x,y]);
                Console.WriteLine();
            }
        }

        static void PopulateMap(char[,] map, (int, int)[] points)
        {
            for(int x = 0; x <  map.GetLength(0); x++)
            {
                for(int y = 0; y < map.GetLength(1); y++)
                {
                    if (points.Contains((x, y)))
                    {
                        map[x, y] = '#';
                    }
                    else
                    {
                        map[x, y] = '.';
                    }
                }
            }
        }

        static int BFS(char[,] map)
        {
            (int x,int y)[] dxy = { (-1, 0), (1, 0), (0, 1), (0, -1) };
            (int x, int y) startPos = (0, 0);
            (int x, int y) endPos = (70, 70);
            bool[,] visited = new bool[71, 71];
            Queue<(int x, int y, int cost)> myQueue = new Queue<(int x, int y, int cost)>();
            myQueue.Enqueue((startPos.x, startPos.y, 0));
            while(myQueue.Count > 0)
            {
                (int x, int y, int curCost) = myQueue.Dequeue();
                if ((x,y) == endPos) return curCost;
                if (!visited[x, y])
                {
                    visited[x, y] = true;
                    for (int i = 0; i < 4; i++)
                    {
                        int nx = x + dxy[i].x;
                        int ny = y + dxy[i].y;
                        if (IsValid(map, nx, ny))
                        {
                            myQueue.Enqueue((nx, ny, curCost + 1));
                        }
                    }
                }
            }
            return int.MaxValue;
        }

        static bool IsValid(char[,] map, int x, int y)
        {
            return x >= 0 && y >= 0 && x < map.GetLength(0) && y < map.GetLength(1) && map[x,y] != '#';
        }
    }
}
