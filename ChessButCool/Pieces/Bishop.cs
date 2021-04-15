using System;
using System.Collections.Generic;

namespace ChessButCool.Pieces
{
	public class Bishop : Piece
	{
		public Bishop(Vector2Int position, SideColor side)
        {
            this.Position = position;
            this.side = side;
			PieceType = ((int)side).ToString() + "B";
        }

		public override void Move()
		{

		}

		protected override void ListAllMoves()
        {
			base.AddBishopMoves();
		}

		public override Vector2Int GetPieceNumbers()
        {
            return new Vector2Int((int)side, 2); 
        }

	}
}
