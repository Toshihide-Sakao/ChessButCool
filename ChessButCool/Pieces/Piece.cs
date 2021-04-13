using System;
using System.Collections.Generic;
using ChessButCool;

namespace ChessButCool.Pieces
{
	public abstract class Piece
	{
		protected SideColor side;
		protected Vector2Int position;
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
			set
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

		public abstract void ShowMoves();
		protected abstract void ListAllMoves();

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
					return ;
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
