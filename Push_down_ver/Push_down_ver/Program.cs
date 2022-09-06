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

        
        //example program
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

            //main control ip:
            CallNode startProgram = new CallNode(empty);
            CallNode endProgram = new CallNode(empty);//call itself

            //s control ip:
            NonDeterministicCallNode s1 = new NonDeterministicCallNode(empty);
            CallNode s2 = new CallNode(empty);
            CallNode s3 = new CallNode(empty);

            //m control ip:
            NonDeterministicCallNode m1 = new NonDeterministicCallNode(empty);
            CallNode m2 = new CallNode(empty);
            CallNode m3 = new CallNode(empty);
            CallNode m4 = new CallNode(empty);

            //set main control flow
            startProgram.setCallIp(s1, endProgram);
            endProgram.setCallIp(endProgram, endProgram);

            //set s control flow
            s1.setCallIp1(returnIp, returnIp);
            s1.setCallIp2(up, s2);
            s2.setCallIp(m1, s3);
            s3.setCallIp(down, returnIp);


            //set m control flow
            m1.setCallIp1(s1, m2);
            m2.setCallIp(left, returnIp);

            m1.setCallIp2(up, m3);
            m3.setCallIp(m1, m4);
            m4.setCallIp(down, returnIp);
            cf.SetMainFunction(startProgram);
            cf.AddFunction(startProgram);
            cf.AddFunction(m1);
            cf.AddFunction(s1);
            cf.AddFunction(up);
            cf.AddFunction(down);
            cf.AddFunction(left);
            return cf;
        }




        //-----------------------------LTL extra syntax----------------------------
        public static LTLFormula WeakUntil(LTLFormula f1, LTLFormula f2)
        {
            return new OrFormula(new Until(f1, f2), AlwaysFormula(f1));
        }

        public static LTLFormula AlwaysFormula(LTLFormula f)
        {
            return new NegFormula(NotAlwaysFormula(f));
        }

        public static LTLFormula NotAlwaysFormula(LTLFormula f)
        {
            var notF = new NegFormula(f);
            var eventualyNotF = new Until(new TrueFormula(), notF);
            return eventualyNotF;
        }



        //----------------------------tests------------------------------------------
        public static LTLFormula test1()
        {
            Atomic up = new Atomic(0);
            Atomic down = new Atomic(1);
            Atomic left = new Atomic(2);

            
            return NotAlwaysFormula(new OrFormula(new NegFormula(down), WeakUntil(new NegFormula(up), left)));
        }


        public static LTLFormula test2()
        {
            Atomic up = new Atomic(0);
            Atomic down = new Atomic(1);
            Atomic left = new Atomic(2);

            
            return NotAlwaysFormula(new OrFormula(new NegFormula(up), WeakUntil(new NegFormula(down), left)));
        }


        public static LTLFormula test3()
        {
            Atomic up = new Atomic(0);
            Atomic down = new Atomic(1);
            Atomic left = new Atomic(2);

            return NotAlwaysFormula(new OrFormula(new NegFormula(up), new Until(new NegFormula(down), left)));
            
        }


        public static LTLFormula test4()
        {
            Atomic up = new Atomic(0);
            Atomic down = new Atomic(1);
            Atomic left = new Atomic(2);


            return NotAlwaysFormula(new OrFormula(new NegFormula(down), new Until(new NegFormula(up), left)));
        }






        //-----------------------simple tests-------------------------------------

        public static LTLFormula test5()
        {
            Atomic up = new Atomic(0);
            Atomic down = new Atomic(1);
            Atomic left = new Atomic(2);


            return new OrFormula(new TrueFormula(), new OrFormula(new OrFormula(left, down), up));
        }

        public static LTLFormula test6()
        {
            Atomic up = new Atomic(0);
            Atomic down = new Atomic(1);
            Atomic left = new Atomic(2);


            return new AndFormula(new FalseFormula(), new AndFormula(new AndFormula(left, down), up));
        }

        public static LTLFormula test7()
        {
            Atomic up = new Atomic(0);
            Atomic down = new Atomic(1);
            Atomic left = new Atomic(2);


            return new NegFormula(new OrFormula(new OrFormula(left, down), new NextFormula(new NextFormula(up))));
        }

        public static LTLFormula test8()
        {
            Atomic up = new Atomic(0);
            Atomic down = new Atomic(1);
            Atomic left = new Atomic(2);


            return new OrFormula(new OrFormula(left, down), new NextFormula(up));
        }

        public static LTLFormula test9()
        {
            Atomic up = new Atomic(0);
            Atomic down = new Atomic(1);
            Atomic left = new Atomic(2);


            return new TrueFormula();
        }

        static void Main(string[] args)
        {
            ControlFlow prog = exampleProgram();
            var pds = prog.createPDS();
            LTLFormula f = test8();
            var gnba = new GNBA(f);
            var nba = new NBA(gnba);
            var buchiPushDownSystem = new BuchiPushDownSystem(pds, nba);
            if (buchiPushDownSystem.CheckForNestedF())
            {
                Console.WriteLine("error in program");
            }
        }
    }
}
