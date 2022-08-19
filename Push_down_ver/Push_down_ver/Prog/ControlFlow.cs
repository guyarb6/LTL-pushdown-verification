using System;
using System.Collections.Generic;
using System.Text;
using Push_down_ver.Structures;
namespace Push_down_ver.Prog
{
    public abstract class IpNode
    {
        


        public bool visited = false;
        
        public static int nameCounter = 0;


        public int ip;
        public HashSet<int> AtomicProp;

        public IpNode(HashSet<int> AtomicProp)
        {
            this.ip = nameCounter++;
            this.AtomicProp = AtomicProp;
        }



        public abstract void SetNodes();
        
    }

    public class ReturnNode : IpNode
    {
        public static LinkedList<ReturnNode> List = new LinkedList<ReturnNode>();

        public ReturnNode(HashSet<int> AtomicProp) : base( AtomicProp) { }

        public override void SetNodes()
        {
            if (visited)
            {
                return;
            }
            visited = true;
            List.AddFirst(this);
        }
    }

    public class CallNode : IpNode
    {

        public static LinkedList<CallNode> List = new LinkedList<CallNode>();

        public IpNode callIp;
        public IpNode next;
        public CallNode(HashSet<int> AtomicProp) : base( AtomicProp) {  }

        public void setCallIp(IpNode callIp, IpNode next)
        {
            this.callIp = callIp;
            

            this.next = next;
        }

        public override void SetNodes()
        {
            if (visited)
            {
                return;
            }
            visited = true;
            List.AddFirst(this);


            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

            //assume callIp is not null. Must set it before

            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            next.SetNodes();
        }
    }

    public class NonDeterministicCallNode : IpNode
    {

        public static LinkedList<NonDeterministicCallNode> List = new LinkedList<NonDeterministicCallNode>();

        public IpNode callIp1;
        public IpNode next1;
        public IpNode callIp2;
        public IpNode next2;
        public NonDeterministicCallNode( HashSet<int> AtomicProp) : base(AtomicProp) { }

        public void setCallIp1(IpNode callIp, IpNode next1)
        {
            this.callIp1 = callIp;
            
            this.next1 = next1;
        }
        public void setCallIp2(IpNode callIp, IpNode next2)
        {
            this.callIp2 = callIp;
            
            this.next2 = next2;
        }

        public override void SetNodes()
        {
            if (visited)
            {
                return;
            }
            visited = true;
            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

            //assume callIp is not null. Must set it before

            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            next1.SetNodes();
            next2.SetNodes();
        }
    }



    public class ControlFlow
    {
        

        private LinkedList<IpNode> functions = new LinkedList<IpNode>();
        private IpNode mainNode;

        public void AddMainFunction(IpNode n )
        {
            mainNode = n;
        }

        public void AddFunction(IpNode n)
        {
            functions.AddFirst(n);
        }

        private SparseMatrix<int> AddPositiveDelta()
        {
            SparseMatrix<int> result= new SparseMatrix<int>(IpNode.nameCounter, IpNode.nameCounter);
            return result;
        }


        private SparseMatrix<int> AddNegativeDelta()
        {
            SparseMatrix<int> result = new SparseMatrix<int>(IpNode.nameCounter, IpNode.nameCounter);
            return result;
        }
    }
}
