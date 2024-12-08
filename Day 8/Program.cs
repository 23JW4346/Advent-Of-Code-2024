using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace Day_8
{
    internal class Program
    {
        static void Main(string[] args)
        {
            char[,] map = GetPuzzleInput();
            int answer = SOL1(map);
            Console.WriteLine($"Part 1: {answer}");
            answer = SOL2(map);
            Console.WriteLine($"Part 2: {answer}");
            Console.ReadKey();
        }

        static Dictionary<char, List<(int x, int y)>> PopulateDict(char[,] map)
        {
            Dictionary<char, List<(int x, int y)>> nodes = new Dictionary<char, List<(int, int)>>();
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i, j] != '.')
                    {
                        if (nodes.ContainsKey(map[i, j]))
                        {
                            nodes[map[i, j]].Add((i, j));
                        }
                        else
                        {
                            nodes.Add(map[i, j], new List<(int, int)>());
                            nodes[map[i, j]].Add((i, j));
                        }
                    }
                }
            }
            return nodes;
        }

        static int SOL1(char[,] map)
        {
            Dictionary<char, List<(int x, int y)>> nodes = PopulateDict(map);
            HashSet<(int, int)> antinode = new HashSet<(int, int)>();
            (int x, int y) distance = (0, 0);
            foreach (char c in nodes.Keys)
            {
                for (int i = 0; i < nodes[c].Count; i++)
                {
                    (int x, int y) temp = nodes[c][i];
                    for (int j = 0; j < nodes[c].Count; j++)
                    {
                        if (temp != nodes[c][j])
                        {
                            distance.y = temp.y - nodes[c][j].y;
                            distance.x = temp.x - nodes[c][j].x;

                            if (temp.y + distance.y < map.GetLength(0) && temp.y + distance.y > -1 && temp.x + distance.x < map.GetLength(1) && temp.x + distance.x > -1)
                            {
                                antinode.Add((temp.y + distance.y, temp.x + distance.x));
                            }
                            if (nodes[c][j].y - distance.y < map.GetLength(0) && nodes[c][j].y - distance.y > -1 && nodes[c][j].x - distance.x < map.GetLength(1) && nodes[c][j].x - distance.x > -1)
                            {
                                antinode.Add((nodes[c][j].y - distance.y, nodes[c][j].x - distance.x));

                            }
                        }
                    }
                }
            }
            return antinode.Count;
        }


        static char[,] GetPuzzleInput()
        {
            string[] lines = File.ReadAllLines("input.txt");
            char[,] map = new char[lines.Length, lines[0].Length];
            int i = 0;
            foreach (string line in lines)
            {
                foreach (char c in line)
                {
                    map[i / map.GetLength(0), i % map.GetLength(1)] = c;
                    i++;
                }
            }
            return map;
        }

        static int SOL2(char[,] map)
        {
            Dictionary<char, List<(int x, int y)>> nodes = PopulateDict(map);
            HashSet<(int, int)> antinode = new HashSet<(int, int)>();
            (int x, int y) distance = (0, 0);
            foreach (char c in nodes.Keys)
            {
                for (int i = 0; i < nodes[c].Count; i++)
                {
                    (int x, int y) temp = nodes[c][i];
                    for (int j = 0; j < nodes[c].Count; j++)
                    {
                        if (temp != nodes[c][j])
                        {
                            int k = 0;
                            distance.y = temp.y - nodes[c][j].y;
                            distance.x = temp.x - nodes[c][j].x;
                            while (temp.y + distance.y*k < map.GetLength(0) && temp.y + distance.y*k > -1 && temp.x + distance.x*k < map.GetLength(1) && temp.x + distance.x * k > -1)
                            {
                                antinode.Add((temp.y + distance.y * k, temp.x + distance.x * k));
                                k++;
                            }
                            k = 0;
                            while (nodes[c][j].y - distance.y*k < map.GetLength(0) && nodes[c][j].y - distance.y*k > -1 && nodes[c][j].x - distance.x * k < map.GetLength(1) && nodes[c][j].x - distance.x * k > -1)
                            {
                                antinode.Add((nodes[c][j].y - distance.y * k, nodes[c][j].x - distance.x * k));
                                k++;
                                
                            }
                        }
                    }
                }
            }
            return antinode.Count;
        }

    }
}
