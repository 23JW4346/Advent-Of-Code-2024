using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Security.Principal;

namespace Day_16_2nd_try
{
    internal class Program
    {

        public class PriorityQueue<T>
        {
            private List<(T item, int priority)> elements = new List<(T item, int priority)>();

            public int Count => elements.Count;

            public void Enqueue(T item, int priority)
            {
                elements.Add((item, priority));
                elements.Sort((x, y) => x.priority.CompareTo(y.priority));
            }

            public T Dequeue()
            {
                if (elements.Count == 0)
                    throw new InvalidOperationException("The priority queue is empty.");

                var bestItem = elements[0].item;
                elements.RemoveAt(0);
                return bestItem;
            }
        }

        public enum Direction
        {
            right, down, left, up
        }

        static void Main(string[] args)
        {
            Console.WriteLine($"Test1: {SOL1(GetPuzzleInput("test1.txt"))}");
            Console.WriteLine($"Test2: {SOL1(GetPuzzleInput("test2.txt"))}");
            Console.WriteLine($"Input: {SOL1(GetPuzzleInput("input.txt"))}");
            Console.WriteLine($"Test1.2: {SOL2(GetPuzzleInput("test1.txt"))}");
            Console.WriteLine($"Test2.2: {SOL2(GetPuzzleInput("test2.txt"))}");
            Console.WriteLine($"Input.2: {SOL2(GetPuzzleInput("input.txt"))}");
            Console.ReadKey();
        }

        static int SOL1(char[,] maze)
        {
            ((int, int) start, (int, int) end) = FindStartEnd(maze);
            int result = BFS1(maze, start, end);
            return result;
        }

