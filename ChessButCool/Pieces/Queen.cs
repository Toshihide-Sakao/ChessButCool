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
		public override void Move(Vector2Int targetPos)
		{

		}
		
		protected override void ListAllMoves()
        {
			base.AddRookMoves();
			base.AddBishopMoves();

			FilterMoves();
		}

		public override Vector2Int GetPieceNumbers()
        {
            return new Vector2Int((int)side, 4); 
        }
	}
}
