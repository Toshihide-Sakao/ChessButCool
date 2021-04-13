using System;
using System.Collections.Generic;

namespace ChessButCool.Pieces
{
	public class Queen : Piece
	{
		public Queen(Vector2Int position, SideColor side)
        {
            this.position = position;
            this.side = side;
			PieceType = side + "Q";
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
			base.AddBishopMoves();
		}

	}
}
