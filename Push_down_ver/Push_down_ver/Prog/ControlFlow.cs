using System;
using System.Collections.Generic;
using System.Text;

namespace Push_down_ver.Prog
{
    public abstract class FunctionNode
    {
        public int id;
        public HashSet<int> AtomicProp;
    }

    

    public class ControlFlow
    {
        private int nameCounter = 0;
    }
}
