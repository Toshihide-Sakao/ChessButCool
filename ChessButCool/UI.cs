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
            Raylib.DrawRectangle(pos.X, pos.Y, width, height, Color.BROWN);
            if (board.Turn % 2 == 0)
            {
                Raylib.DrawText("White turn", pos.X + 5, 50, 40, Color.WHITE);
            }
            else
            {
                Raylib.DrawText("Black turn", pos.X + 5, 50, 40, Color.BLACK);
            }

            Raylib.DrawText(((board.Turn + 2) / 2).ToString(), pos.X + 5, 100, 30, Color.WHITE);
            
        }
    }
}
