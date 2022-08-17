using System;
using System.Collections.Generic;
using System.Text;
using Push_down_ver.LTL;
namespace Push_down_ver.Structures
{

    public class NODE
    {

        public bool[] metaFormulaAssigmnet;//this list is of sub formulas from which it is created
        //if this node is initial
        public bool init;

        public LinkedList<NODE> neighbor;

        public bool[] F;

        //each atomic name 'i' is in this node iff AtomicProp[i]==true
        public HashSet<int> AtomicProp;

        public bool visited = false;
    }
    public class GNBA
    {
        private LTLFormula root;


        public List<NODE> nodes = new List<NODE>(); 

        public List<NODE> iniNodes = new List<NODE>();


        public GNBA(LTLFormula root)
        {
            this.root = root;

            //set nodes
            foreach (bool[] assignment in LTLFormula.GetElementaryAssignments(root))
            {
                nodes.Add(CreateNode(assignment));
            }

            //add neighbors
            foreach (NODE n in nodes)
            {
                n.neighbor = neighbors(n);
            }

            //set inits
            foreach(NODE n in nodes)
            {
                if (n.init)
                {
                    iniNodes.Add(n);
                }
            }
            //add reachability
        }

        private LinkedList<NODE> neighbors(NODE n)
        {
            LinkedList<NODE> l = new LinkedList<NODE>();
            foreach(NODE m in nodes)
            {
                if(LTLFormula.DeltaNextCondition(n.metaFormulaAssigmnet,m.metaFormulaAssigmnet)
                    &&
                    LTLFormula.DeltaUntilCondition(n.metaFormulaAssigmnet, m.metaFormulaAssigmnet))
                {
                    l.AddLast(m);
                }
            }

            return l;
        }

        //create node
        private NODE CreateNode(bool[] assignment)
        {
            NODE n = new NODE();
            n.init = LTLFormula.InitSet(assignment, root);
            n.metaFormulaAssigmnet = assignment;
            n.F = LTLFormula.FList(assignment);
            n.AtomicProp = LTLFormula.AtomicSet(assignment);
            return n;
        }
    }
}
