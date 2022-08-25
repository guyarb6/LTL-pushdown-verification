using System;
using System.Collections.Generic;
using System.Text;

namespace Push_down_ver.LTL

{
    public abstract class LTLFormula
    {
        //must first call SetList() and set assignment.


        //------------------------set subFormulas list-----------------------------------------
        public static List<LTLFormula> subFormulas;

        public static List<Atomic> atomicFormulas;

        public static List<NextFormula> nextFormulas;

        public static List<Until> untilFormulas;

        //assignment for every sub formula in list, 
        public static bool[] assignment;




        //-----------------basics methods and properies for all formulas-----------------------

        public virtual bool isTrue()
        {
            return assignment[index];
        }

        public bool added = false;//used to add each sub-formula once.
        public int index;//each sub-formula will be given index of corrsponding 
                         //set sub formulas list:


        public abstract void SetList();//add all sub formulas to subFormulas, return number of sub formulas



        //-------------------------assignments methods-----------------------------------------
        //all assignments will be bool[]
        


        //as init can be called only once so can GetElementaryAssignments
        public static List<bool[]> GetElementaryAssignments(LTLFormula root)
        {
            init(root);
            List<bool[]> all = GetAllAssignments();
            List<bool[]> result = new List<bool[]>();
            foreach(bool[] ass in all)
            {
                if (CheckElementaryAssignment(ass))
                {
                    result.Add(ass);
                }
                    
            }
            return result;
        }

        //conditions to create delta function for buchi automata from elementary sets(next and until)
        public static bool DeltaNextCondition(bool[] a, bool[] b)
        {
            foreach(NextFormula next in nextFormulas)
            {
                if (a[next.index]!= b[next.a.index])
                {
                    return false;
                }
                   
            }

            return true;
        }

        public static bool DeltaUntilCondition(bool[] a, bool[] b)
        {
            foreach (Until u in untilFormulas)
            {
                if (a[u.index] != ( a[u.r.index] || (a[u.l.index] && b[u.index]) ))
                {
                    return false;
                }
                          
            }

            return true;
        }

        //set of all atomic propositions (by name)
        public static HashSet<int> AtomicSet(bool[] l)
        {
            assignment = l;
            HashSet<int> result =new HashSet<int>();

            foreach (Atomic a in atomicFormulas)
            {
                if (a.isTrue())
                {
                    result.Add(a.name);
                }
            }
            return result;
        }

        public static bool[] FList(bool[] l)
        {
            assignment = l;
            bool[] result = new bool[untilFormulas.Count];
            int i = 0;
            foreach (Until a in untilFormulas)
            {
                result[i++] = ((!a.isTrue())||a.r.isTrue());
            }
            return result;
        }

        public static bool InitSet(bool[] l,LTLFormula root)
        {
            assignment = l;
            return root.isTrue();
        }
        //----------------------helper methods------------------------------------------


        //can be called only once as once called all added are true.
        private static void init(LTLFormula root)
        {
            subFormulas = new List<LTLFormula>();

            atomicFormulas = new List<Atomic>();

            nextFormulas = new List<NextFormula>();

            untilFormulas = new List<Until>();

            root.SetList();

        }

        //check if elementry
        private static bool CheckElementaryAssignment(bool[] a)
        {
            assignment = a;
            foreach (Until f in untilFormulas)
            {
                if (f.r.isTrue())
                {
                    if (f.isTrue() == false)
                    {
                        return false;
                    }
                }
                if (f.isTrue()&&(!f.r.isTrue()))
                {
                    if (f.l.isTrue() == false)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        
        private static List<bool[]> GetAllAssignments()
        {
            List<bool[]> result = new List<bool[]>();
            int p = IntPow(2, (uint)subFormulas.Count);
            for (int i = 0; i < p; i++)
            {
                bool[] temp = new bool[subFormulas.Count];
                uint j = (uint)i;
                int k = 0;
                while (j > 0)
                {
                    temp[k] = (j % 2 == 1);
                    k++;
                    j >>= 1;
                }
                result.Add(temp);
            }
            return result;
        }


        private static int IntPow(int x, uint pow)
        {
            int ret = 1;
            while (pow != 0)
            {
                if ((pow & 1) == 1)
                    ret *= x;
                x *= x;
                pow >>= 1;
            }
            return ret;
        }
    }

    public class Until : LTLFormula
    {
        public LTLFormula l;
        public LTLFormula r;



        public override void SetList()
        {
            if (added)
            {
                return;
            }
            added = true;
            index = subFormulas.Count;
            subFormulas.Add(this);
            untilFormulas.Add(this);

            l.SetList();
            r.SetList();

        }
        //locally consistent with until:
        //if r is true than this formula is true
        //if r not true and this formula is true than l is true

        //thus, if this formula is true:
        //r true l true
        //r true l false
        //r false l true

        //if this formula is false 
        //r false l true 
        //r false l false
        public Until(LTLFormula l, LTLFormula r)
        {

            this.l = l;
            this.r = r;
        }
    }

    public class Atomic : LTLFormula
    {
        public static int AtomicSize = 3;




        public override void SetList()
        {
            if (added)
            {
                return;
            }
            added = true;
            
            index = subFormulas.Count;
            subFormulas.Add(this);
            atomicFormulas.Add(this);

        }

        public int name;
        public Atomic(int name)
        {
            this.name = name;
        }
    }

    public class NegFormula : LTLFormula
    {
        public LTLFormula f;



        public override bool isTrue()
        {
            return !f.isTrue();
        }
        public override void SetList()
        {
            if (added)
            {
                return;
            }
            added = true;
            f.SetList();

        }


        public NegFormula(LTLFormula f) { this.f = f; }

    }

    public class AndFormula : LTLFormula
    {
        public LTLFormula a;
        public LTLFormula b;



        public override bool isTrue()
        {
            return a.isTrue() && b.isTrue();
        }

        public override void SetList()
        {
            if (added)
            {
                return;
            }
            added = true;

            a.SetList();
            b.SetList();

        }

        public AndFormula(LTLFormula a, LTLFormula b) { this.a = a; this.b = b; }
    }

    public class OrFormula : LTLFormula
    {
        public LTLFormula a;
        public LTLFormula b;




        public override bool isTrue()
        {
            return a.isTrue() || b.isTrue();
        }

        public override void SetList()
        {
            if (added)
            {
                return;
            }
            added = true;

            a.SetList();
            b.SetList();

        }

        public OrFormula(LTLFormula a, LTLFormula b) { this.a = a; this.b = b; }
    }

    public class NextFormula : LTLFormula
    {
        public LTLFormula a;




        public override void SetList()
        {
            if (added)
            {
                return;
            }
            added = true;
            index = subFormulas.Count;
            subFormulas.Add(this);
            nextFormulas.Add(this);
            a.SetList();


        }

        public NextFormula(LTLFormula a) { this.a = a; }
    }

    public class TrueFormula : LTLFormula
    {


        public override bool isTrue()
        {
            return true;
        }

        public override void SetList()
        {
            if (added)
            {
                return;
            }
            added = true;



        }
    }

    public class FalseFormula : LTLFormula
    {




        public override bool isTrue()
        {
            return false;
        }

        public override void SetList()
        {
            if (added)
            {
                return;
            }
            added = true;



        }
    }
}
