using System;
using System.Collections.Generic;

namespace ChessButCool.Pieces
{
	public class Knight : Piece
	{
		public Knight(Vector2Int position, SideColor side)
        {
            this.Position = position;
            this.side = side;
			PieceType = ((int)side).ToString() + "N";
        }

		public override void Move()
		{

		}

		protected override void ListAllMoves()
        {
			
		}
		
		public override Vector2Int GetPieceNumbers()
        {
            return new Vector2Int((int)side, 1); 
        }
	}
}
