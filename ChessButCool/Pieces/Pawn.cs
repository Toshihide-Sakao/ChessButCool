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

            FirstMove(); // If first time moving, give them the choice to move 2 steps. 
            CanTake(); // If piece can take left, give them the choice to do so.
            GoForward();// When nothing

            if (side == SideColor.White) // If white, reverse y axis so pawns go towards black
            {
                foreach (var item in moves)
                {
                    item.Multiply(new Vector2Int(1, -1));
                }
            }

            FilterMoves();
        }

        private void FirstMove()
        {
            if ((Position.Y == 1 && side == SideColor.Black) || (Position.Y == 6 && side == SideColor.White))
            {
                if (CanProceed(2))
                {
                    moves.Add(new Vector2Int(0, 2));
                }
            }
        }

        private void CanTake()
        {
            int sideMultiplier = side == 0 ? -1 : 1;
            for (int i = -1; i <= 2; i += 2)
            {
                if (Position.X + i < 8 && Position.X + i >= 0 && Position.Y + sideMultiplier < 8 && Position.Y + sideMultiplier >= 0)
                {
                    if (!board.GetMap()[Position.X + i, Position.Y + sideMultiplier].NoVal3) // if enemy piece exists on the place pawns can take on left side
                    {
                        // Adds the move to moves list
                        moves.Add(new Vector2Int(i, 1));
                    }
                }
            }
        }

        private void GoForward()
        {
            if (CanProceed(1))
                moves.Add(new Vector2Int(0, 1));
        }

        private bool CanProceed(int moves)
        {
            if (side == SideColor.White)
            {
                moves *= -1;
            }

            if (Position.Y + moves >= 8 || Position.Y + moves < 0)
            {
                return false;
            }
            return board.GetMap()[Position.X, Position.Y + moves].NoVal3;
        }

        public override Vector2Int GetPieceNumbers()
        {
            return new Vector2Int((int)side, 0);
        }
    }
}
