using System;
using System.Collections.Generic;
using System.Text;
using Push_down_ver.Structures;


namespace Push_down_ver.Prog
{
    public class PdsNode
    {
        public int id;
        public HashSet<int> AtomicProp;
        
    }

    public class PDS
    {

        public SparseMatrix<int> positiveDelta;
        public SparseMatrix<int> negativeDelta;

        public int alphabetSize;
        public PdsNode[] nodes;
        public int initNode;


        
    }
}
