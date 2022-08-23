using System;
using System.Collections.Generic;
using Push_down_ver.LTL;
using Push_down_ver.Structures;
using Push_down_ver.Prog;
namespace Push_down_ver
{
    class Program
    {

        //we will have 3 Atomic properties:
        //0- At this Ip up will be called
        //1- At this Ip down will be called
        //2- At this Ip left will be called

        public static ControlFlow exampleProgram()
        {
            //set

            ControlFlow cf = new ControlFlow();


            //basic atomic propoerites:

            //set up function
            HashSet<int> upHash = new HashSet<int>();
            upHash.Add(0);
            ReturnNode up = new ReturnNode(upHash);


            //set down function
            HashSet<int> downHash = new HashSet<int>();
            downHash.Add(1);
            ReturnNode down = new ReturnNode(downHash);

            //set left function
            HashSet<int> leftHash = new HashSet<int>();
            leftHash.Add(2);
            ReturnNode left = new ReturnNode(leftHash);



            //simple return:
            HashSet<int> empty = new HashSet<int>();
            ReturnNode returnIp = new ReturnNode(empty);
            
            //main:
            CallNode startProgram = new CallNode(empty);
            CallNode endProgram = new CallNode(empty);//call itself

            //s:
            NonDeterministicCallNode s1 = new NonDeterministicCallNode(empty);
            CallNode s2 = new CallNode(empty);
            CallNode s3 = new CallNode(empty);
            CallNode s4 = new CallNode(empty);
            CallNode s5 = new CallNode(empty);

            //m:
            NonDeterministicCallNode m1 = new NonDeterministicCallNode(empty);
            CallNode m2 = new CallNode(empty);
            CallNode m3 = new CallNode(empty);
            CallNode m4 = new CallNode(empty);
            CallNode m6 = new CallNode(empty);
            return null;
        }

        
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
