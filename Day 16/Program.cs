using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace Day_16
{
    internal class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine($"Part 1: {SOL1()}");
            Console.ReadKey();
        }

        static char[,] GetPuzzleInput()
        {
            string[] lines = File.ReadAllLines("input.txt");
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

        static long SOL1()
        {
            char[,] maze = GetPuzzleInput();
            int mazetraveled = DjikstrasP1(maze);
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

        static int DjikstrasP1(char[,] maze)
        {
            (int x, int y) startNode = (0,0), endNode = (0,0);
            int[,] bestScores = new int[maze.GetLength(0), maze.GetLength(1)];
            for (int x = 0; x <  maze.GetLength(0); x++) 
            {
                for(int y = 0; y < maze.GetLength(1); y++)
                {
                    if (maze[x, y] == 'S')
                    {
                        startNode = (x, y);
                        bestScores[x, y] = 0;
                    }
                    else if (maze[x,y] == 'E')
                    {
                        endNode = (x, y);
                        bestScores[x, y] = int.MaxValue;
                    }
                    else
                    {
                        bestScores[x, y] = int.MaxValue;
                    }
                }
            }
            (int,int) direction = (1,0);
            HashSet<(int, int, (int, int))> visited = new HashSet<(int, int, (int, int))>();
            Queue<(int x, int y, (int,int) d, int distance)> myQueue = new Queue<(int, int, (int,int), int)>();
            myQueue.Enqueue((startNode.x, startNode.y, direction, 0));
            while(myQueue.Count > 0 )
            {
                (int x, int y, (int,int) d, int distance) = myQueue.Dequeue();
                if (visited.Add((x, y, d)))
                {
                    bool reachedEnd = false;
                    (int x, int y) myPoint = (x + d.Item1, y + d.Item2);
                    if (maze[myPoint.x, myPoint.y] == '.')
                    {
                        myQueue.Enqueue((myPoint.x, myPoint.y, d, distance + 1));
                    }
                    else if (myPoint == endNode)
                    {
                        reachedEnd = true;
                    }
                    if (maze[x,y] != '#')
                    {
                        if (bestScores[myPoint.x, myPoint.y] > distance + 1) bestScores[myPoint.x, myPoint.y] = distance + 1;
                    }
                    if (!reachedEnd)
                    {
                        if(d.Item1 !=0 )
                        {
                            if (maze[x, y + 1] != '#') myQueue.Enqueue((x, y, (0, 1), distance + 1000));
                            if (maze[x, y - 1] != '#') myQueue.Enqueue((x, y, (0, -1), distance + 1000));
                        }
                        else
                        {
                            if (maze[x + 1, y] != '#') myQueue.Enqueue((x, y, (1, 0), distance + 1000));
                            if (maze[x - 1, y] != '#') myQueue.Enqueue((x, y, (-1, 0), distance + 1000));
                        }
                    }
                }
                Print(maze, (x, y));
                System.Threading.Thread.Sleep(100);
                Console.Clear();
            }
            return bestScores[endNode.x, endNode.y];
        }
    }
}