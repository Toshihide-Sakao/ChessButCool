using System;
using System.Collections.Generic;

namespace ChessButCool.Pieces
{
	public class Queen : Piece
	{
		public Queen(Vector2Int position, SideColor side, ChessBoard board)
        {
            Position = position;
            this.side = side;
			this.board = board;

			// Sets piece type
			PieceType = ((int)side).ToString() + "Q";
        }
		
		// Lists all moves
		protected override void ListAllMoves()
        {
			moves = new List<Vector2Int>();
			base.AddRookMoves();
			base.AddBishopMoves();

			FilterMoves();
		}

		// Returns piece type in vector2int
		public override Vector2Int GetPieceNumbers()
        {
            return new Vector2Int((int)side, 4);
        }
	}
}
