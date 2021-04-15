using System;
using Raylib_cs;

namespace ChessButCool
{
    public class Game
    {
        int width;
        int height;

        public Game(int width, int height)
        {
            this.width = width;
            this.height = height;

            Draw();
        }
        public void Draw()
        {
            Raylib.InitWindow(width, height, "Chess");

            // generate chessboard
            ChessBoard board = new ChessBoard(700, new Vector2Int(150, 50));

            board.DeBuggerBoard();

            // update function
            while (!Raylib.WindowShouldClose())
            {
                // begins drawing
                Raylib.BeginDrawing();
                // draw bg
                Raylib.ClearBackground(new Color(163, 163, 163, 255));

                board.Update();
                board.Draw();

                // ends drawing
                Raylib.EndDrawing();
            }
        }
    }
}
