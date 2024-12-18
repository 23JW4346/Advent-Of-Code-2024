using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Configuration;
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
            int[,] costs = new int[map.GetLength(0), map.GetLength(1)];
            for (int x = 0; x < costs.GetLength(0); x++)
                for (int y = 0; y < costs.GetLength(1); y++)
                    costs[x, y] = int.MaxValue;
            bool[,,] visited = new bool[map.GetLength(0), map.GetLength(1), 4];
            Queue<(int x, int y, Direction, int cost)> myQueue = new Queue<(int x, int y, Direction, int cost)>();
            myQueue.Enqueue((startPos.x, startPos.y, Direction.right, 0));
            while (myQueue.Count > 0)
            {
                (int x, int y, Direction dir, int curCost) = myQueue.Dequeue();
                if ((x, y) == endPos)
                {
                    costs[x, y] = costs[x, y] > curCost ? curCost : costs[x, y];
                }
                else if (!visited[x, y, (int)dir])
                {
                    visited[x, y, (int)dir] = true;
                    if (costs[x, y] > curCost)
                        costs[x, y] = curCost;
                    int nx = x + dxy[(int)dir].x;
                    int ny = y + dxy[(int)dir].y;
                    if (IsValidMove(nx, ny, map))
                        myQueue.Enqueue((nx, ny, dir, curCost + 1));

                    Direction tempDir = (Direction)(((int)dir + 1) % 4);
                    nx = x + dxy[(int)tempDir].x;
                    ny = y + dxy[(int)tempDir].y;
                    int tempCost = curCost + 1001;
                    costs[nx, ny] = tempCost;
                    if (costs[nx, ny] > curCost)
                        costs[nx, ny] = tempCost;
                    else
                        tempCost = costs[nx, ny] + 1001;
                    if (IsValidMove(nx, ny, map))
                        myQueue.Enqueue((nx, ny, tempDir, tempCost));

                    tempDir = (Direction)(((int)dir + 3) % 4);
                    nx = x + dxy[(int)tempDir].x;
                    ny = y + dxy[(int)tempDir].y;
                    tempCost = curCost + 1001;
                    if (costs[nx, ny] > curCost)
                        costs[nx, ny] = tempCost;
                    else
                        tempCost = costs[nx, ny] + 1001;
                    if (IsValidMove(nx, ny, map))
                        myQueue.Enqueue((nx, ny, tempDir, tempCost));
                }
            }
            return costs[endPos.x, endPos.y];
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
