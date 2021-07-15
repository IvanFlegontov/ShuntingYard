using System;
using System.Collections.Generic;
using System.Text;

namespace sorting_station
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                string s;
                s = Console.ReadLine();

                var parser = new Parser();
                var sh = new ShuntingYard();

                foreach (var token in parser.GetToken(s))
                {
                    Console.WriteLine($"   {token}");
                }

                if(sh.Evaluate(s))
                    Console.WriteLine($" value = {sh.Value}");
                else
                    Console.WriteLine($" Can't evaluate");
            }
        }
    }
}
