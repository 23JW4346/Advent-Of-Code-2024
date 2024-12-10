using System;
using System.Collections.Generic;
using System.IO;

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

        static HashSet<tile> nines = new HashSet<tile> ();
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

        static void PrintMap(tile[,] map)
        {
            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    Console.Write(map[x, y].height);
                }
                Console.WriteLine();
            }
        }

        static int SOL1(tile[,] map)
        {
            int answer = 0;
            //PrintMap(map);
            foreach (tile myTile in map)
            {
                if (myTile.height == 0)
                {
                    CheckAround(map, myTile);
                    answer += nines.Count;
                    nines.Clear();
                }
            }
            return answer;
        }

        static void CheckAround(tile[,] map, tile startTile)
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
                        CheckAround(map, temptile);
                    }
                }
                if (startTile.up)
                {
                    if (startTile.height + 1 == map[startTile.x, startTile.y + 1].height)
                    {
                        tile temptile = map[startTile.x, startTile.y + 1];
                        CheckAround(map, temptile);
                    }
                }
                if (startTile.right)
                {
                    if (startTile.height + 1 == map[startTile.x + 1, startTile.y].height)
                    {
                        tile temptile = map[startTile.x + 1, startTile.y];
                        CheckAround(map, temptile);
                    }
                }
                if (startTile.left)
                {
                    if (startTile.height + 1 == map[startTile.x - 1, startTile.y].height)
                    {
                        tile temptile = map[startTile.x - 1, startTile.y];
                        CheckAround(map, temptile);
                    }
                }
            }
        }

        static int SOL2(tile[,] map)
        {
            int answer = 0;
            foreach(tile myTile in map)
            {
                if(myTile.height == 0)
                {
                    answer += CheckRating(map, myTile);
                }
            }
            return answer;
        }

        static int CheckRating(tile[,] map, tile startTile)
        {
            int ratingl = 0, ratingd=0, ratingu=0, ratingr=0;
            if(startTile.height == 9)
            {
                return 1;
            }
            else
            {
                if (startTile.down)
                {
                    if(startTile.height+1 == map[startTile.x, startTile.y-1].height)
                    {
                        tile temptile = map[startTile.x, startTile.y-1];
                        ratingd = CheckRating(map, temptile); 
                    }
                }
                if (startTile.up)
                {
                    if (startTile.height + 1 == map[startTile.x, startTile.y + 1].height)
                    {
                        tile temptile = map[startTile.x, startTile.y + 1];
                        ratingu = CheckRating(map, temptile);
                    }
                }
                if (startTile.right)
                {
                    if (startTile.height + 1 == map[startTile.x+1, startTile.y].height)
                    {
                        tile temptile = map[startTile.x+1, startTile.y];
                        ratingr = CheckRating(map, temptile);
                    }
                }
                if (startTile.left)
                {
                    if (startTile.height + 1 == map[startTile.x-1, startTile.y].height)
                    {
                        tile temptile = map[startTile.x-1, startTile.y];
                        ratingl = CheckRating(map, temptile);
                    }
                }
            }
            return ratingr + ratingd + ratingl + ratingu;
        }
    }
}
