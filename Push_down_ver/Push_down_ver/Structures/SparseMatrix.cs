using System;
using System.Collections.Generic;
using System.Text;

namespace Push_down_ver.Structures
{
    public class SparseMatrix<T>
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public long Size { get; private set; }

        private Dictionary<long, T> _cells = new Dictionary<long, T>();


        private Dictionary<long, T>[] rows;
        private Dictionary<long, T>[] columns;
        public SparseMatrix(int w, int h)
        {
            this.Width = w;
            this.Height = h;
            this.Size = w * h;
            this.rows = new Dictionary<long, T>[h];
            for(int i=0; i < h; i++)
            {
                this.rows[i]= new Dictionary<long, T>();
            }
            this.columns = new Dictionary<long, T>[w];
            for (int i = 0; i < w; i++)
            {
                this.columns[i] = new Dictionary<long, T>();
            }
        }

        public bool IsCellEmpty(int row, int col)
        {
            long index = row * Width + col;
            return _cells.ContainsKey(index);
        }

        public T this[int row, int col]
        {
            get
            {
                long index = row * Width + col;
                T result;
                _cells.TryGetValue(index, out result);
                return result;
            }
            set
            {
                long index = row * Width + col;
                _cells[index] = value;
                rows[row][col]= value;
                columns[col][row] = value;
                
            }
        }
        public Dictionary<long, T> getRow(int r)
        {
            return rows[r];
        }
        public Dictionary<long, T> getCol(int c)
        {
            return columns[c];
        }
    }

}
