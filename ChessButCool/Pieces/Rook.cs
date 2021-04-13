using System;
using System.Collections.Generic;

namespace ChessButCool.Pieces
{
	public class Rook : Piece
	{
		public Rook(Vector2Int position, SideColor side)
        {
            this.position = position;
            this.side = side;
			PieceType = side + "R";
        }

		public override void Move()
		{

		}

		public override void ShowMoves()
		{

		}

		protected override void ListAllMoves()
        {
			base.AddRookMoves();
		}
	}
}
