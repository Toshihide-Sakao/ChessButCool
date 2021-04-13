using System;
using System.Collections.Generic;
using System.Numerics;

namespace ChessButCool
{
	public class Vector2Int
	{
		int x;
		int y;

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

		public void Multiply(Vector2Int vector2)
		{
			this.x *= vector2.X;
			this.y *= vector2.Y;
		}

		public Vector2Int()
		{
			x = 0;
			y = 0;
		}

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

		public int X
		{
			get
			{
				return x;
			}
			set
			{
				x = value;
			}
		}

		public int Y
		{
			get
			{
				return y;
			}
			set
			{
				y = value;
			}
		}
	}
}
