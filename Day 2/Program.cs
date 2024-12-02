using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Win32.SafeHandles;

namespace Day_2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines("input.txt");
            List<string[]> strings = new List<string[]>();
            foreach (string line in lines)
            {
                string[] temp = line.Split(' ');
                strings.Add(temp);
            }
            Console.WriteLine($"Solution1: {SOl1(strings)}\nSolution2: {SOl2()}");
            Console.ReadKey();
        }

        static int SOl1(List<string[]> input)
        {
            int answer = 0;
            for(int i = 0; i < input.Count; i++)
            {
                bool safe = true;
                for(int j = 1;  j < input[i].Length; j++)
                {
                    int change= int.Parse(input[i][j]) - int.Parse(input[i][j-1]);
                    if(Math.Abs(change) > 3 || Math.Abs(change) < 1)
                    {
                        safe = false;
                        break;
                    }
                }
                if (safe) answer++;
            }
            return answer;
        }

        static int SOl2() 
        {
            int answer = 0;
            return answer;
        }
    }
}
