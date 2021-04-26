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
        public abstract Vector2Int GetPieceNumbers();

        public bool Move(Vector2Int targetPos)
        {
            Vector2Int convPos = new Vector2Int(targetPos.X - Position.X, targetPos.Y - Position.Y);
            if (moves.Contains(convPos))
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
        protected void FilterMoves()
        {
            moves.RemoveAll(item => item.X >= 8 - Position.X || item.X < 0 - Position.X || item.Y >= 8 - Position.Y || item.Y < 0 - Position.Y);

            List<int> removeInts = new List<int>();
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
            // remove all blocked moves by allied pieces

            // var nodupeMoves = moves.Distinct();
        }

        // FIXME: move into chessboard this can be the problem (can be ignored)
        public static Piece GetPieceFromPieceType(string type, Vector2Int pos, ChessBoard board)
        {
            switch (type[1])
            {
                case 'P':
                    return new Pawn(new Vector2Int(pos.X, pos.Y), (SideColor)int.Parse(type[0].ToString()), board);
                case 'B':
                    return new Bishop(new Vector2Int(pos.X, pos.Y), (SideColor)int.Parse(type[0].ToString()), board);
                case 'N':
                    return new Knight(new Vector2Int(pos.X, pos.Y), (SideColor)int.Parse(type[0].ToString()), board);
                case 'R':
                    return new Rook(new Vector2Int(pos.X, pos.Y), (SideColor)int.Parse(type[0].ToString()), board);
                case 'Q':
                    return new Queen(new Vector2Int(pos.X, pos.Y), (SideColor)int.Parse(type[0].ToString()), board);
                case 'K':
                    return new King(new Vector2Int(pos.X, pos.Y), (SideColor)int.Parse(type[0].ToString()), board);
                default:
                    throw new ArgumentException("that piece does not exist");
            }
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
