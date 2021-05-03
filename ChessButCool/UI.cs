using System;
using Raylib_cs;

namespace ChessButCool
{
    public class UI
    {
        private readonly int width;
        private readonly int height;
        private readonly Vector2Int pos;
        private readonly ChessBoard board;

        public UI(int width, int height, Vector2Int pos, ChessBoard board)
        {
            // Sets values
            this.width = width;
            this.height = height;
            this.pos = pos;
            this.board = board;
        }

        public void Draw()
        {
            // Draws the ouline of UI
            Raylib.DrawRectangle(pos.X, pos.Y, width, height, Color.BROWN);
            // if white turn draw text with white font
            if (board.Turn % 2 == 0)
            {
                Raylib.DrawText("White turn", pos.X + 5, 50, 40, Color.WHITE);
            }
            // if black turn draw text with black font
            else
            {
                Raylib.DrawText("Black turn", pos.X + 5, 50, 40, Color.BLACK);
            }

            // draws the turn the game is on
            Raylib.DrawText(((board.Turn + 2) / 2).ToString(), pos.X + 5, 100, 30, Color.WHITE);
        }
    }
}
