using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Day one part 2");
            var p1a = new P1A();
            Console.WriteLine(p1a.GetFuel());

            Console.WriteLine("Day 2 Part 1");
            var d2 = new Day2();
            Console.WriteLine(d2.Run(12,2));
            Console.WriteLine("part2");
            Console.WriteLine(d2.FindResult(19690720));





            Console.ReadLine();
        }

        
    }
}
