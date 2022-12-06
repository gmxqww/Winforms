﻿using PushingGame.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushingGame.Eventargs
{
    public class GameStartedEventArgs
    {
        public int BoardSize { get; set; }

        public  FieldState[,] Board { get; set; }

        public GameStartedEventArgs(int boardSize, FieldState[,] board)
        {
            BoardSize = boardSize;
            Board = board;
        }
    }
}
