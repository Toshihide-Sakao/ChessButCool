using System;
using System.Collections.Generic;

namespace ChessButCool.Pieces
{
	public class King : Piece
	{
		public King(Vector2Int position, SideColor side)
        {
            this.position = position;
            this.side = side;
			PieceType = ((int)side).ToString() + "K";
        }

		public override void Move()
		{

		}

		public override void ShowMoves()
		{

		}

		protected override void ListAllMoves()
        {
			
		}
	}
}
