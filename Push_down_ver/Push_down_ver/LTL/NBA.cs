using System;
using System.Collections.Generic;
using System.Text;

namespace Push_down_ver.LTL
{

    public class NbaNode
    {

        //------------------------------------------------------

        //each node is constructed from number and GnbaNode
        public int index;//index in F list
        public GnbaNode b;
        

        //------------------------------------------------------

        //call in case F is empty.
        public NbaNode(GnbaNode n)
        {
            
            this.b = n;
            b.metaSingleNba = this;
            init = n.init;
            AtomicProp = n.AtomicProp;
            F = true;
        }


        //call in case F isnt empty
        public NbaNode(GnbaNode n, int index)
        {
            this.index = index;
            this.b = n;
            n.metaNba[index] = this;
            
            AtomicProp = n.AtomicProp;
            if (index == 0)
            {
                F = n.F[0];
                init = n.init;
            }
        }


        //add neighbor when F is empty
        public void SetSimpleNeighbor()
        {
            foreach(GnbaNode nei in b.neighbor)
            {
                neighbor.AddFirst(nei.metaSingleNba);
            }
        }


        //add neighbor when F isnt empty
        public void SetNeighbor(int fSize)
        {
            int i = index;
            if (b.F[index])
            {
                i++;
                i = i % fSize;
            }
            foreach(GnbaNode nei in b.neighbor)
            {
                neighbor.AddFirst(nei.metaNba[i]);
            }
        }

        public bool init=false;

        public LinkedList<NbaNode> neighbor = new LinkedList<NbaNode>();

        public bool F=false;

        //each atomic name 'i' is in this node iff AtomicProp[i]==true
        public HashSet<int> AtomicProp;

        public bool visited = false;
    }

    public class NBA
    {


        
        private int fSize;
        
        private LinkedList<NbaNode> nodes = new LinkedList<NbaNode>();
        


        //set nodes:
        private void SetNodes(GNBA g)
        {
            
            foreach(GnbaNode n in g.nodes)
            {
                for(int j=0; j < fSize; j++)
                {
                    nodes.AddFirst( new NbaNode(n, j));
                }
                
            }
        }

        private void SetSimpleNodes(GNBA g)
        {
            
            foreach (GnbaNode n in g.nodes)
            {
                nodes.AddFirst(new NbaNode(n));
            }
        }

        
        
        public NBA(GNBA g)
        {
            fSize = g.nodes[0].F.Length;
            
            //3 cases, empty F, F with only one set or more than 1 set:

            if (fSize == 0)//if F is empty all node will be accepting
            {
                
                SetSimpleNodes(g);
                foreach (NbaNode n in nodes)
                {
                    n.SetSimpleNeighbor();
                }
            }
            else
            {
                
                SetNodes(g);
                foreach (NbaNode n in nodes)
                {
                    n.SetNeighbor(fSize);
                }
            }
            
            
            //set init list


        }


    }
}
