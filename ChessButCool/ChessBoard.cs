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
        List<Piece> pieces = new List<Piece>(); // FIXME: Bassically not in use, can remove
        private readonly string StartingFEN = "8/3b4/1k6/6R1/P7/N7/4K3/B6n";
        private readonly string basePath = "Sprites/";
        int turn = 0;
        bool[] check = new bool[2];

        Image[][] imageArray;
        Texture2D[][] textureArray;
        Pair<bool, Piece> ShowingMoves = new Pair<bool, Piece>();

        public ChessBoard(int width, Vector2Int pos)
        {
            this.width = width;
            this.pos = pos;
            sqWidth = (int)(width / 8.0f);
            ShowingMoves.SetValue(false);

            StartBoard(); // Creating board checkred board
            FENStringConverter(StartingFEN); // puts pieces in starting position
            LoadTextures(); // Loading all textures from images
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

        private void LoadImages() // Loads all piece images
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

        private void LoadTextures() // Loads all texture
        {
            LoadImages();
            textureArray = new Texture2D[2][];
            for (int i = 0; i < imageArray.Length; i++)
            {
                textureArray[i] = new Texture2D[imageArray[i].Length];
                for (int j = 0; j < imageArray[i].Length; j++)
                {
                    textureArray[i][j] = Raylib.LoadTextureFromImage(imageArray[i][j]);
                    textureArray[i][j].width = sqWidth;
                    textureArray[i][j].height = sqWidth;
                }
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

                    // highlights
                    switch (map[x, y].Value2)
                    {
                        case 1:
                            Raylib.DrawRectangle(xPos, yPos, sqWidth, sqWidth, new Color(118, 135, 57, (int)(0.6f * 255)));
                            break;
                        case 2:
                            Raylib.DrawRectangle(xPos, yPos, sqWidth, sqWidth, new Color(232, 74, 101, (int)(0.6f * 255)));
                            break;
                        case 3:
                            Raylib.DrawRectangle(xPos, yPos, sqWidth, sqWidth, new Color(255, 0, 0, (int)(0.7f * 255)));
                            break;
                        case 99:
                            Raylib.DrawRectangle(xPos, yPos, sqWidth, sqWidth, new Color(17, 208, 212, (int)(0.6f * 255)));
                            break;
                    }

                    if (!map[x, y].NoVal3) // if piece exists on position
                    {
                        // Gets piece values which is used to find which sprite to draw.
                        Vector2Int val = map[x, y].Value3.GetPieceNumbers();

                        // draws texture.
                        Raylib.DrawTexture(textureArray[val.X][val.Y], xPos, yPos, Color.WHITE);
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
                        if (Turn % 2 == (int)map[mapX, mapY].Value3.Side)
                        {
                            map[mapX, mapY].Value3.ShowMoves();
                            ShowingMoves.SetValue(true, map[mapX, mapY].Value3);
                            map[mapX, mapY].Value2 = 2;
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

                    // Vector2Int oldPos = ShowingMoves.Value2.Position;
                    bool moved = ShowingMoves.Value2.Move(new Vector2Int(mapX, mapY));

                    // add turn
                    if (moved)
                    {
                        ShowingMoves.Value1 = false;

                        CheckForCheck((SideColor)(turn % 2));
                        Turn++;
                    }
                }
            }
        }

        // Checking for check (color is for the one attacking the king)
        private void CheckForCheck(SideColor color)
        {
            SideColor kingColor = color == SideColor.White ? SideColor.Black : SideColor.White; // getting the opposite color for the king
            Vector2Int kingPos = GetKingPos(kingColor); // Getting the king position

            // Getting allmoves from the attacking side color
            var allMoves = ListAllMoves(color);

            // if king is being attacked
            if (allMoves.Contains(kingPos))
            {
                // king side color is checked
                check[(int)kingColor] = true;

                // highlighting the king
                map[kingPos.X, kingPos.Y].Value2 = 3;
            }
            // if king is not attacked
            else
            {
                check[(int)kingColor] = false;
            }
        }

        // Gets the position of the king from color
        private Vector2Int GetKingPos(SideColor color)
        {
            Vector2Int kingPos = new Vector2Int(-99, -99); // a temporary position

            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    // if there is a piece, piece is a king, the king has the right color
                    if (!map[x, y].NoVal3 && map[x, y].Value3 is King && map[x, y].Value3.Side == color)
                    {
                        kingPos = new Vector2Int(x, y);
                    }
                }
            }

            // there was no king found
            if (kingPos.X == -99)
            {
                // throw an error
                throw new Exception("no King");
            }

            return kingPos; // return the positon of the king
        }

        // Returns a list with all possible moves on one side
        private List<Vector2Int> ListAllMoves(SideColor color)
        {
            // Makes a new list which will be returned
            List<Vector2Int> allMoves = new();

            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    // if there is a piece and the piece is the color
                    if (!map[x, y].NoVal3 && map[x, y].Value3.Side == color)
                    {
                        // adds a list which contains all possible moves the piece coud do.
                        allMoves.AddRange(map[x, y].Value3.GetPublicMoves());
                    }
                }
            }

            // return the list
            return allMoves;
        }

        private List<Piece> ListAllPieces(SideColor color)
        {
            // Makes a new list which will be returned
            List<Piece> allPieces = new();

            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    // if there is a piece and the piece is the color
                    if (!map[x, y].NoVal3 && map[x, y].Value3.Side == color)
                    {
                        // adds a list which contains all possible moves the piece coud do. FIXME: just chnage comment
                        allPieces.Add(map[x, y].Value3);
                    }
                }
            }

            // return the list
            return allPieces;
        }

        // remove invalid moves when checked (color is the one is cheked)
        private List<Vector2Int> RemoveInvalidMoves(List<Vector2Int> alliedMoves, SideColor checkedColor)
        {
            SideColor oppositeColor = checkedColor == SideColor.White ? SideColor.Black : SideColor.White;
            if (check[(int)checkedColor])
            {
                alliedMoves = new();
                var oppositeMoves = ListAllMoves(oppositeColor);
                var alliedPieces = ListAllPieces(checkedColor);

                // do simulation if check is removed
                for (int i = 0; i < alliedPieces.Count; i++)
                {
                    var publicMoves = alliedPieces[i].GetPublicMoves(); // list for all public moves for the piece right now
                    var oldPos = alliedPieces[i].Position;
                    List<int> IntsToRemove = new List<int>();

                    for (int j = 0; j < publicMoves.Count; j++)
                    {
                        alliedPieces[i].Move(publicMoves[j]); // tries out the move

                        CheckForCheck(oppositeColor);
                        if (check[(int)checkedColor])
                        {
                            IntsToRemove.Add(j);
                        }

                        alliedPieces[i].Move(oldPos);
                    }

                    for (int j = IntsToRemove.Count - 1; j >= 0; j--)
                    {
                        publicMoves.RemoveAt(IntsToRemove[j]);
                    }

                    alliedMoves.AddRange(publicMoves);
                }
                return alliedMoves;
            }
            return alliedMoves;
        }

        // TODO: Checkmate
        private void CheckForCheckmate()
        {
            // TODO: Need to get a list for all moves that are possible with checked
            // if that list.count == 0  then check mate
        }
        // Returns all value2(highlights) to not highlighted.
        private void UnClick()
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    // Makes non highlighted
                    map[x, y].Value2 = 0;
                }
            }
        }

        // Method for converting FENstring to the map array.
        private void FENStringConverter(string fen)
        {
            // A vector2 for recording the current position
            Vector2Int currentPos = new Vector2Int(0, 0);

            foreach (char item in fen)
            {
                // skip a row
                if (item == '/')
                {
                    currentPos.X = 0;
                    currentPos.Y++;
                }
                else
                {
                    // skip a number to the right
                    if (char.IsDigit(item))
                    {
                        currentPos.X += (int)char.GetNumericValue(item);
                    }
                    // A piece which is white
                    else if (char.IsUpper(item))
                    {
                        map[currentPos.X, currentPos.Y].Value3 = Piece.GetPieceFromPieceType("0" + item.ToString().ToUpper(), currentPos, this);
                    }
                    // A piece which is black
                    else
                    {
                        map[currentPos.X, currentPos.Y].Value3 = Piece.GetPieceFromPieceType("1" + item.ToString().ToUpper(), currentPos, this);
                    }
                    // adding the 
                    // pieces.Add(map[currentPos.X, currentPos.Y].Value3); // possibly remove FIXME:

                    // add a position if the char was representing a piece
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

        public int Turn
        {
            get { return turn; }
            private set { turn = value; }
        }
        public Triple<int, int, Piece>[,] GetMap()
        {
            return map;
        }
    }
}
