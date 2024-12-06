using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines("input.txt");
            List<int> line1 = new List<int>();
            List<int> line2 = new List<int>();
            foreach(string line in lines)
            {
                string[] nums = line.Split(' ');
                line1.Add(int.Parse(nums[0].Trim()));
                line2.Add(int.Parse(nums[3].Trim()));
            }
            line1.Sort(); 
            line2.Sort();
            Console.WriteLine($"Solution1: {SOl1(line1,line2)}\nSolution2: {SOl2(line1,line2)}");
            Console.ReadKey();
        }

        static int SOl1(List<int> line1, List<int> line2)
        {
            int answer = 0; 
            for(int i = 0; i < line1.Count; i++) answer += Math.Abs(line2[i] - line1[i]);
            return answer;
        }

        static int SOl2(List<int> line1, List<int> line2)
        {
            int answer = 0;
            for(int i = 0; i < line1.Count; i++)
            {
                int amount = 0;
                for (int j = 0; j< line2.Count; i++)
                {
                    if (line1[i] == line2[j]) amount++;
                }
                answer += line1[i] * amount;
            }
            return answer;
        }
    }
}
