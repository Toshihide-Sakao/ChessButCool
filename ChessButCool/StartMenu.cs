using System;
using Raylib_cs;

namespace ChessButCool
{
    public class StartMenu
    {
        private readonly Game game;
        private readonly Vector2Int pos;
        private readonly int width;
        private Rectangle playLocalRect;
        private Vector2Int centerPos;

        public StartMenu(int width, Vector2Int pos, Game game)
        {
            // sets values
            this.width = width;
            this.pos = pos;
            this.game = game;

            // Calculates centerposition
            centerPos = new(pos.X + (width / 2), pos.Y + (width / 2));
            
            // Measures and gets rect for hitbox and viewing
            int playLocalWidth = Raylib.MeasureText("Play local", 60);
            playLocalRect = new(centerPos.X - ((playLocalWidth + 20) / 2), centerPos.Y + 40, playLocalWidth + 20, 100);
        }

        // Checks for inputs from user
        public void Update()
        {
            if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_LEFT_BUTTON))
            {
                Vector2Int mousepos = new(Raylib.GetMousePosition());
                if (mousepos.X > playLocalRect.x && mousepos.X < (playLocalRect.x + playLocalRect.width) && mousepos.Y > playLocalRect.y && mousepos.Y < (playLocalRect.y + playLocalRect.height))
                {
                    game.State = 2;
                }
            }
        }

        // Draws text and triangle
        public void Draw()
        {
            // Draws title centeralligned
            int titleWidth = Raylib.MeasureText("Cool Chess xd", 80);
            Raylib.DrawText("Cool Chess xd", centerPos.X - (titleWidth / 2), centerPos.Y - 280, 80, Color.WHITE);

            // Draws plalocal center aligned as well as rectangle below
            int playLocalWidth = Raylib.MeasureText("Play local", 60);
            Raylib.DrawRectangle((int)playLocalRect.x, (int)playLocalRect.y, (int)playLocalRect.width, (int)playLocalRect.height, new Color(62, 131, 150, (int)(255 * 0.95f)));
            Raylib.DrawText("Play local", centerPos.X - (playLocalWidth / 2), centerPos.Y + 50, 60, Color.WHITE);
        }
    }
}
