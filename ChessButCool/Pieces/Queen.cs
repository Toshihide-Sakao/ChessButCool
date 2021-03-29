using System;
using System.Collections.Generic;

namespace ChessButCool.Pieces
{
	public class Queen : Piece
	{
		public Queen(Vector2Int position, SideColor side)
        {
            this.position = position;
            this.side = side;
        }
		public override void Move()
		{

		}

		public override void ShowMoves()
		{

		}
		
		protected override void ListAllMoves()
        {
			for (int x = 0; x < 8; x++)
			{
				moves.Add(new Vector2Int());
			}
			
		}

	}
}
