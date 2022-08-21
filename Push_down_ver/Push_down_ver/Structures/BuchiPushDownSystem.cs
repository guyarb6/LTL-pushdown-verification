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


        
    }

    
    public class BuchiPushDownSystem
    {

        public PDS p;
        public NBA n;
        public LinkedList<BpdsNode> nodes = new LinkedList<BpdsNode>();

        //Our Buchi Push Down System will be defined as follows:
        public int qSize;
        public HashSet<int>[] AtomicPropList;
        public int alphabetSize;
        public bool[] fList;
        public bool[] initList;
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
            alphabetSize = p.alphabetSize;
            qSize = nodes.Count;
            fList = new bool[qSize];
            initList = new bool[qSize];
            AtomicPropList = new HashSet<int>[qSize];

            foreach(BpdsNode n1 in nodes)
            {
                fList[n1.id] = n1.F;
                AtomicPropList[n1.id] = n1.AtomicProp;

                initList[n1.id] = n1.init;
            }
            positiveDelta = new SparseMatrix<int>(qSize, qSize);
            negativeDelta = new SparseMatrix<int>(qSize, qSize);
            foreach (BpdsNode n1 in nodes)
            {
                foreach( BpdsNode n2 in nodes)
                {
                    if (PossiblePositiveTransition(n1, n2))
                    {
                        positiveDelta[n1.id, n2.id] = transitionAlphabet;
                    }else if (PossibleNegativeTransition(n1, n2))
                    {
                        negativeDelta[n1.id, n2.id] = transitionAlphabet;
                    }
                }
            }
        }


        private int transitionAlphabet;
        private bool PossiblePositiveTransition(BpdsNode from, BpdsNode to)
        {
            if (from.nbaNode.neighbor.Contains(to.nbaNode) && (!p.positiveDelta.IsCellEmpty(from.pdsNode.id,to.pdsNode.id)))
            {
                transitionAlphabet = positiveDelta[from.pdsNode.id, to.pdsNode.id];
                return true;
            }
            return false;
        }

        private bool PossibleNegativeTransition(BpdsNode from, BpdsNode to)
        {
            if (from.nbaNode.neighbor.Contains(to.nbaNode) && (!p.negativeDelta.IsCellEmpty(from.pdsNode.id, to.pdsNode.id)))
            {
                transitionAlphabet = negativeDelta[from.pdsNode.id, to.pdsNode.id];
                return true;
            }
            return false;
        }





        private SparseMatrix<bool> epsilonDelta;

        private Stack<Tuple<int, int>> s;
        private void addToEpsilon(int from, int to)
        {
            if(epsilonDelta.IsCellEmpty(from, to))
            {
                epsilonDelta[from, to] = true;
                s.Push(new Tuple<int, int>(from, to));
            }
        }

        private void addDirect(int from, int to)
        {
            
            Dictionary<int, int> preFrom = positiveDelta.getCol(from);
            Dictionary<int, int> postTo = positiveDelta.getRow(to);

            foreach(var f in preFrom)
            {
                foreach(var t in postTo)
                {
                    if (f.Value == t.Value)
                    {
                        addToEpsilon(f.Key, t.Key);
                    }
                }
            }
        }
        private void addTrans(int from, int to)
        {

        }
        private void setEpsilon()
        {
            s = new Stack<Tuple<int, int>>();
            epsilonDelta = new SparseMatrix<bool>(qSize,qSize);
            //set init
            for (int i = 0; i < qSize; i++)
            {
                if (initList[i])
                {
                    s.Push(new Tuple<int, int>(i, i));
                }
            }

            while (s.Count > 0)
            {
                Tuple<int, int> a = s.Pop();
                //add directs

                //add transitive
            }
        }
    }
}
