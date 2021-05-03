using System;
using Raylib_cs;

namespace ChessButCool
{
    public class Game
    {
        private readonly int width;
        private readonly int height;
        private int state = 0;
        private ChessBoard board;
        private UI ui;
        private StartMenu menu;

        // Constructor getting the width and height
        public Game(int width, int height)
        {
            this.width = width;
            this.height = height;

            // Draws for first time
            Draw();
        }

        // Drawing the game
        public void Draw()
        {
            Raylib.InitWindow(width, height, "Chess");

            // generate chessboard and ui
            board = new(700, new Vector2Int(50, 50), this);
            ui = new(230, 700, new Vector2Int(760, 50), board);

            // defines menu
            menu = new(width, new Vector2Int(0, 0), this);

            // update function
            while (!Raylib.WindowShouldClose())
            {
                // begins drawing
                Raylib.BeginDrawing();
                // draw bg
                Raylib.ClearBackground(new Color(163, 163, 163, 255));

                // Updates everything
                Update();

                // ends drawing
                Raylib.EndDrawing();
            }
        }
        
        // Updates differently for every state
        private void Update()
        {
            switch (state)
            {
                case 0:
                    // Updates and draws menu
                    menu.Update();
                    menu.Draw();
                    break;
                case 1:
                    // Updates and draws board
                    board.Update();
                    board.Draw();

                    // Draws UI
                    ui.Draw();
                    break;
                case 2:
                    // Starts a new board and goes to state 1
                    newBoard();
                    break;
                default:
                    break;
            }
        }

        // Intances a new board and moves to state 1
        private void newBoard()
        {
            board = new(700, new Vector2Int(50, 50), this);
            ui = new(230, 700, new Vector2Int(760, 50), board);

            state = 1;
        }

        // Property for state
        public int State
        {
            get { return state; }
            set { state = value; }
        }

    }
}
