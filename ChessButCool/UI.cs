using System;
using Raylib_cs;

namespace ChessButCool
{
    public class UI
    {
        int width;
        int height;
        Vector2Int pos;
        ChessBoard board;

        public UI(int width, int height, Vector2Int pos, ChessBoard board)
        {
            this.width = width;
            this.height = height;
            this.pos = pos;
            this.board = board;
        }

        public void Draw()
        {
            if (board.Turn % 2 == 0)
            {
                Raylib.DrawText("White turn", pos.X, 600, 40, Color.WHITE);
            }
            else
            {
                Raylib.DrawText("Black turn", pos.X, 600, 40, Color.WHITE);
            }
            
        }
    }
}
