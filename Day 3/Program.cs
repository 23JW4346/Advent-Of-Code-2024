using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Day_3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string lines = File.ReadAllText("input.txt");
            Console.WriteLine($"Solution1: {SOl1(lines)}\nSolution2: {SOl2(lines)}");
            Console.ReadKey();

        }

        static int SOl1(string line)
        {
            int answer = 0;
            foreach (Match s in Regex.Matches(line, "mul\\([1-9][0-9]?[0-9]?\\,[1-9][0-9]?[0-9]?\\)")) answer += FindMul(s.Value);
            return answer;
        }

        static int FindMul(string temp)
        {
            int num1 = 0, num2 = 0;
            int indexoflastbracket = Array.IndexOf(temp.ToCharArray(), ')');
            if (indexoflastbracket == temp.Length - 1)
            {
                temp = temp.Replace(")", "");
            }
            else
            {
                temp = temp.Remove(indexoflastbracket);
            }
            temp = temp.Replace("(", "").Replace("mul", "");
            if (Regex.IsMatch(temp, "[1-9][0-9]?[0-9]?\\,[1-9][0-9]?[0-9]?"))
            {
                string[] split = temp.Trim().Split(',');
                num1 = int.Parse(split[0]);
                num2 = int.Parse(split[1]);
            }
            return num1 * num2;
        }

        static int SOl2(string lines)
        {
            int answer = 0;
            bool dont = false;
            foreach (Match s in Regex.Matches(lines, "mul\\([1-9][0-9]?[0-9]?\\,[1-9][0-9]?[0-9]?\\)|do\\(\\)|don't\\(\\)"))
            {
                if (s.Value == "don't()") dont = true;
                else if (s.Value == "do()") dont = false;
                if (s.Value.StartsWith("mul(") && !dont) answer += FindMul(s.Value);
            }
            return answer;
        }
    }
}
