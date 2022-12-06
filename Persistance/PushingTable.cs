using PushingGame.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushingGame.Persistance
{
    public class PushingTable
    {
        public int size;

        public FieldState[,] board;

        public int w;
        
        public int b;
        

        public PushingTable(int size, FieldState[,] board, int w, int b)
        {
            this.size = size;
            this.board = board;
            this.w = w;
            this.b = b;
        }

        

    }
}
