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
        Triple<int, int, Piece>[,] map = new Triple<int, int, Piece>[8, 8];
        List<Piece> pieces = new List<Piece>();
        private readonly string StartingFEN = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR";
        private readonly string basePath = "Sprites/";

        public ChessBoard(int width, Vector2Int pos)
        {
            this.width = width;
            this.pos = pos;
            sqWidth = (int)(width / 8.0f);

            StartBoard();
            FENStringConverter(StartingFEN);
        }

        public void DeBuggerBoard()
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    if (!map[x, y].GetnoVal3())
                    {
                        Console.Write(map[x, y].Value3.Position.X + "," + map[x, y].Value3.Position.Y + " ");
                    }
                }
                Console.Write("\n");
            }
        }

        public void Draw()
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    int xPos = ((sqWidth * x)) + (int)pos.X;
                    int yPos = ((sqWidth * y)) + (int)pos.Y;

                    // Drawing Colors
                    if (map[x, y].Value1 == 0) // if place is white
                    {
                        Raylib.DrawRectangle(xPos, yPos, sqWidth, sqWidth, Color.WHITE);
                    }
                    else // if place is black
                    {
                        Raylib.DrawRectangle(xPos, yPos, sqWidth, sqWidth, new Color(29, 112, 89, 255));
                    }

                    if (map[x, y].Value2 == 1) // if you can move to place
                    {
                        Raylib.DrawRectangle(xPos, yPos, sqWidth, sqWidth, new Color(118, 135, 57, (int)(0.6f * 255)));
                    }

                    if (!map[x, y].GetnoVal3()) // if piece exists on position
                    {
                        // image loading
                        string path = basePath + map[x, y].Value3.PieceType + ".png";
                        Image piece = Raylib.LoadImage(path);
                        Raylib.ImageResize(ref piece, sqWidth, sqWidth);
                        Texture2D texture = Raylib.LoadTextureFromImage(piece);
                        Raylib.DrawTexture(texture, xPos, yPos, Color.WHITE);

                        Raylib.UnloadImage(piece);
                    }
                }
            }
        }

        public bool CheckIFPOSISBRUH()
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    if (!map[x, y].GetnoVal3())
                    {
                        if (map[x, y].Value3.Position.X == 8 && map[x, y].Value3.Position.Y == 7)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public void Update()
        {
            Clicked();
            if (CheckIFPOSISBRUH())
                System.Console.WriteLine("ok");
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

                    Console.WriteLine(mapX + ", " + mapY);

                    // bruh orkar inte
                    if (!map[mapX, mapY].GetnoVal3())
                    {
                        map[mapX, mapY].Value3.ShowMoves(map);
                    }
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
                        currentPos.X += (int)char.GetNumericValue(item) - 1;
                    }
                    else if (char.IsUpper(item))
                    {
                        map[currentPos.X, currentPos.Y].Value3 = Piece.GetPieceFromPieceType("0" + item.ToString().ToUpper(), currentPos);
                    }
                    else
                    {
                        map[currentPos.X, currentPos.Y].Value3 = Piece.GetPieceFromPieceType("1" + item.ToString().ToUpper(), currentPos);
                    }

                    if (!char.IsDigit(item) && currentPos.X < 8)
                    {
                        currentPos.X++;
                    }
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
                        Triple<int, int, Piece> triple = new();
                        triple.SetValue(0, 0);

                        map[x, y] = triple;
                    }
                    else // if not black square
                    {
                        Triple<int, int, Piece> triple = new();
                        triple.SetValue(1, 0);

                        map[x, y] = triple;
                    }
                }
            }
        }
    }
}
