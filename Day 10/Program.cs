using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day_10
{
    internal class Program
    {

        public struct tile
        {
            public int x, y;
            public bool up, down, left, right;
            public int height;
        }

        static void Main(string[] args)
        {
            Console.WriteLine($"Part 1: {SOL1(GetMap())}");
            Console.WriteLine($"Part 2: {SOL2(GetMap())}");
            Console.ReadKey();
        }

        static tile[,] GetMap()
        {
            string[] lines = File.ReadAllLines("input.txt");
            tile[,] map = new tile[lines.Length, lines[0].Length];
            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[0].Length; j++)
                {
                    tile temptile;
                    temptile.x = j;
                    temptile.y = i;
                    temptile.left = false;
                    if (j != 0) temptile.left = true;
                    temptile.right = false;
                    if (j != map.GetLength(0) - 1) temptile.right = true;
                    temptile.down = false;
                    if (i != 0) temptile.down = true;
                    temptile.up = false;
                    if (i != map.GetLength(1) - 1) temptile.up = true;
                    temptile.height = int.Parse(lines[i][j].ToString());
                    map[temptile.x, temptile.y] = temptile;
                }
            }
            return map;
        }

        static int SOL1(tile[,] map)
        {
            int answer = 0;
            HashSet<tile> nines = new HashSet<tile>();
            foreach (tile myTile in map)
            {
                if (myTile.height == 0)
                {
                    CheckAround(map, myTile, nines);
                    answer += nines.Count;
                    nines.Clear();
                }
            }
            return answer;
        }

        static void CheckAround(tile[,] map, tile startTile, ICollection<tile> nines)
        {
            if (startTile.height == 9)
            {
                nines.Add(startTile);
            }
            else
            {
                if (startTile.down)
                {
                    if (startTile.height + 1 == map[startTile.x, startTile.y - 1].height)
                    {
                        tile temptile = map[startTile.x, startTile.y - 1];
                        CheckAround(map, temptile, nines);
                    }
                }
                if (startTile.up)
                {
                    if (startTile.height + 1 == map[startTile.x, startTile.y + 1].height)
                    {
                        tile temptile = map[startTile.x, startTile.y + 1];
                        CheckAround(map, temptile, nines);
                    }
                }
                if (startTile.right)
                {
                    if (startTile.height + 1 == map[startTile.x + 1, startTile.y].height)
                    {
                        tile temptile = map[startTile.x + 1, startTile.y];
                        CheckAround(map, temptile, nines);
                    }
                }
                if (startTile.left)
                {
                    if (startTile.height + 1 == map[startTile.x - 1, startTile.y].height)
                    {
                        tile temptile = map[startTile.x - 1, startTile.y];
                        CheckAround(map, temptile, nines);
                    }
                }
            }
        }

        static int SOL2(tile[,] map)
        {
            List<tile> nines = new List<tile>();
            foreach(tile myTile in map)
            {
                if(myTile.height == 0)
                {
                    CheckAround(map, myTile, nines);
                }
            }
            return nines.Count;
        }
    }
}
