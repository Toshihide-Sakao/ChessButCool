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

            // Sets piece type
            PieceType = ((int)side).ToString() + "P";
        }

        // Lists all moves
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

            FilterMoves(); // filters moves from invalid moves
        }

        // if it their first move
        private void FirstMove()
        {
            // it is the first move
            if ((Position.Y == 1 && side == SideColor.Black) || (Position.Y == 6 && side == SideColor.White))
            {
                // if piece can go 2 steps forward
                if (CanProceed(2))
                {
                    // adds moves
                    moves.Add(new Vector2Int(0, 2));
                }
            }
        }

        // if piece can take
        private void CanTake()
        {
            // which way to proceed
            int sideMultiplier = side == 0 ? -1 : 1;
            for (int i = -1; i <= 2; i += 2)
            {
                // if they can go to sides
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

        // adds move to go forward
        private void GoForward()
        {
            // if piece can go one step forward
            if (CanProceed(1))
                moves.Add(new Vector2Int(0, 1)); // adds move
        }

        // if piece can proceed the amount of moves
        private bool CanProceed(int moves)
        {
            // if color is white change direction
            if (side == SideColor.White)
            {
                moves *= -1;
            }

            // if moves go out of plane return false
            if (Position.Y + moves >= 8 || Position.Y + moves < 0)
            {
                return false;
            }

            // if path is blocked
            if (moves == 2)
            {
                return board.GetMap()[Position.X, Position.Y + 1].NoVal3;
            }
            if (moves == -2)
            {
                return board.GetMap()[Position.X, Position.Y - 1].NoVal3;
            }

            // return if piece exists on the piece 
            return board.GetMap()[Position.X, Position.Y + moves].NoVal3;
        }

        // Returns piece type in vector2int
        public override Vector2Int GetPieceNumbers()
        {
            return new Vector2Int((int)side, 0);
        }
    }
}
