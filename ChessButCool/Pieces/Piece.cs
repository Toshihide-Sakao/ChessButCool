using System.Numerics;
using System;
using System.Linq;
using System.Collections.Generic;
using ChessButCool;

namespace ChessButCool.Pieces
{
    public abstract class Piece
    {
        protected ChessBoard board;
        protected SideColor side;
        private Vector2Int position;
        protected List<Vector2Int> moves = new();
        private string pieceType = "";

        // Get piece numbers
        public abstract Vector2Int GetPieceNumbers();

        // Move the piece to the target, force can force the move to happen
        public bool Move(Vector2Int targetPos, bool force = false)
        {
            // converts to glboal pos
            Vector2Int convPos = new(targetPos.X - Position.X, targetPos.Y - Position.Y);

            // if targetpos exists in moves list or move is forced
            if (moves.Contains(convPos) || force)
            {
                UpdateToMap(convPos);
                return true;
            }
            else
            {
                return false;
            }
        }

        // Shows moves by highlighting positoins
        public void ShowMoves()
        {
            // Lists moves and removes all invalid moves
            ListAllMoves();
            board.RemoveInvalidMovesPiece(this);
            FilterMoves();

            // draws it out to map
            foreach (var item in moves)
            {
                board.GetMap()[position.X + item.X, position.Y + item.Y].Value2 = 1;
            }
        }

        // Updates to map
        protected void UpdateToMap(Vector2Int convPos)
        {
            // Adds position to map so it can be drawn
            board.GetMap()[Position.X, Position.Y].NoVal3 = true;
            Position.Add(convPos);
            board.GetMap()[Position.X, Position.Y].Value3 = this;
        }

        // Lists all moves
        protected abstract void ListAllMoves();
        
        // Gets moves but in global position
        public List<Vector2Int> GetPublicMoves()
        {
            // lists and updates all moves
            ListAllMoves();

            // makes new list for the public moves
            List<Vector2Int> publicMoves = new();

            // loops through moves
            for (int i = 0; i < moves.Count; i++)
            {
                // add moves globall pos
                publicMoves.Add(new Vector2Int(Position.X + moves[i].X, Position.Y + moves[i].Y));
            }

            // returns public moves
            return publicMoves;
        }

        // filtermoves that was outside of the board
        protected void FilterMoves()
        {
            // removes all moves outside the map
            moves.RemoveAll(item => item.X + Position.X >= 8 || item.X + Position.X < 0 || item.Y + Position.Y >= 8 || item.Y + Position.Y < 0);
            RemoveAllyBlockedMoves();
        }

        // removes all blocked moves by allied pieces
        private void RemoveAllyBlockedMoves()
        {
            // list for if they need something to be removed
            List<int> removeInts = new();

            // cehcks moves
            for (int i = 0; i < moves.Count; i++)
            {
                // if ally is on the position
                if (!board.GetMap()[Position.X + moves[i].X, Position.Y + moves[i].Y].NoVal3 && board.GetMap()[Position.X + moves[i].X, Position.Y + moves[i].Y].Value3.Side == this.side)
                {
                    // add the index
                    removeInts.Add(i);
                }
            }

            // removes moves that was blocked
            for (int i = removeInts.Count - 1; i >= 0; i--)
            {
                moves.RemoveAt(removeInts[i]);
            }
        }

        // Returns a piece from piecetype
        public static Piece GetPieceFromPieceType(string type, Vector2Int pos, ChessBoard board)
        {
            // returns the piece type that was mentioned
            return type[1] switch
            {
                'P' => new Pawn(new Vector2Int(pos.X, pos.Y), (SideColor)int.Parse(type[0].ToString()), board),
                'B' => new Bishop(new Vector2Int(pos.X, pos.Y), (SideColor)int.Parse(type[0].ToString()), board),
                'N' => new Knight(new Vector2Int(pos.X, pos.Y), (SideColor)int.Parse(type[0].ToString()), board),
                'R' => new Rook(new Vector2Int(pos.X, pos.Y), (SideColor)int.Parse(type[0].ToString()), board),
                'Q' => new Queen(new Vector2Int(pos.X, pos.Y), (SideColor)int.Parse(type[0].ToString()), board),
                'K' => new King(new Vector2Int(pos.X, pos.Y), (SideColor)int.Parse(type[0].ToString()), board),
                _ => throw new ArgumentException("that piece does not exist"),
            };
        }

