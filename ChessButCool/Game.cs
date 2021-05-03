using System;
using Raylib_cs;

namespace ChessButCool
{
    public class Game
    {
        private readonly int width;
        private readonly int height;
        private int state = 0;

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
            ChessBoard board = new(700, new Vector2Int(50, 50), this);
            UI ui = new(230, 700, new Vector2Int(760, 50), board);

            // ok
            StartMenu menu = new(width, new Vector2Int(0,0), this);
            // update function
            while (!Raylib.WindowShouldClose())
            {
                // begins drawing
                Raylib.BeginDrawing();
                // draw bg
                Raylib.ClearBackground(new Color(163, 163, 163, 255));

                if (state == 1)
                {
                    // Updates and draws board
                    board.Update();
                    board.Draw();

                    ui.Draw();
                }
                else if (state == 0)
                {
                    menu.Update();
                    menu.Draw();
                }
                else if (state == 2)
                {
                    board = new(700, new Vector2Int(50, 50), this);
                    ui = new(230, 700, new Vector2Int(760, 50), board);

                    state = 1;
                }


                // ends drawing
                Raylib.EndDrawing();
            }
        }

        public int State
        {
            get { return state; }
            set { state = value; }
        }
        
    }
}
