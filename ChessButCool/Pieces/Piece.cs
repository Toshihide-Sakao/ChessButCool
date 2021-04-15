using System;
using System.Collections.Generic;
using ChessButCool;

namespace ChessButCool.Pieces
{
	public abstract class Piece
	{
		protected SideColor side;
		private Vector2Int position;
		protected List<Vector2Int> moves;
		private string pieceType = "";

		public SideColor Side
		{
			get
			{
				return side;
			}
			set
			{
				side = value;
			}
		}

		public Vector2Int Position
		{
			get
			{
				return position;
			}
			protected set
			{
				position = value;
			}
		}

		public string PieceType
		{
			get { return pieceType; }
			set { pieceType = value; }
		}

		public abstract void Move();
		public void ShowMoves(Triple<int, int, Piece>[,] map)
		{
			ListAllMoves();

			foreach (var item in moves)
			{
				map[position.X + item.X, position.Y + item.Y].Value2 = 1;
			}
		}

		protected abstract void ListAllMoves();

		// FIXME: move into chessboard this can be the problem 
		public static Piece GetPieceFromPieceType(string type, Vector2Int pos)
		{
			switch (type[1])
			{
				case 'P':
					return new Pawn(pos, (SideColor)int.Parse(type[0].ToString()));
				case 'B':
					return new Bishop(pos, (SideColor)int.Parse(type[0].ToString()));
				case 'N':
					return new Knight(pos, (SideColor)int.Parse(type[0].ToString()));
				case 'R':
					return new Rook(pos, (SideColor)int.Parse(type[0].ToString()));
				case 'Q':
					return new Queen(pos, (SideColor)int.Parse(type[0].ToString()));
				case 'K':
					return new King(pos, (SideColor)int.Parse(type[0].ToString()));
				default:
					return new Dummy();
			}
		}

		protected void AddRookMoves()
		{
			// adding all possible x routes (as a rook)
			for (int x = -position.X; x < (-position.X) + 8; x++)
			{
				if (x != position.X || !moves.Contains(new Vector2Int(x, 0)))
				{
					moves.Add(new Vector2Int(x, 0));
				}
			}
			// adding all possible y routes (as a rook)
			for (int y = -position.Y; y < (-position.Y) + 8; y++)
			{
				if (y != position.Y || !moves.Contains(new Vector2Int(0, y)))
				{
					moves.Add(new Vector2Int(0, y));
				}
			}
		}

		protected void AddBishopMoves()
		{
			// adding all possible diagonal (as a bishop)
			for (int q = -8; q < 8; q++)
			{
				if (new Vector2Int(q) != position || !moves.Contains(new Vector2Int(q)))
				{
					moves.Add(new Vector2Int(q));
				}
			}
		}
	}
}
