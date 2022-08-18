using System;
using System.Collections.Generic;
using System.Text;

namespace Push_down_ver.Structures
{

    public class NbaNode
    {

        public bool[] metaFormulaAssigmnet;//this list is of sub formulas from which it is created
        //if this node is initial
        public bool init;

        public LinkedList<NbaNode> neighbor;

        public bool[] F;

        //each atomic name 'i' is in this node iff AtomicProp[i]==true
        public HashSet<int> AtomicProp;

        public bool visited = false;
    }
    public class NBA
    {
    }
}
