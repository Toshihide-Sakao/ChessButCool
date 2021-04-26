using System;
using System.Collections.Generic;

namespace ChessButCool.Pieces
{
	public class Rook : Piece
	{
		public Rook(Vector2Int position, SideColor side, ChessBoard board)
        {
            this.Position = position;
            this.side = side;
			this.board = board;
			PieceType = ((int)side).ToString() + "R";
        }

		protected override void ListAllMoves()
        {
			moves = new List<Vector2Int>();
			
			base.AddRookMoves();
			FilterMoves();
		}

		public override Vector2Int GetPieceNumbers()
        {
            return new Vector2Int((int)side, 3); 
        }
	}
}
