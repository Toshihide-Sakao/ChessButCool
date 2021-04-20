using System;
using System.Linq;
using System.Collections.Generic;
using ChessButCool;

namespace ChessButCool.Pieces
{
	public abstract class Piece
	{
		protected SideColor side;
		private Vector2Int position;
		protected List<Vector2Int> moves = new();
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
		public abstract Vector2Int GetPieceNumbers();

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
		protected void FilterMoves()
		{
			moves.RemoveAll(item => item.X >= 8 - Position.X || item.X < 0 - Position.X || item.Y >= 8 - Position.Y || item.Y < 0  - Position.Y );
			// var nodupeMoves = moves.Distinct();
		}

		// FIXME: move into chessboard this can be the problem (can be ignored)
		public static Piece GetPieceFromPieceType(string type, Vector2Int pos)
		{
			switch (type[1])
			{
				case 'P':
					return new Pawn(new Vector2Int(pos.X, pos.Y), (SideColor)int.Parse(type[0].ToString()));
				case 'B':
					return new Bishop(new Vector2Int(pos.X, pos.Y), (SideColor)int.Parse(type[0].ToString()));
				case 'N':
					return new Knight(new Vector2Int(pos.X, pos.Y), (SideColor)int.Parse(type[0].ToString()));
				case 'R':
					return new Rook(new Vector2Int(pos.X, pos.Y), (SideColor)int.Parse(type[0].ToString()));
				case 'Q':
					return new Queen(new Vector2Int(pos.X, pos.Y), (SideColor)int.Parse(type[0].ToString()));
				case 'K':
					return new King(new Vector2Int(pos.X, pos.Y), (SideColor)int.Parse(type[0].ToString()));
				default:
					throw new ArgumentException("that piece does not exist");
			}
		}
		
		// FIXME: fix these
		protected void AddRookMoves()
		{
			// adding all possible x routes (as a rook)
			for (int x = -7; x < 8; x++)
			{
				if (x != 0)
					moves.Add(new Vector2Int(x, 0));
			}
			// adding all possible y routes (as a rook)
			for (int y = -7; y < 8; y++)
			{
				if (y != 0)
					moves.Add(new Vector2Int(0, y));
			}
		}

		// FIXME: fix these
		protected void AddBishopMoves()
		{
			// adding all possible diagonal (as a bishop)
			for (int q = -7; q < 8; q++)
			{
				if (new Vector2Int(q) != position || !moves.Contains(new Vector2Int(q)))
				{
					moves.Add(new Vector2Int(q));
				}
			}
		}
	}
}
