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
        private bool sameAtomicProp(NbaNode nbaNode, PdsNode pdsNode)
        {
            return nbaNode.AtomicProp.SetEquals(pdsNode.AtomicProp);
        }


        public BuchiPushDownSystem(PDS p, NBA n)
        {
            this.p = p;
            this.n = n;
        }

        private bool PossiblePositiveTransition(BpdsNode from, BpdsNode to)
        {
            if (from.nbaNode.neighbor.Contains(to.nbaNode) && (!p.positiveDelta.IsCellEmpty(from.pdsNode.id,to.pdsNode.id)))
            {
                return 
            }
            return true;
        }
    }
}
