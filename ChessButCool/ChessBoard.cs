using System;
using System.Collections.Generic;
using ChessButCool.Pieces;

namespace ChessButCool
{
	public class ChessBoard
	{
		int height = 8;
		int width = 8;
		int[,] map = new int[8, 8];
		List<Piece> pieces = new List<Piece>(); 
		public void Draw()
		{
			
		}

		public ChessBoard()
		{
			
		}
	}
}
