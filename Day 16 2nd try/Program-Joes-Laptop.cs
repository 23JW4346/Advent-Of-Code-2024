using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;

namespace Day_16_2nd_try
{
    internal class Program
    {

        static void Main(string[] args)
        {

            Console.WriteLine($"Test1: {SOL1(GetPuzzleInput("test1.txt"))}");
            Console.WriteLine($"Test2: {SOL1(GetPuzzleInput("test2.txt"))}");
            Console.WriteLine($"Input: {SOL1(GetPuzzleInput("input.txt"))}");
            Console.ReadKey();
        }

        static int SOL1(char[,] maze)
        {
            int result = BFS(maze, FindStart(maze), FindEnd(maze));
            return result;
        }

        static char[,] GetPuzzleInput(string filename)
        {
            string[] lines = File.ReadAllLines(filename);
            char[,] maze = new char[lines[0].Length, lines.Length];
            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    maze[j, i] = lines[i][j];
                }
            }
            return maze;
        }

        static (int, int) FindStart(char[,] maze)
        {
            for (int i = 0; i < maze.GetLength(0); i++)
            {
                for (int j = 0; j < maze.GetLength(1); j++)
                {
                    if (maze[j, i] == 'S')
                    {
                        return (j, i);
                    }
                }
            }
            return (0, 0);
        }

        static (int, int) FindEnd(char[,] maze)
        {
            for (int i = 0; i < maze.GetLength(0); i++)
            {
                for (int j = 0; j < maze.GetLength(1); j++)
                {
                    if (maze[j, i] == 'E')
                    {
                        return (j, i);
                    }
                }
            }
            return (0, 0);
        }

        static int BFS(char[,] maze, (int startX, int startY) start, (int endX, int endY) end)
        {
            // Directions: North, East, South, West
            int[] dx = { -1, 0, 1, 0 };  // Row changes for directions
            int[] dy = { 0, 1, 0, -1 };  // Column changes for directions

            int rows = maze.GetLength(0);
            int cols = maze.GetLength(1);
            int startX = start.startX;
            int startY = start.startY;
            int endX = end.endX;
            int endY = end.endY;
            // Cost array to track the minimum cost to reach each position with a specific direction
            int[,] cost = new int[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    cost[i, j] = int.MaxValue; 
                }
            }
            bool[,,] visited = new bool[rows, cols, 4];
            Queue<(int, int, int, int)> queue = new Queue<(int, int, int, int)>();
            queue.Enqueue((startX, startY, 1, 0));
            cost[startX, startY] = 0;
            while (queue.Count > 0)
            {
                (int x, int y, int dir, int currentCost) = queue.Dequeue();

                if (!visited[x, y, dir])
                {
                    visited[x, y, dir] = true;
                    // If we reached the end point, return the cost
                    if (x == endX && y == endY)
                    {
                        if (cost[x, y] > currentCost) cost[x, y] = currentCost;
                    }

                    int tempCost;

                    // 1. Move forward in the current direction
                    int nx = x + dx[dir];
                    int ny = y + dy[dir];
                    if (IsValidMove(nx, ny, maze))
                    {
                        tempCost = currentCost + 1;
                        if (cost[nx, ny] > tempCost) cost[nx, ny] = tempCost;
                        else tempCost = cost[nx,ny];
                        visited[nx, ny, dir] = false;
                        queue.Enqueue((nx, ny, dir, tempCost));
                    }

                    // 2. Rotate 90 degrees clockwise
                    int newDir = (dir + 1) % 4;
                    tempCost = currentCost + 1000;
                    if (tempCost < cost[x, y])
                    {
                        cost[x, y] = tempCost;
                    }
                    else
                    {
                        tempCost = cost[x, y] + 1000 ;
                    }
                    queue.Enqueue((x, y, newDir, tempCost));

                    // 3. Rotate 90 degrees counter-clockwise
                    newDir = (dir + 3) % 4;
                    tempCost = currentCost + 1000;
                    if (tempCost < cost[x, y])
                    {
                        cost[x, y] = tempCost;
                    }
                    else
                    {
                        tempCost = cost[x, y] + 1000;
                    }
                    queue.Enqueue((x, y, newDir, tempCost));
                } 
            }
            return cost[endX, endY]; 
        }

        static bool IsValidMove(int x, int y, char[,] maze)
        {
            return x >= 0 && y >= 0 && x < maze.GetLength(0) && y < maze.GetLength(1) && maze[x, y] != '#';
        }
    }
}
