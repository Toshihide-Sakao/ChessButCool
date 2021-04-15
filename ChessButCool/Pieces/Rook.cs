using System;
using System.Collections.Generic;

namespace ChessButCool.Pieces
{
	public class Rook : Piece
	{
		public Rook(Vector2Int position, SideColor side)
        {
            this.Position = position;
            this.side = side;
			PieceType = ((int)side).ToString() + "R";
        }

		public override void Move()
		{

		}

		protected override void ListAllMoves()
        {
			base.AddRookMoves();
		}
	}
}
