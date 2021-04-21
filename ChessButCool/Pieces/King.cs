using System;
using System.Collections.Generic;

namespace ChessButCool.Pieces
{
	public class King : Piece
	{
		public King(Vector2Int position, SideColor side)
        {
            this.Position = position;
            this.side = side;
			PieceType = ((int)side).ToString() + "K";
        }

		public override void Move(Vector2Int targetPos)
		{

		}

		protected override void ListAllMoves()
        {
			for (int y = -1; y < 2; y++)
			{
				for (int x = -1; x < 2; x++)
				{
					if (x != 0 || y != 0)
					{
						moves.Add(new Vector2Int(x, y));
					}
				}
			}

			FilterMoves();
		}

		public override Vector2Int GetPieceNumbers()
        {
            return new Vector2Int((int)side, 5); 
        }
	}
}
