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

        public override void Move(Vector2Int targetPos)
        {

        }

        protected override void ListAllMoves()
        {
            moves.Add(new Vector2Int(2, 1));
            moves.Add(new Vector2Int(1, 2));
			moves.Add(new Vector2Int(-2, 1));
            moves.Add(new Vector2Int(1, -2));
			moves.Add(new Vector2Int(-2, -1));
            moves.Add(new Vector2Int(-1, -2));
			moves.Add(new Vector2Int(2, -1));
            moves.Add(new Vector2Int(-1, 2));
            FilterMoves();
        }

        public override Vector2Int GetPieceNumbers()
        {
            return new Vector2Int((int)side, 1);
        }
    }
}
