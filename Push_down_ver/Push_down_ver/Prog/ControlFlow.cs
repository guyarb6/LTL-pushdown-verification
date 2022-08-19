using System;
using System.Collections.Generic;
using System.Text;

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



        public virtual bool isReturn()
        {
            return false;
        }
    }

    public class ReturnNode : IpNode
    {

        public ReturnNode(HashSet<int> AtomicProp) : base( AtomicProp) { }

        public override bool isReturn()
        {
            return true;

        }
    }

    public class CallNode : IpNode
    {
        public IpNode callIp;
        public CallNode(HashSet<int> AtomicProp) : base( AtomicProp) {  }

        public void setCallIp(IpNode callIp)
        {
            this.callIp = callIp;
        }
    }

    public class NonDeterministicCallNode : IpNode
    {
        public IpNode callIp1;
        public IpNode callIp2;
        public NonDeterministicCallNode( HashSet<int> AtomicProp) : base(AtomicProp) { }

        public void setCallIp1(IpNode callIp)
        {
            this.callIp1 = callIp;
        }
        public void setCallIp2(IpNode callIp)
        {
            this.callIp2 = callIp;
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


    }
}
