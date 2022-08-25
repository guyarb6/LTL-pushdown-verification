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
        public bool F = false;
        public HashSet<int> AtomicProp;
        public BpdsNode(NbaNode nbaNode, PdsNode pdsNode)
        {
            this.nbaNode = nbaNode;
            this.pdsNode = pdsNode;
            AtomicProp = nbaNode.AtomicProp;
            id = idCounter++;

            if (nbaNode.init && pdsNode.init)
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

            foreach (NbaNode nbaNode in n.nodes)
            {
                foreach (PdsNode pdsNode in p.nodes)
                {
                    if (sameAtomicProp(nbaNode, pdsNode))
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

            foreach (BpdsNode n1 in nodes)
            {
                fList[n1.id] = n1.F;
                AtomicPropList[n1.id] = n1.AtomicProp;

                initList[n1.id] = n1.init;
            }
            positiveDelta = new SparseMatrix<int>(qSize, qSize);
            negativeDelta = new SparseMatrix<int>(qSize, qSize);
            foreach (BpdsNode n1 in nodes)
            {
                foreach (BpdsNode n2 in nodes)
                {
                    if (PossiblePositiveTransition(n1, n2))
                    {
                        positiveDelta[n1.id, n2.id] = transitionAlphabet;
                    }
                    if (PossibleNegativeTransition(n1, n2))
                    {
                        negativeDelta[n1.id, n2.id] = transitionAlphabet;
                    }
                }
            }
        }


        private int transitionAlphabet;
        private bool PossiblePositiveTransition(BpdsNode from, BpdsNode to)
        {
            if (from.nbaNode.neighbor.Contains(to.nbaNode) && (!p.positiveDelta.IsCellEmpty(from.pdsNode.id, to.pdsNode.id)))
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



        //motheds to create epsilon transitions -------------------------------------------------------------


        //will be used to create both all epsilon paths and f-epsilon paths:
        private SparseMatrix<bool> epsilonDelta;

        private Stack<Tuple<int, int>> s;
        private void addToEpsilon(int from, int to)
        {
            if (epsilonDelta.IsCellEmpty(from, to))
            {
                epsilonDelta[from, to] = true;
                s.Push(new Tuple<int, int>(from, to));
            }
        }



        private void addDirect(int from, int to)
        {

            Dictionary<int, int> preFrom = positiveDelta.getCol(from);
            Dictionary<int, int> postTo = positiveDelta.getRow(to);

            foreach (var f in preFrom)
            {
                foreach (var t in postTo)
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
            Dictionary<int, bool> transFrom = epsilonDelta.getCol(from);
            Dictionary<int, bool> transTo = epsilonDelta.getRow(to);

            foreach (var t in transFrom)
            {
                addToEpsilon(t.Key, from);
            }
            foreach (var t in transTo)
            {
                addToEpsilon(to, t.Key);
            }
        }
        private void setEpsilon()
        {
            s = new Stack<Tuple<int, int>>();
            epsilonDelta = new SparseMatrix<bool>(qSize, qSize);
            //set init
            for (int i = 0; i < qSize; i++)
            {

                s.Push(new Tuple<int, int>(i, i));

            }

            while (s.Count > 0)
            {
                Tuple<int, int> a = s.Pop();
                //add directs
                addDirect(a.Item1, a.Item2);
                //add transitive
                addTrans(a.Item1, a.Item2);
            }
        }


        //methods to create f-epsilon transitions----------------------------------------------------------
        private void setFEpsilon()
        {
            setEpsilon();

            s = new Stack<Tuple<int, int>>();
            var allEpsilonDelta = epsilonDelta;
            epsilonDelta = new SparseMatrix<bool>(qSize, qSize);
            //set init
            for (int i = 0; i < qSize; i++)
            {
                if (fList[i])
                {
                    s.Push(new Tuple<int, int>(i, i));
                }


            }

            while (s.Count > 0)
            {
                Tuple<int, int> a = s.Pop();
                //add directs
                addDirect(a.Item1, a.Item2);
                //add transitive
                addFTrans(a.Item1, a.Item2, allEpsilonDelta);
            }
        }

        private void addFTrans(int from, int to, SparseMatrix<bool> allEpsilonDelta)
        {
            Dictionary<int, bool> transFrom = allEpsilonDelta.getCol(from);
            Dictionary<int, bool> transTo = allEpsilonDelta.getRow(to);

            foreach (var t in transFrom)
            {
                addToEpsilon(t.Key, from);
            }
            foreach (var t in transTo)
            {
                addToEpsilon(to, t.Key);
            }
        }



        //used for DFS search:


        private int calledCounter;

        private bool[] called;//for first step in CSS, accsseed via index
        

        
        private bool[] tranposeCalled;//for second step in CSS, transpose graph.


        private int[] topoSort;// contains elements according to top-sort
        public bool CheckForNestedF() 
        {
            called = new bool[qSize];
            calledCounter = qSize;
            topoSort = new int[qSize];
            
            tranposeCalled = new bool[qSize];

            setEpsilon();
            foreach(BpdsNode n1 in nodes)
            {
                if (n1.init)
                {
                    dfs(n1.id);
                }
            }

            
            HashSet<HashSet<int>> components = new HashSet<HashSet<int>>();
            for (int i= calledCounter; i < qSize; i++)
            {
                int id = topoSort[i];
                if (tranposeCalled[id] == false)
                {
                    SCCItems = new HashSet<int>();
                    components.Add(SCCItems);
                    tranposeDfs(id);
                    
                }
            }

            setFEpsilon();
            foreach (HashSet<int> scc in components)
            {
                if (FInSCC(scc))
                {
                    return true;
                }
            }
            return false;
        }
        
        private void dfs(int i)
        {
            if (called[i])
            {
                return;
            }
            called[i] = true;
            
            foreach(var j in positiveDelta.getRow(i))
            {
                dfs(j.Key);
            }
            foreach(var j in epsilonDelta.getRow(i))
            {
                dfs(j.Key);
            }
            topoSort[--calledCounter] = i;
        }

        //used for storing element in transpose DFS
        public HashSet<int> SCCItems;

        private void tranposeDfs(int i)
        {
            if (tranposeCalled[i])
            {
                return;
            }
            tranposeCalled[i] = true;

            foreach (var j in positiveDelta.getCol(i))
            {
                tranposeDfs(j.Key);
            }
            foreach (var j in epsilonDelta.getCol(i))
            {
                tranposeDfs(j.Key);
            }
            SCCItems.Add(i);
        }



        //called after setFEpsilon(), so epsilonDelta is only f epsilons.
        private bool FInSCC(HashSet<int> scc)
        {
            foreach(int i in scc)
            {
                if (fList[i]&&(scc.Count>1)) 
                {
                    return true;
                }
                    
            }
            foreach(int i in scc)
            {
                foreach(int j in scc)
                {
                    if (epsilonDelta[i, j])
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }

    
}
