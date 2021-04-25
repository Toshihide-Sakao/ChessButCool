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
        int turn = 0;

        Image[][] imageArray;
        Pair<bool, Piece> ShowingMoves = new Pair<bool, Piece>();

        public ChessBoard(int width, Vector2Int pos)
        {
            this.width = width;
            this.pos = pos;
            sqWidth = (int)(width / 8.0f);
            ShowingMoves.SetValue(false);

            StartBoard(); // Creating board checkred board
            FENStringConverter(StartingFEN); // puts pieces in starting position
            LoadImages(); // Loading all images
        }

        // debug ----------------------
        public void DeBuggerBoard()
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    if (!map[x, y].NoVal3)
                    {
                        Console.Write(map[x, y].Value3.Position.X + "," + map[x, y].Value3.Position.Y + " ");
                    }
                }
                Console.Write("\n");
            }
        }
        // -----------------------------

        public void LoadImages() // Loads all piece images
        {
            imageArray = new Image[2][];
            for (int i = 0; i < 2; i++)
            {
                imageArray[i] = new Image[]
                {
                    Raylib.LoadImage(basePath + i.ToString() + "P" + ".png"),
                    Raylib.LoadImage(basePath + i.ToString() + "N" + ".png"),
                    Raylib.LoadImage(basePath + i.ToString() + "B" + ".png"),
                    Raylib.LoadImage(basePath + i.ToString() + "R" + ".png"),
                    Raylib.LoadImage(basePath + i.ToString() + "Q" + ".png"),
                    Raylib.LoadImage(basePath + i.ToString() + "K" + ".png"),
                };
            }
        }

        public void Draw()
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    // Calculates position of where to draw.
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

                    if (!map[x, y].NoVal3) // if piece exists on position
                    {
                        // Gets piece values which is used to find which sprite to draw.
                        Vector2Int val = map[x, y].Value3.GetPieceNumbers();
                        // makes texture from loaded image in array
                        Texture2D texture = Raylib.LoadTextureFromImage(imageArray[val.X][val.Y]);

                        // fixes width and height of texture
                        texture.width = sqWidth;
                        texture.height = sqWidth;

                        // draws texture.
                        Raylib.DrawTexture(texture, xPos, yPos, Color.WHITE);
                    }
                }
            }
        }

        // Update command which checks for user inputs
        public void Update()
        {
            Clicked();
            ChooseMove();
        }

        private void Clicked()
        {
            if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_LEFT_BUTTON))
            {
                // Resets highlighted positions
                UnClick();
                Vector2Int mousepos = new Vector2Int(Raylib.GetMousePosition());

                if (mousepos.X > pos.X && mousepos.X < (pos.X + width) && mousepos.Y > pos.Y && mousepos.Y < (pos.Y + width))
                {
                    int mapX = (mousepos.X - pos.X) / sqWidth;
                    int mapY = (mousepos.Y - pos.Y) / sqWidth;

                    // bruh orkar inte
                    if (!map[mapX, mapY].NoVal3)
                    {
                        if (turn % 2 == (int)map[mapX, mapY].Value3.Side)
                        {
                            map[mapX, mapY].Value3.ShowMoves();
                            ShowingMoves.SetValue(true, map[mapX, mapY].Value3);
                        }

                    }
                }
            }
        }

        private void ChooseMove()
        {
            if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_LEFT_BUTTON) && ShowingMoves.Value1)
            {
                Vector2Int mousepos = new Vector2Int(Raylib.GetMousePosition());

                if (mousepos.X > pos.X && mousepos.X < (pos.X + width) && mousepos.Y > pos.Y && mousepos.Y < (pos.Y + width))
                {
                    int mapX = (mousepos.X - pos.X) / sqWidth;
                    int mapY = (mousepos.Y - pos.Y) / sqWidth;

                    bool moved = ShowingMoves.Value2.Move(new Vector2Int(mapX, mapY));

                    // add turn
                    if (moved)
                    {
                        turn++;
                    }
                }
            }
        }

        private void UnClick()
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    map[x, y].Value2 = 0;
                }
            }
        }

        // Method for converting FENstring to the map array.
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
                        map[currentPos.X, currentPos.Y].Value3 = Piece.GetPieceFromPieceType("0" + item.ToString().ToUpper(), currentPos, this);
                    }
                    else
                    {
                        map[currentPos.X, currentPos.Y].Value3 = Piece.GetPieceFromPieceType("1" + item.ToString().ToUpper(), currentPos, this);
                    }
                    pieces.Add(map[currentPos.X, currentPos.Y].Value3);

                    if (!char.IsDigit(item) && currentPos.X < 8)
                    {
                        currentPos.X++;
                    }
                }

            }
        }

        // Creating a checkred boared
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

        public Triple<int, int, Piece>[,] GetMap()
        {
            return map;
        }
    }
}
