using System;
using System.Collections.Generic;
using System.Numerics;

namespace ChessButCool
{
    // A class for a vector2 with ints instead of floats
    public class Vector2Int
    {
        private int x;
        private int y;

        // 4 different Constructors
        public Vector2Int(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Vector2Int(int xy)
        {
            this.x = xy;
            this.y = xy;
        }

        public Vector2Int(Vector2 vector2)
        {
            this.x = (int)vector2.X;
            this.Y = (int)vector2.Y;
        }

        public Vector2Int()
        {
            x = 0;
            y = 0;
        }

        // Add to the vector2
        public void Add(int x, int y)
        {
            this.x += x;
            this.y += y;
        }

        public void Add(Vector2Int vector2)
        {
            this.x += vector2.X;
            this.y += vector2.Y;
        }

        // Multiply to the vector2
        public void Multiply(Vector2Int vector2)
        {
            this.x *= vector2.X;
            this.y *= vector2.Y;
        }

        public override bool Equals(object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Vector2Int p = (Vector2Int)obj;
                return (X == p.X) && (Y == p.Y);
            }
        }

        public int X
        {
            get { return x; }
            set { x = value; }
        }

        public int Y
        {
            get { return y; }
            set { y = value; }
        }
    }
}
