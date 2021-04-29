using System;

namespace ChessButCool.Pieces
{
    public class Dummy : Piece
    {
        public Dummy()
        {
        }

		protected override void ListAllMoves()
        {
		}

        public override Vector2Int GetPieceNumbers()
        {
            return new Vector2Int(-99, -99);
        }
    }
}
