using System;
using System.Collections.Generic;

namespace ChessButCool.Pieces
{
	public class Bishop : Piece
	{
		// Bishop constructor
		public Bishop(Vector2Int position, SideColor side, ChessBoard board)
        {
			// Sets values
            this.Position = position;
            this.side = side;
			this.board = board;

			// Sets piece type
			PieceType = ((int)side).ToString() + "B";
        }

		// Lists all moves
		protected override void ListAllMoves()
        {
			moves = new List<Vector2Int>();
			base.AddBishopMoves();
			FilterMoves();
		}

		// Returns piece type in vector2int
		public override Vector2Int GetPieceNumbers()
        {
            return new Vector2Int((int)side, 2);
        }
	}
}
