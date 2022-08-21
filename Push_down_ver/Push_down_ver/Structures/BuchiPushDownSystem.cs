using System;
using System.Collections.Generic;
using System.Text;
using Push_down_ver.Prog;
using Push_down_ver.LTL;
namespace Push_down_ver.Structures
{
    public class BpdsNode
    {

        public static int idCounter = 0;

        public int id;
        public NbaNode nbaNode;
        public PdsNode pdsNode;


        public bool init;
        public bool F=false;
        public HashSet<int> AtomicProp;
        public BpdsNode(NbaNode nbaNode, PdsNode pdsNode)
        {
            this.nbaNode = nbaNode;
            this.pdsNode = pdsNode;
            AtomicProp = nbaNode.AtomicProp;
            id = idCounter++;

            if(nbaNode.init && pdsNode.init)
            {
                this.init = true;
            }
            if (nbaNode.F)
            {
                F = true;
            }

        }


        public LinkedList<BpdsNode> neighbor = new LinkedList<BpdsNode>();
    }

    
    public class BuchiPushDownSystem
    {

        public PDS p;
        public NBA n;
        public LinkedList<BpdsNode> nodes = new LinkedList<BpdsNode>();

        //Our Buchi Push Down System will be defined as follows:
        public int qSize;
        public HashSet<int>[] AtomicProp;
        public int alphabetSize;
        public bool[] fList;

        public SparseMatrix<int> positiveDelta;
        public SparseMatrix<int> negativeDelta;

        private bool sameAtomicProp(NbaNode nbaNode, PdsNode pdsNode)
        {
            return nbaNode.AtomicProp.SetEquals(pdsNode.AtomicProp);
        }


        public BuchiPushDownSystem(PDS p, NBA n)
        {

            this.p = p;
            this.n = n;
            
            foreach(NbaNode nbaNode in n.nodes)
            {
                foreach(PdsNode pdsNode in p.nodes)
                {
                    if(sameAtomicProp(nbaNode, pdsNode))
                    {
                        nodes.AddFirst(new BpdsNode(nbaNode, pdsNode));
                    }
                    
                }
            }

        }

        private bool PossiblePositiveTransition(BpdsNode from, BpdsNode to)
        {
            if (from.nbaNode.neighbor.Contains(to.nbaNode) && (!p.positiveDelta.IsCellEmpty(from.pdsNode.id,to.pdsNode.id)))
            {
                return true;
            }
            return false;
        }

        private bool PossibleNegativeTransition(BpdsNode from, BpdsNode to)
        {
            if (from.nbaNode.neighbor.Contains(to.nbaNode) && (!p.negativeDelta.IsCellEmpty(from.pdsNode.id, to.pdsNode.id)))
            {
                return true;
            }
            return false;
        }
    }
}
