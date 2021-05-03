using System;
using System.Collections.Generic;

namespace ChessButCool.Pieces
{
	public class King : Piece
	{
		public King(Vector2Int position, SideColor side, ChessBoard board)
        {
            this.Position = position;
            this.side = side;
			this.board = board;

			// Sets piece type
			PieceType = ((int)side).ToString() + "K";
        }

		// FIXME: Castelling
		protected override void ListAllMoves()
        {
			moves = new List<Vector2Int>();
			for (int y = -1; y < 2; y++)
			{
				for (int x = -1; x < 2; x++)
				{
					if (x != 0 || y != 0)
					{
						moves.Add(new Vector2Int(x, y));
					}
				}
			}

			FilterMoves();
		}

		// Returns piece type in vector2int
		public override Vector2Int GetPieceNumbers()
        {
            return new Vector2Int((int)side, 5);
        }
	}
}
