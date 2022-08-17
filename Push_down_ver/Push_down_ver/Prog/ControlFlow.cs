using System;
using System.Collections.Generic;
using System.Text;

namespace Push_down_ver.Prog
{
    public abstract class IpNode
    {
        public int id;
        public HashSet<int> AtomicProp;

        public IpNode(int id, HashSet<int> AtomicProp)
        {
            this.id = id;
            this.AtomicProp = AtomicProp;
        }

        public virtual bool isReturn()
        {
            return false;
        }
    }

    public class ReturnNode : IpNode
    {

        public ReturnNode(int id, HashSet<int> AtomicProp) : base(id, AtomicProp) { }

        public override bool isReturn()
        {
            return true;

        }
    }

    public class CallNode : IpNode
    {
        public int callId;
        public CallNode(int id, HashSet<int> AtomicProp, int callId) : base(id, AtomicProp) { this.callId = callId; }

        
    }

    public class NonDeterministicCallNode : IpNode
    {
        public int callId1;
        public int callId2;
        public NonDeterministicCallNode(int id, HashSet<int> AtomicProp, int callId1, int callId2) : base(id, AtomicProp) { this.callId1 = callId1; this.callId2 = callId2; }


    }



    public class ControlFlow
    {
        private int nameCounter = 0;
    }
}
