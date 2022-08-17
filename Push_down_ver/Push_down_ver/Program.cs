using System;
using Push_down_ver.LTL;
using Push_down_ver.Structures;
namespace Push_down_ver
{
    class Program
    {
        static void Main(string[] args)
        {
            var a = new Atomic(1);
            var b = new NegFormula(a);
            
            var d = new NextFormula(b);
            var e = new Until(a, d);
            var c = new GNBA(a);
            Console.WriteLine("Hello World!");
            Console.WriteLine(c.nodes.Count);
        }
    }
}
