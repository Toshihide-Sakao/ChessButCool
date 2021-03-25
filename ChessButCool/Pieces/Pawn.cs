using System;
using System.Collections.Generic;

namespace ChessButCool.Pieces
{
    public class Pawn : Piece
    {

        public Pawn(Vector2Int position)
        {
            this.position = position;

        }

        public override void Move()
        {

        }

        public override void ShowMoves()
        {

        }

        protected override void ListAllMoves()
        {
            if (FirstMove()) // If first time moving, give them the choice to move 2 steps. 
            {
                moves.Add(new Vector2Int(0, 2));
            }
            if (CanTakeLeft()) // If piece can take left, give them the choice to do so.
            {
                moves.Add(new Vector2Int(-1, 1));
            }
            if (CanTakeRight()) // If piece can take right, give them the choice to do so.
            {
                moves.Add(new Vector2Int(1, 1));
            }
            moves.Add(new Vector2Int(1, 0));

            if (side == SideColor.White) // If white, reverse y axis so pawns go towards black
            {
				foreach (var item in moves)
				{
					item.Multiply(new Vector2Int(1, -1));
				}
            }
        }

        private bool FirstMove()
        {
            if (position.Y == 1 && side == SideColor.Black)
            {
                return true;
            }
            else if (position.Y == 6 && side == SideColor.White)
            {
                return true;
            }

            return false;
        }

        private bool CanTakeLeft()
        {
            if (false) // if enemy piece exists on the place pawns can take on left side
            {
                // TODO: Write code

                return true; // Piece can take on left side
            }
            return false; // Can't take pieces to the left
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
    }
}
