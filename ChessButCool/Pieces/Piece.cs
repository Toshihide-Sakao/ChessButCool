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

		public abstract void Move();

		public abstract void ShowMoves();
		protected abstract void ListAllMoves();
	}
}
