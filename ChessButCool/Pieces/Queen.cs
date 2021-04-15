using System;
using System.Collections.Generic;

namespace ChessButCool.Pieces
{
	public class Queen : Piece
	{
		public Queen(Vector2Int position, SideColor side)
        {
            Position = position;
            this.side = side;
			PieceType = ((int)side).ToString() + "Q";
        }
		public override void Move()
		{

		}
		
		protected override void ListAllMoves()
        {
			base.AddRookMoves();
			base.AddBishopMoves();
		}

		public override Vector2Int GetPieceNumbers()
        {
            return new Vector2Int((int)side, 4); 
        }
	}
}