        // Checks if collided (used for rook moves and bishop moves as well as queens)
        private bool CheckCollision(Vector2Int addingVector)
        {
            // sets collided to true
            bool collided = true;

            if (Position.X + addingVector.X < 8 && Position.X + addingVector.X >= 0 && Position.Y + addingVector.Y < 8 && Position.Y + addingVector.Y >= 0) // if the targeted position is out of bound
            {
                if (!board.GetMap()[Position.X + addingVector.X, Position.Y + addingVector.Y].NoVal3) // If there is a piece
                {
                    if (board.GetMap()[Position.X + addingVector.X, Position.Y + addingVector.Y].Value3.Side == this.Side) // if the piece is the same color as you
                    {
                        collided = true;
                    }
                    else // if the color is no the same as yours
                    {
                        moves.Add(addingVector);
                        collided = true;
                    }
                }
                else // no piece
                {
                    moves.Add(addingVector);
                    collided = false;
                }
            }

            // return bool
            return collided;
        }

        // Adds rook moves
        protected void AddRookMoves()
        {
            // bool array for if they havew collided the direction
            bool[] boolArray = new bool[4];

            // adding all possible x routes (as a rook)
            for (int i = 1; i < 8; i++)
            {
                // counter for which bool it chould watch
                int boolCounter = 0;

                // for loop for adding moves
                for (int j = -1; j <= 1; j += 2)
                {
                    // if not yet collided x val
                    if (!boolArray[boolCounter])
                    {
                        // the value to add to moves
                        int addingVal = i * j;
                        bool collided = CheckCollision(new Vector2Int(addingVal, 0));

                        // if it doesn't collide
                        if (!collided)
                        {
                            // add move
                            moves.Add(new Vector2Int(addingVal, 0));
                        }
                        // updates array for collisions
                        boolArray[boolCounter] = collided;
                    }
                    // if not yet collided y val
                    if (!boolArray[boolCounter + 2])
                    {
                        // the value to add to moves
                        int addingVal = i * j;
                        bool collided = CheckCollision(new Vector2Int(0, addingVal));

                        // if it doesn't collide
                        if (!collided)
                        {
                            // add move
                            moves.Add(new Vector2Int(0, addingVal));
                        }
                        // updates array for collisions
                        boolArray[boolCounter + 2] = collided;
                    }

                    // adding to bool array counter
                    boolCounter++;
                }
            }
        }

        // Adding bishop moves
        protected void AddBishopMoves()
        {
            // bool array for if they can move more
            bool[] boolArray = new bool[4];

            // adding all possible diagonal (as a bishop)
            for (int i = 1; i < 8; i++)
            {
                // counter for which bool it chould watch
                int boolCounter = 0;

                // for loop for adding moves
                for (int j = -1; j <= 1; j += 2)
                {
                    // if not collided to a piece downleft/upright wards yet
                    if (!boolArray[boolCounter])
                    {
                        // checks if they have collided
                        int addingPos = i * j;
                        bool collided = CheckCollision(new Vector2Int(addingPos));

                        // if not collided the new pos
                        if (!collided)
                        {
                            // adds move
                            moves.Add(new Vector2Int(addingPos));
                        }
                        // updates bool array
                        boolArray[boolCounter] = collided;
                    }
                    // if not collided to a piece upperleft/downright wards yet
                    if (!boolArray[boolCounter + 2])
                    {
                        // checks if they have collided
                        int ok = i * j;
                        bool collided = CheckCollision(new Vector2Int(ok, -ok));

                        // if not collided
                        if (!collided)
                        {
                            // adds move
                            moves.Add(new Vector2Int(ok, -ok));
                        }
                        // updates bool array
                        boolArray[boolCounter + 2] = collided;
                    }
                    // adds bool counter
                    boolCounter++;
                }
            }
        }

        // Property
        public SideColor Side
        {
            get { return side; }
            set { side = value; }
        }

        public Vector2Int Position
        {
            get { return position; }
            protected set { position = value; }
        }

        public string PieceType
        {
            get { return pieceType; }
            set { pieceType = value; }
        }

        public List<Vector2Int> Moves
        {
            get { return moves; }
            set { moves = value; }
        }
    }
}
