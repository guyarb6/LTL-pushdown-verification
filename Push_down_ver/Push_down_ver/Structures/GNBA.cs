using System;
using System.Collections.Generic;
using System.Text;
using Push_down_ver.LTL;
namespace Push_down_ver.Structures
{

    public class GnbaNode
    {

        public bool[] metaFormulaAssigmnet;//this list is of sub formulas from which it is created
        //if this node is initial
        public bool init;

        public LinkedList<GnbaNode> neighbor;

        public bool[] F;

        //each atomic name 'i' is in this node iff AtomicProp[i]==true
        public HashSet<int> AtomicProp;

        public bool visited = false;
    }
    public class GNBA
    {
        private LTLFormula root;


        public List<GnbaNode> nodes = new List<GnbaNode>(); 

        public List<GnbaNode> iniNodes = new List<GnbaNode>();


        public GNBA(LTLFormula root)
        {
            this.root = root;

            //set nodes
            foreach (bool[] assignment in LTLFormula.GetElementaryAssignments(root))
            {
                nodes.Add(CreateNode(assignment));
            }

            //add neighbors
            foreach (GnbaNode n in nodes)
            {
                n.neighbor = neighbors(n);
            }

            //set inits
            foreach(GnbaNode n in nodes)
            {
                if (n.init)
                {
                    iniNodes.Add(n);
                }
            }
            //add reachability
        }

        private LinkedList<GnbaNode> neighbors(GnbaNode n)
        {
            LinkedList<GnbaNode> l = new LinkedList<GnbaNode>();
            foreach(GnbaNode m in nodes)
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
        private GnbaNode CreateNode(bool[] assignment)
        {
            GnbaNode n = new GnbaNode();
            n.init = LTLFormula.InitSet(assignment, root);
            n.metaFormulaAssigmnet = assignment;
            n.F = LTLFormula.FList(assignment);
            n.AtomicProp = LTLFormula.AtomicSet(assignment);
            return n;
        }
    }
}
