using System;
using System.Collections.Generic;

namespace ChessButCool.Pieces
{
	public class King : Piece
	{
		public King(Vector2Int position, SideColor side)
        {
            this.Position = position;
            this.side = side;
			PieceType = ((int)side).ToString() + "K";
        }

		public override void Move()
		{

		}

		protected override void ListAllMoves()
        {
			
		}

		public override Vector2Int GetPieceNumbers()
        {
            return new Vector2Int((int)side, 5); 
        }
	}
}
