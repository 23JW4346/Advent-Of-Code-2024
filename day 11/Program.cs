using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day_11
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Part 1: {SOL1(GetPuzzleInput())}");
            Console.WriteLine($"Part 2: {SOL2(GetPuzzleInput())}");
            Console.ReadKey();
        }

        static string GetPuzzleInput()
        {
            string line = File.ReadAllText("input.txt");
            return line;
        }

        static int SOL1(string input)
        {
            string[] strlist = input.Split(' ');
            List<long> ints = new List<long>();
            foreach (string str in strlist)
            {
                ints.Add(long.Parse(str));
            }
            for (int i = 0; i < 25; i++)
            {
                GetNextStones(ref ints);
            }
            return ints.Count;
        }

        static void GetNextStones(ref List<long> ints)
        {
            List<long> temp = new List<long>();
            for (int j = 0; j < ints.Count; j++)
            {
                if (ints[j] == 0) temp.Add(1);
                else if (ints[j].ToString().Length % 2 == 0)
                {
                    long left = long.Parse(ints[j].ToString().Substring(0, ints[j].ToString().Length / 2));
                    long right = long.Parse(ints[j].ToString().Substring(ints[j].ToString().Length / 2));
                    temp.Add(left);
                    temp.Add(right);
                }
                else
                {
                    temp.Add(2024 * ints[j]);
                }
            }
            ints = temp;
        }

        static void GetNextStonespt2(ref HashSet<long> stones, ref Dictionary<long, long> counts)
        {
            HashSet<long> tempstones = new HashSet<long>();
            Dictionary<long, long> tempcounts = new Dictionary<long, long>();
            foreach (long stone in stones)
            {
                int length = (int)Math.Floor(Math.Log10(stone) + 1);

                if (stone == 0)
                {
                    if (tempstones.Add(1)) tempcounts.Add(1, 0);
                    tempcounts[1] += counts[stone];
                }
                else if (length % 2 == 0)
                {
                    var left = (long)(stone / Math.Pow(10, length / 2));
                    var right = (long)(stone % Math.Pow(10, length / 2));
                    if (tempstones.Add(left)) tempcounts.Add(left, 0);
                    tempcounts[left] += counts[stone];
                    if (tempstones.Add(right)) tempcounts.Add(right, 0);
                    tempcounts[right] += counts[stone];
                }
                else
                {
                    long val = stone * 2024;
                    if (tempstones.Add(val)) tempcounts.Add(val, 0);
                    tempcounts[val] += counts[stone];
                }
            }

            stones = tempstones;
            counts = tempcounts;
        }

        static long SOL2(string input)
        {
            string[] strlist = input.Split(' ');
            HashSet<long> stones = new HashSet<long>();
            Dictionary<long, long> counts = new Dictionary<long, long>();
            foreach (string str in strlist)
            {
                stones.Add(long.Parse(str));
                counts.Add(long.Parse(str), 1);
            }
            for (int i = 0; i < 75; i++)
            {
                GetNextStonespt2(ref stones, ref counts);
            }
            return counts.Values.Sum();
        }
    }
}
