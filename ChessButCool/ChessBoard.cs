using System;
using System.Collections.Generic;
using ChessButCool.Pieces;
using Raylib_cs;

namespace ChessButCool
{
    public class ChessBoard
    {
        Vector2Int pos;
        int width;
        int sqWidth;
        Pair<int, Piece>[,] map = new Pair<int, Piece>[8, 8];
        List<Piece> pieces = new List<Piece>();
        private readonly string StartingFEN = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR";
        private readonly string basePath = "Sprites/";
        // Dictionary<string, Image> images = new Dictionary<string, Image>();

        public ChessBoard(int width, Vector2Int pos)
        {
            this.width = width;
            this.pos = pos;
            sqWidth = (int)(width / 8.0f);

            StartBoard();
            FENStringConverter(StartingFEN);

            // for (int q = 0; q < 2; q++)
            // {
            //     foreach (string item in pieceSymbols)
            //     {
            //         Image sprite = Raylib.LoadImage(basePath + q.ToString() + item + ".png");
            //         images.Add(q.ToString() + item, sprite);
            //     }
            // }
        }

        // public void DeBuggerBoard()
        // {
        //     for (int y = 0; y < map.GetLength(1); y++)
        //     {
        //         for (int x = 0; x < map.GetLength(0); x++)
        //         {
        //             Console.Write(map[x, y].Value2);
        //         }
        //         Console.Write("\n");
        //     }
        // }

        public void Draw()
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    int xPos = ((sqWidth * x)) + (int)pos.X;
                    int yPos = ((sqWidth * y)) + (int)pos.Y;

                    // Drawing Colors
                    if (map[x, y].Value1 == 0)
                    {
                        Raylib.DrawRectangle(xPos, yPos, sqWidth, sqWidth, Color.WHITE);
                    }
                    else
                    {
                        Raylib.DrawRectangle(xPos, yPos, sqWidth, sqWidth, new Color(29, 112, 89, 255));
                    }
                    if (!map[x, y].GetnoVal2())
                    {
                        // if (map[x, y].Value2.GetType() !is Dummy)
                        // {
                            string path = basePath + map[x, y].Value2.PieceType + ".png";
                            Image piece = Raylib.LoadImage(path);
                            // Image piece = images[map[x, y].Value2];
                            Raylib.ImageResize(ref piece, sqWidth, sqWidth);
                            Texture2D texture = Raylib.LoadTextureFromImage(piece);
                            Raylib.DrawTexture(texture, xPos, yPos, Color.WHITE);

                            Raylib.UnloadImage(piece);
                        // }
                    }


                }
            }
        }

        public void Update()
        {

        }

        // Fix Click method
        public void Clicked()
        {
            if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_LEFT_BUTTON))
            {
                Vector2Int mousepos = new Vector2Int(Raylib.GetMousePosition());

                if (mousepos.X > pos.X && mousepos.X < (pos.X + width) && mousepos.Y > pos.Y && mousepos.Y < (pos.Y + width))
                {
                    int mapX = (mousepos.X - pos.X) / sqWidth;
                    int mapY = (mousepos.Y - pos.Y) / sqWidth;

                    // bruh orkar inte
                }
            }
        }

        private void FENStringConverter(string fen)
        {
            Vector2Int currentPos = new Vector2Int(0, 0);

            foreach (char item in fen)
            {
                if (item == '/')
                {
                    currentPos.X = 0;
                    currentPos.Y++;
                }
                else
                {
                    if (char.IsDigit(item))
                    {
                        currentPos.X += (int)char.GetNumericValue(item);
                    }
                    else if (char.IsUpper(item))
                    {

                        map[currentPos.X, currentPos.Y].Value2 = Piece.GetPieceFromPieceType("0" + item.ToString().ToUpper(), currentPos);
                    }
                    else
                    {
                        map[currentPos.X, currentPos.Y].Value2 = Piece.GetPieceFromPieceType("1" + item.ToString().ToUpper(), currentPos);
                    }
                    currentPos.X++;
                }
            }
        }

        private void StartBoard()
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    if ((x % 2 == 0 && y % 2 == 0) || (x % 2 == 1 && y % 2 == 1)) // if white square
                    {
                        Pair<int, Piece> pair = new();
                        pair.SetValue(0);

                        map[x, y] = pair;
                    }
                    else // if not black square
                    {
                        Pair<int, Piece> pair = new();
                        pair.SetValue(1);

                        map[x, y] = pair;
                    }

                }
            }
        }
    }
}
