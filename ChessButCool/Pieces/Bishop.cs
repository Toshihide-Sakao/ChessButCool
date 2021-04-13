using System;
using System.Collections.Generic;

namespace ChessButCool.Pieces
{
	public class Bishop : Piece
	{
		public Bishop(Vector2Int position, SideColor side)
        {
            this.position = position;
            this.side = side;
			PieceType = ((int)side).ToString() + "B";
        }

		public override void Move()
		{

		}

		public override void ShowMoves()
		{

		}

		protected override void ListAllMoves()
        {
			base.AddBishopMoves();
		}

	}
}
