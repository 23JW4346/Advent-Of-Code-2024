using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace Day_16
{
    internal class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine($"Part 1:");
            Console.WriteLine($"Test 1: {SOL1(GetPuzzleInput("test1.txt"))}");
            Console.WriteLine($"Test 2: {SOL1(GetPuzzleInput("test2.txt"))}");
            Console.WriteLine($"Actual: {SOL1(GetPuzzleInput("input.txt"))}");
            Console.ReadKey();
        }

        static char[,] GetPuzzleInput(string filename)
        {
            string[] lines = File.ReadAllLines(filename);
            char[,] maze = new char[lines[0].Length, lines.Length];
            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    maze[j,i] = lines[i][j];
                }
            }
            return maze;
        }

        static long SOL1(char[,] maze)
        {
            ((int x,int y) start, (int x, int y) end) = FindStartAndEnd(maze);
            int mazetraveled = BFS(maze, start, end);
            return mazetraveled;
        }

        static void Print(char[,] maze, (int x, int y) point)
        {
            for (int i = 0; i < maze.GetLength(0); i++)
            {
                for (int j = 0; j < maze.GetLength(1); j++)
                {
                    if ((j, i) == point) Console.Write('X');
                    else Console.Write(maze[j,i]);
                }
                Console.WriteLine();
            }
        }

        static ((int, int), (int, int)) FindStartAndEnd(char[,] maze)
        {
            (int x, int y) start = (0, 0), end = (0, 0);
            for (int i = 0; i < maze.GetLength(0); i++)
            {
                for (int j = 0; j < maze.GetLength(1); j++)
                {
                    if (maze[i, j] == 'S') // Start point
                    {
                        start = (i, j);
                    }
                    else if (maze[i, j] == 'E') // End point
                    {
                        start = (i, j);
                    }
                }
            }
            return (start, end);
        }

        static int BFS(char[,] maze, (int x, int y) start, (int x, int y) end)
        {
            int rows = maze.GetLength(0);
            int cols = maze.GetLength(1);

            (int, int)[] directions = { (1, 0), (0, -1), (-1, 0), (0, -1) };
            // Distance matrix to keep track of the shortest distance from the start
            int[,] distance = new int[rows, cols];
            for(int x = 0; x < rows; x++)
            {
                for(int y = 0; y < cols; y++)
                {
                    distance[x, y] = int.MaxValue;
                }
            }
            bool[,,] visited = new bool[rows, cols, directions.Length]; // To track visited cells
            (int, int)[,] parent = new (int, int)[rows, cols]; // For path reconstruction

            Queue<((int, int),(int ,int)) > queue = new Queue<((int, int), (int, int))>();
            queue.Enqueue((start, directions[0]));
            visited[start.x, start.y,3] = true;
            distance[start.x, start.y] = 0;

            // BFS traversal
            while (queue.Count > 0)
            {
                ((int, int) current, (int x, int y) direction) = queue.Dequeue();
                // If we reach the end point
                if (current == end)
                {
                    PrintPath(parent, end.x, end.y);
                }
                int x = current.Item1;
                int y = current.Item2;

                // Explore Neighbours
                for (int i = 0; i < 4; i += 1)
                {
                    (int x, int y) d = directions[(Array.IndexOf(directions, direction) + 1) % 4];
                    int nx = x + d.x;
                    int ny = y + d.y;
                    if(d == (-direction.x, -direction.y))
                    // check if new position isn't in the wall and not visited
                    if (maze[nx, ny] != '#' && !visited[nx, ny,Array.IndexOf(directions,d)])
                    {
                        visited[nx, ny, Array.IndexOf(directions, d)] = true;
                        if (distance[nx, ny] > distance[x, y] + 1) distance[nx, ny] = distance[x, y] + 1;
                        parent[nx, ny] = (x, y);
                        queue.Enqueue(((nx, ny), d));
                    }
                }
            }

            return distance[end.x, end.y];
        }

        static void PrintPath((int, int)[,] parent, int endX, int endY)
        {
            List<string> path = new List<string>();
            (int, int) current = (endX, endY);

            while (current != (-1,-1))
            {
                path.Add($"({current.Item1}, {current.Item2})");
                current = parent[current.Item1, current.Item2];
            }

            path.Reverse();
            Console.WriteLine("Shortest Path: ");
            foreach (var step in path)
            {
                Console.WriteLine(step);
            }
        }
    }
}