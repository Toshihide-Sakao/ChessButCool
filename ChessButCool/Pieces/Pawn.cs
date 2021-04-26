using System;
using System.Collections.Generic;

namespace ChessButCool.Pieces
{
    public class Pawn : Piece
    {
        public Pawn(Vector2Int position, SideColor side, ChessBoard board)
        {
            Position = position;
            this.side = side;
            this.board = board;
            PieceType = ((int)side).ToString() + "P";
        }

        protected override void ListAllMoves()
        {
            moves = new List<Vector2Int>();

            if (FirstMove()) // If first time moving, give them the choice to move 2 steps. 
            {
                moves.Add(new Vector2Int(0, 2));
            }

            CanTake(); // If piece can take left, give them the choice to do so.

            moves.Add(new Vector2Int(0, 1)); // When nothing

            if (side == SideColor.White) // If white, reverse y axis so pawns go towards black
            {
                foreach (var item in moves)
                {
                    item.Multiply(new Vector2Int(1, -1));
                }
            }

            FilterMoves();
        }

        private bool FirstMove()
        {
            if (Position.Y == 1 && side == SideColor.Black)
            {
                return true;
            }
            else if (Position.Y == 6 && side == SideColor.White)
            {
                return true;
            }

            return false;
        }

        private void CanTake()
        {
            int sideMultiplier = 1;
            for (int i = -1; i <= 1; i += 2)
            {
                if ((int)side == 0)
                {
                    sideMultiplier = -1;
                }
                if (!board.GetMap()[Position.X + i, Position.Y + sideMultiplier].NoVal3) // if enemy piece exists on the place pawns can take on left side
                {
                    // Adds the move to moves list
                    moves.Add(new Vector2Int(i, 1));
                }
            }
        }

        private bool CanTakeRight()
        {
            if (false)// if enemy piece exists on the place pawns can take on right side
            {
                // TODO: Write code

                return true; // Piece can take on right side
            }

            return false; // Can't take pieces to the right
        }

        public override Vector2Int GetPieceNumbers()
        {
            return new Vector2Int((int)side, 0);
        }
    }
}
