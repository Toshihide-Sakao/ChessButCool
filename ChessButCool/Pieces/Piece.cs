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

        public SideColor Side
        {
            get
            {
                return side;
            }
            set
            {
                side = value;
            }
        }

        public Vector2Int Position
        {
            get
            {
                return position;
            }
            protected set
            {
                position = value;
            }
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
        public abstract Vector2Int GetPieceNumbers();

        public bool Move(Vector2Int targetPos, bool force = false)
        {
            Vector2Int convPos = new(targetPos.X - Position.X, targetPos.Y - Position.Y);
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
        public void ShowMoves()
        {
            ListAllMoves();
            board.RemoveInvalidMovesPiece(this);
            
            FilterMoves();
            foreach (var item in moves)
            {
                board.GetMap()[position.X + item.X, position.Y + item.Y].Value2 = 1;
            }
        }

        protected void UpdateToMap(Vector2Int convPos)
        {
            board.GetMap()[Position.X, Position.Y].NoVal3 = true;
            Position.Add(convPos);
            board.GetMap()[Position.X, Position.Y].Value3 = this;
        }

        protected abstract void ListAllMoves();

        public List<Vector2Int> GetPublicMoves()
        {
            ListAllMoves();

            List<Vector2Int> publicMoves = new();
            for (int i = 0; i < moves.Count; i++)
            {
                publicMoves.Add(new Vector2Int(Position.X + moves[i].X, Position.Y + moves[i].Y));
            }

            // debugging purpose
            // foreach (var item in publicMoves)
            // {
            //     board.GetMap()[item.X, item.Y].Value2 = 99;
            // }
            // ----------------------------------

            return publicMoves;
        }
        protected void FilterMoves()
        {
            moves.RemoveAll(item => item.X + Position.X >= 8 || item.X + Position.X < 0 || item.Y + Position.Y >= 8 || item.Y + Position.Y < 0);
            RemoveAllyBlockedMoves();

            // var nodupeMoves = moves.Distinct();
        }

        // removes all blocked moves by allied pieces
        private void RemoveAllyBlockedMoves()
        {
            List<int> removeInts = new();
            for (int i = 0; i < moves.Count; i++)
            {
                if (!board.GetMap()[Position.X + moves[i].X, Position.Y + moves[i].Y].NoVal3 && board.GetMap()[Position.X + moves[i].X, Position.Y + moves[i].Y].Value3.Side == this.side)
                {
                    removeInts.Add(i);
                }
            }
            for (int i = removeInts.Count - 1; i >= 0; i--)
            {
                moves.RemoveAt(removeInts[i]);
            }
        }

        // Returns a piece from piecetype
        public static Piece GetPieceFromPieceType(string type, Vector2Int pos, ChessBoard board)
        {
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

        private bool CheckCollision(Vector2Int addingVector)
        {
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

            return collided;
        }

        protected void AddRookMoves()
        {
            bool[] boolArray = new bool[4];
            // adding all possible x routes (as a rook)
            for (int i = 1; i < 8; i++)
            {
                int boolCounter = 0;
                for (int j = -1; j <= 1; j += 2)
                {
                    if (!boolArray[boolCounter])
                    {
                        int ok = i * j;
                        bool temp = CheckCollision(new Vector2Int(ok, 0));
                        if (!temp)
                        {
                            moves.Add(new Vector2Int(ok, 0));
                        }
                        boolArray[boolCounter] = temp;
                    }
                    if (!boolArray[boolCounter + 2])
                    {
                        int ok = i * j;
                        bool temp = CheckCollision(new Vector2Int(0, ok));
                        if (!temp)
                        {
                            moves.Add(new Vector2Int(0, ok));
                        }
                        boolArray[boolCounter + 2] = temp;
                    }
                    boolCounter++;
                }
            }
        }

        protected void AddBishopMoves()
        {
            bool[] boolArray = new bool[4];

            // adding all possible diagonal (as a bishop)
            for (int i = 1; i < 8; i++)
            {
                int boolCounter = 0;
                for (int j = -1; j <= 1; j += 2)
                {
                    if (!boolArray[boolCounter])
                    {
                        int ok = i * j;
                        bool temp = CheckCollision(new Vector2Int(ok));
                        if (!temp)
                        {
                            moves.Add(new Vector2Int(ok));
                        }
                        boolArray[boolCounter] = temp;
                    }
                    if (!boolArray[boolCounter + 2])
                    {
                        int ok = i * j;
                        bool temp = CheckCollision(new Vector2Int(ok, -ok));
                        if (!temp)
                        {
                            moves.Add(new Vector2Int(ok, -ok));
                        }
                        boolArray[boolCounter + 2] = temp;
                    }
                    boolCounter++;
                }
            }
        }
    }
}
