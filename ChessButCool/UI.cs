using System;
using Raylib_cs;

namespace ChessButCool
{
    public class UI
    {
        int width;
        int height;
        Vector2Int pos;

        public UI(int width, int height, Vector2Int pos)
        {
            this.width = width;
            this.height = height;
            this.pos = pos;
        }

        public void Draw()
        {
            Raylib.DrawText("TEST", pos.X, 600, 50, Color.WHITE);
        }
    }
}
