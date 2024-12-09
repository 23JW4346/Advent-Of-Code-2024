using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day_9
{
    internal class Program
    {

        public struct file
        {
            public int Id;
            public int Length;
        }

        static void Main(string[] args)
        {
            string input = File.ReadAllText("input.txt");
            Console.WriteLine("Part 1: " + SOL1(input));
            Console.WriteLine("Part 2: " + SOL2(input));
            Console.ReadKey();
        }

        static List<file> ReadDisk(string disks)
        {
            List<file> outlist = new List<file>();
            int k = 0;
            for (int i = 0; i < disks.Length; i++)
            {
                if (i % 2 == 0)
                {
                    file tempfile;
                    tempfile.Id = k;
                    tempfile.Length = int.Parse(disks[i].ToString());
                    for (int j = 0; j < tempfile.Length; j++) outlist.Add(tempfile);
                    k++;
                }
                else
                {
                    file nullfile;
                    nullfile.Id = -1;
                    nullfile.Length = int.Parse(disks[i].ToString());
                    for (int j = 0; j < nullfile.Length; j++) outlist.Add(nullfile);
                }
            }
            return outlist;
        }

        static List<file> Collapse(List<file> disk)
        {
            for (int i = 0; i < disk.Count; i++)
            {
                if (disk[i].Id == -1)
                {
                    for (int j = disk.Count - 1; j > i; j--) 
                    {
                        if (disk[j].Id != -1) 
                        {
                            file tempFile = disk[i];
                            disk[i] = disk[j];
                            disk[j] = tempFile;
                            break;
                        }
                    }
                }
            }
            return disk;
        }

        static long SOL1(string disks)
        {
            List<file> sortedDisk = Collapse(ReadDisk(disks));
            List<long> answers = new List<long>();
            for (int i = 0; i < sortedDisk.Count; i++)
            {
                if (sortedDisk[i].Id != -1)
                {
                    answers.Add(sortedDisk[i].Id * i);
                }
            }
            return answers.Sum();
        }

        static long SOL2(string disks)
        {
            List<file> sorted = Defragment(ReadDisk(disks));
            List<long> answers = new List<long>();
            for (int i = 0; i < sorted.Count; i++)
            {
                if (sorted[i].Id != -1) answers.Add(sorted[i].Id * i);
            }
            return answers.Sum();
        }

        static List<file> Defragment(List<file> disk)
        {
            for (int i = 0; i < disk.Count; i++)
            {
                if (disk[i].Id == -1)
                {
                    for (int j = disk.Count - 1; j > i; j--)
                    {
                        if (disk[j].Id != -1)
                        {
                            if (disk[i].Length >= disk[j].Length)
                            {
                                int startIndex = (j - disk[j].Length) + 1;
                                int length = disk[i].Length;
                                for (int k = 0; k < length; k++)
                                {
                                    file nullFile;
                                    nullFile.Id = -1;
                                    nullFile.Length = length - disk[j].Length;
                                    disk[i + k] = nullFile;
                                }
                                for (int k = 0; k < disk[j].Length; k++)
                                {
                                    file tempFile = new file();
                                    disk[i + k] = disk[startIndex + k];
                                    disk[startIndex + k] = tempFile;
                                }
                                break;
                            }
                        }
                    }
                }
            }
            return disk;
        }
    }
}
