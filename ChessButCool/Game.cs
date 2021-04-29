using System;
using Raylib_cs;

namespace ChessButCool
{
    public class Game
    {
        private readonly int width;
        private readonly int height;

        public Game(int width, int height)
        {
            this.width = width;
            this.height = height;

            Draw();
        }
        public void Draw()
        {
            Raylib.InitWindow(width, height, "Chess");

            // generate chessboard and ui
            ChessBoard board = new(700, new Vector2Int(50, 50));
            UI ui = new(230, 700, new Vector2Int(760, 50), board);

            // update function
            while (!Raylib.WindowShouldClose())
            {
                // begins drawing
                Raylib.BeginDrawing();
                // draw bg
                Raylib.ClearBackground(new Color(163, 163, 163, 255));

                // Updates and draws board
                board.Update();
                board.Draw();

                ui.Draw();

                // ends drawing
                Raylib.EndDrawing();
            }
        }
    }
}
