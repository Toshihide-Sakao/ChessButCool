using System;
using System.Collections.Generic;

namespace ChessButCool.Pieces
{
	public class Knight : Piece
	{
		public Knight(Vector2Int position, SideColor side)
        {
            this.position = position;
            this.side = side;
			PieceType = ((int)side).ToString() + "N";
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
