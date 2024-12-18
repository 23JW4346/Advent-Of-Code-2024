using System;
using System.Collections.Generic;
using System.IO;

namespace Day_16_2nd_try
{
    internal class Program
    {

        public enum Direction
        {
            right, down, left, up
        }

        static void Main(string[] args)
        {
            Console.WriteLine($"Test1: {SOL1(GetPuzzleInput("test1.txt"))}");
            Console.WriteLine($"Test2: {SOL1(GetPuzzleInput("test2.txt"))}");
            Console.WriteLine($"Input: {SOL1(GetPuzzleInput("input.txt"))}");
            Console.ReadKey();
        }

        static int SOL1(char[,] maze)
        {
            ((int, int) start, (int, int) end) = FindStartEnd(maze);
            int result = BFS(maze, start, end);
            return result;
        }

        static char[,] GetPuzzleInput(string filename)
        {
            string[] lines = File.ReadAllLines(filename);
            char[,] maze = new char[lines[0].Length, lines.Length];
            for (int i = 0; i < lines.Length; i++)
                for (int j = 0; j < lines[i].Length; j++)
                    maze[j, i] = lines[i][j];

            return maze;
        }

        static int BFS(char[,] map, (int x, int y) startPos, (int x, int y) endPos)
        {
            (int x, int y)[] dxy = { (1, 0), (0, 1), (-1, 0), (0, -1) };
            int[,,] costs = new int[map.GetLength(0), map.GetLength(1), 4];
            for (int x = 0; x < costs.GetLength(0); x++)
                for (int y = 0; y < costs.GetLength(1); y++)
                    for(int d = 0; d <  dxy.GetLength(0); d++)  
                        costs[x, y, d] = int.MaxValue;
            HashSet<(int, int, Direction)> seen = new HashSet<(int, int, Direction)>();
            int minval = int.MaxValue;
            Queue<(int x, int y, Direction, int cost)> myQueue = new Queue<(int x, int y, Direction, int cost)>();
            myQueue.Enqueue((startPos.x, startPos.y, Direction.right, 0));
            while (myQueue.Count > 0)
            {
                (int x, int y, Direction dir, int curCost) = myQueue.Dequeue();
                if (costs[x, y, (int)dir] > curCost)
                    costs[x, y, (int)dir] = curCost;
                if ((x, y) == endPos)
                    minval = costs[x, y, (int)dir];
                else if (seen.Add((x, y, dir)))
                {
                    int nx = x + dxy[(int)dir].x;
                    int ny = y + dxy[(int)dir].y;
                    if (IsValidMove(nx, ny, map))
                        myQueue.Enqueue((nx, ny, dir, curCost + 1));
                    Direction tempDir = (Direction)(((int)dir + 1) % 4);
                    int tempCost = curCost + 1000;
                    myQueue.Enqueue((x, y, tempDir, tempCost));
                    tempDir = (Direction)(((int)dir + 3) % 4);
                    myQueue.Enqueue((x, y, tempDir, tempCost));
                }
            }
            return minval;
        }

        static ((int, int), (int, int)) FindStartEnd(char[,] maze)
        {
            (int, int) start = (0, 0), end = (0, 0);
            for (int i = 0; i < maze.GetLength(0); i++)
                for (int j = 0; j < maze.GetLength(1); j++)
                    if (maze[j, i] == 'E')
                        end = (j, i);
                    else if (maze[j, i] == 'S')
                        start = (j, i);

            return (start, end);
        }


        static bool IsValidMove(int x, int y, char[,] maze) => x >= 0 && y >= 0 && x < maze.GetLength(0) && y < maze.GetLength(1) && maze[x, y] != '#';

    }
}
