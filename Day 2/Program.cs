using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Win32.SafeHandles;
using System.Linq.Expressions;

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
            Console.WriteLine($"Solution1: {SOl1(strings.ToArray())}\nSolution2: {SOl2(strings.ToArray())}");
            Console.ReadKey();
        }

        static int SOl1(string[][] input)
        {
            int answer = 0;
            for (int i = 0; i < input.Length; i++)
            {
                if (IsSafe(input[i])) answer++;
            }
            return answer;
        }

        static bool IsSafe(string[] input)
        {
            bool firstpass = true;
            bool safe = true;
            bool gradispositive = true;
            for(int i = 1; i < input.Length; i++) 
            {
                int change = int.Parse(input[i]) - int.Parse(input[i-1]);
                if (firstpass)
                {
                    if (change > 0)
                    {
                        gradispositive = true;
                    }
                    else gradispositive = false;
                    firstpass = false;
                }
                if (gradispositive)
                {
                    if (change > 3 || change < 1)
                    {
                        safe = false;
                        break;
                    }
                }
                else
                {
                    if (change < -3 || change > -1)
                    {
                        safe = false;
                        break;
                    }
                }
            }
            return safe;
        }

        static int SOl2(string[][] input ) 
        {
            int answer = 0;
            for(int i = 0; i<input.Length; i++)
            {
                bool safeMisOne = false;
                List<string[]> strings = GetStrings(input[i]);
                if (IsSafe(input[i])) answer++;
                else
                {
                    foreach (string[] lines in strings)
                    {
                        if (IsSafe(lines)) safeMisOne = true;
                    }
                    if (safeMisOne) answer++;
                }
            }
            return answer;
        }

        static List<string[]> GetStrings(string[] input)
        {
            List<string[]> output = new List<string[]>();
            for (int i = 0; i < input.Length; i++)
            {
                List<string> temp = new List<string>();
                for (int j = 0; j < input.Length; j++)
                {
                    if (i != j) temp.Add(input[j]);
                }
                output.Add(temp.ToArray());
            }
            return output;
        }
    }
}