        static int SOL2(char[,] maze)
        {
            ((int, int) start, (int, int) end) = FindStartEnd(maze);
            int result = BFS2(maze, start, end);
            if(result > 100)
            {
                string[] print = new string[maze.GetLength(0)];
                for(int x = 0; x < maze.GetLength(0); x++)
                {
                    for(int y = 0; y < maze.GetLength(1); y++)
                    {
                        print[x] += maze[x, y];
                    }
                }
                File.WriteAllLines("output.txt", print);
            }
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

        static void Print(char[,] map)
        {
            for(int x= 0; x < map.GetLength(0); x++)
                for(int y = 0;  y < map.GetLength(1); y++)
                    Console.Write(map[y, x] + (y == map.GetLength(1)-1? "\n":""));
        }

        static int BFS(char[,] map, (int x,int y) startPos, (int x,int y) endPos, HashSet<(int,int,Direction)> seen, int[,] costs, ref Direction dIn) 
        {
            (int x, int y)[] dxy = { (1, 0), (0, 1), (-1, 0), (0, -1) };
            PriorityQueue<(int x, int y, Direction, int)> myQueue = new PriorityQueue<(int x, int y, Direction, int)>();
            myQueue.Enqueue((startPos.x, startPos.y, dIn, 0), 0);
            costs[startPos.x, startPos.y] = 0;

            while (myQueue.Count > 0)
            {
                (int x, int y, Direction dir, int score) = myQueue.Dequeue();
                if ((x, y) == endPos)
                {
                    if (costs[x, y] > score)
                    {
                        costs[x, y] = Math.Min(score, costs[endPos.x, endPos.y]);
                        dIn = dir;
                    }
                }

                if (!seen.Add((x, y, dir)))
                    continue;

                costs[x, y] = Math.Min(score, costs[x, y]);
                int nx = x + dxy[(int)dir].x;
                int ny = y + dxy[(int)dir].y;
                if (IsValidMove(nx, ny, map) && !seen.Contains((nx, ny, dir)))
                {
                    myQueue.Enqueue((nx, ny, dir, score + 1), score + 1);
                }

                for (int i = 1; i <= 3; i++)
                {
                    Direction newDir = (Direction)(((int)dir + i) % 4);
                    if (!seen.Contains((x, y, newDir)))
                    {
                        myQueue.Enqueue((x, y, newDir, score + 1000), score + 1000);
                    }
                }
            }
            return costs[endPos.x, endPos.y];
        }

        static int BFS1(char[,] map, (int x, int y) startPos, (int x, int y) endPos)
        {
            (int x, int y)[] dxy = { (1, 0), (0, 1), (-1, 0), (0, -1) };
            HashSet<(int, int, Direction)> seen = new HashSet<(int, int, Direction)>();
            int[,] costs = new int[map.GetLength(0), map.GetLength(1)];
            Direction dir = Direction.right;
            for (int x = 0; x < costs.GetLength(0); x++)
                for (int y = 0; y < costs.GetLength(1); y++)
                    costs[x, y] = int.MaxValue;
            return BFS(map, startPos, endPos, seen, costs, ref dir);
        }

        static ((int, int), (int, int)) FindStartEnd(char[,] maze)
        {
            (int, int) start = (0, 0), end = (0, 0);
            for (int i = 0; i < maze.GetLength(0); i++)
                for (int j = 0; j < maze.GetLength(1); j++)
                    if (maze[i, j] == 'E')
                        end = (i, j);
                    else if (maze[i, j] == 'S')
                        start = (i, j);

            return (start, end);
        }

        static bool IsValidMove(int x, int y, char[,] maze) 
            => x >= 0 && y >= 0 && x < maze.GetLength(0) && y < maze.GetLength(1) && maze[x, y] != '#';

        public static int BFS2(char[,] map, (int x, int y) startPos, (int x, int y) endPos)
        {
            (int x, int y)[] dxy = { (1, 0), (0, 1), (-1, 0), (0, -1) };
            HashSet<(int, int, Direction)> seen = new HashSet<(int, int, Direction)>();
            int[,] costs2 = new int[map.GetLength(0), map.GetLength(1)];
            int[,] costs = new int[map.GetLength(0), map.GetLength(1)];
            for (int x = 0; x < costs.GetLength(0); x++)
                for (int y = 0; y < costs.GetLength(1); y++)
                {
                    costs[x, y] = int.MaxValue;
                    costs2[x, y] = int.MaxValue;
                }
            Direction dir = Direction.right;
            int best = BFS(map, startPos, endPos, seen, costs, ref dir);
            seen.Clear();
            dir = (Direction)(((int)dir + 2) % 4);
            BFS(map, endPos, startPos, seen, costs2, ref dir);
            HashSet<(int, int)> bestPaths = new HashSet<(int, int)>();
            for(int x = 0; x < map.GetLength(0); x++) 
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    if (costs[x, y] == int.MaxValue || costs2[x, y] == int.MaxValue)
                        continue;
                    //first for in a straight line, second for the corners, as they both update with 1000 later
                    if (costs[x, y] + costs2[x, y] == best || costs[x, y] + costs2[x, y] == best - 1000)
                        bestPaths.Add((x, y));
                }
            }
            foreach((int x, int y) in bestPaths)
            {
                map[x, y] = 'X';
            }
            Print(map);
            return bestPaths.Count;
        }

        static void MarkBestPaths(int x, int y, int cost, int[,] costs, char[,] map, List<(int, int)> bestPaths, HashSet<(int, int, Direction)> seen)
        {
            if (cost == int.MaxValue || costs[x, y] != cost)
                return;
            bestPaths.Add((x, y));
            (int x, int y)[] dxy = { (1, 0), (0, 1), (-1, 0), (0, -1) };
            for (int i = 0; i < 4; i++)
            {
                int nx = x - dxy[i].x;
                int ny = y - dxy[i].y;
                Direction prevDir = (Direction)i;
                if (IsValidMove(nx, ny, map) && seen.Contains((nx, ny, prevDir)))
                {
                    if (costs[nx, ny] == cost - 1)
                        MarkBestPaths(nx, ny, cost - 1, costs, map, bestPaths, seen);

                    else if (costs[nx, ny] == cost - 1001)
                        MarkBestPaths(nx, ny, cost - 1001, costs, map, bestPaths, seen);
                }
            }
        }
    }
}
