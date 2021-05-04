using System;
using System.Collections.Generic;
using ChessButCool.Pieces;
using Raylib_cs;
using System.Linq;

namespace ChessButCool
{
    public class ChessBoard
    {
        private readonly Game game;
        private readonly Vector2Int pos;
        private readonly int width;
        private readonly int sqWidth;
        private Triple<int, int, Piece>[,] map = new Triple<int, int, Piece>[8, 8];
        private readonly string StartingFEN = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR";
        private readonly string basePath = "Sprites/";
        private int turn = 0;
        private bool[] check = new bool[2];

        private Image[][] imageArray;
        private Texture2D[][] textureArray;
        private Pair<bool, Piece> ShowingMoves = new();
        private Pair<bool, SideColor> checkmated = new();
        private Rectangle playAgainPop;
        private Rectangle exitMenuPop;

        // Constructor
        public ChessBoard(int width, Vector2Int pos, Game game)
        {
            this.width = width;
            this.pos = pos;
            sqWidth = (int)(width / 8.0f);
            ShowingMoves.SetValue(false);
            checkmated.SetValue(false);
            this.game = game;

            StartBoard(); // Creating board checkred board
            FENStringConverter(StartingFEN); // puts pieces in starting position
            LoadTextures(); // Loading all textures from images

            /*
            TODO: For network

            Client
                TcpClient
                Networkstream (Get from tcp client)
                stream.write / stream.read

            Server
                TcpListener
                TcpClient
                Server.acceptTcpClient
                needs threading for new clients
                networkstream
                send map back to client

            lcok for threadings to be safe ()

            TODO: Startmenu
            */
        }



        // Update command which checks for user inputs every frame
        public void Update()
        {
            Clicked();
            ChooseMove();
        }

        // Checks player click on piece he can move
        private void Clicked()
        {
            if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_LEFT_BUTTON))
            {
                Vector2Int mousepos = new(Raylib.GetMousePosition()); // Records mouse position when clicked

                // If checkmated popup is there or not
                if (!checkmated.Value1)
                {
                    UnClick(); // Resets highlighted positions
                    int offsetVal = 5; // offset for max value so it is inside board
                    // if mouse is inside of the chessboard
                    if (mousepos.X > pos.X && mousepos.X < (pos.X + width - offsetVal) && mousepos.Y > pos.Y && mousepos.Y < (pos.Y + width - offsetVal))
                    {
                        // Calculates position according to the map array
                        int mapX = (mousepos.X - pos.X) / sqWidth;
                        int mapY = (mousepos.Y - pos.Y) / sqWidth;

                        // if there is a piece on the position and if the piece color is their color they can move
                        if (!map[mapX, mapY].NoVal3 && Turn % 2 == (int)map[mapX, mapY].Value3.Side)
                        {
                            // Highlight what moves the piece can do
                            map[mapX, mapY].Value3.ShowMoves();

                            // record what piece is being selected
                            ShowingMoves.SetValue(true, map[mapX, mapY].Value3);

                            // Highlight the piece which is selected
                            map[mapX, mapY].Value2 = 2;
                        }
                    }
                }
                else
                {
                    // if they clicked the box of playagain
                    if (mousepos.X > playAgainPop.x && mousepos.X < (playAgainPop.x + playAgainPop.width) && mousepos.Y > playAgainPop.y && mousepos.Y < (playAgainPop.y + playAgainPop.height))
                    {
                        game.State = 2;

                        UnloadAll();
                    }
                    // if they clicked the box of exit to menu
                    else if (mousepos.X > exitMenuPop.x && mousepos.X < (exitMenuPop.x + exitMenuPop.width) && mousepos.Y > exitMenuPop.y && mousepos.Y < (exitMenuPop.y + exitMenuPop.height))
                    {
                        game.State = 0;
                        UnloadAll();
                    }
                }
            }
        }

        // Moves the piece to place clicked
        private void ChooseMove()
        {
            // If clicked and a piece is clicked and is showing moves.
            if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_LEFT_BUTTON) && ShowingMoves.Value1)
            {
                // records mouse position when clicked
                Vector2Int mousepos = new(Raylib.GetMousePosition());
                int offsetVal = 5; // offset for max value so it is inside board
                // mouse was clicked above the chessboard
                if (mousepos.X > pos.X && mousepos.X < (pos.X + width - offsetVal) && mousepos.Y > pos.Y && mousepos.Y < (pos.Y + width - offsetVal))
                {
                    // Calculates position according to the map array
                    int mapX = (mousepos.X - pos.X) / sqWidth;
                    int mapY = (mousepos.Y - pos.Y) / sqWidth;

                    // Moves the piece as well as recording if the move was done
                    bool moved = ShowingMoves.Value2.Move(new Vector2Int(mapX, mapY));

                    // if move was successful
                    if (moved)
                    {
                        // reset that no piece is selected
                        ShowingMoves.Value1 = false;

                        SideColor movedColor = (SideColor)(turn % 2); // the side that moved
                        SideColor oppositeColor = (SideColor)(1 - (turn % 2)); // the side that is going to move next turn

                        check[(int)oppositeColor] = CheckForCheck(movedColor); // record if the move checked
                        if (check[(int)oppositeColor]) // If they were cheked
                        {
                            // Cheking if they were mated
                            checkmated = CheckForCheckmate(oppositeColor);
                        }
                        // adds turn
                        Turn++;
                    }
                }
            }
        }

        // Checking for check 
        private bool CheckForCheck(SideColor attackingColor)
        {
            SideColor kingColor = (SideColor)(1 - attackingColor); // getting the opposite color for the king
            Vector2Int kingPos = GetKingPos(kingColor); // Getting the king position

            // Getting allmoves from the attacking side color
            var allMoves = ListAllMoves(attackingColor);

            // if king is being attacked
            if (allMoves.Contains(kingPos))
            {
                // highlighting the king
                map[kingPos.X, kingPos.Y].Value2 = 3;

                // king side color is checked
                return true;
            }
            // if king is not attacked
            else
            {
                return false;
            }
        }

        // Gets the position of the king from color
        private Vector2Int GetKingPos(SideColor color)
        {
            Vector2Int kingPos = new(-99, -99); // a temporary position

            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    // if there is a piece, piece is a king, the king has the right color
                    if (!map[x, y].NoVal3 && map[x, y].Value3 is King && map[x, y].Value3.Side == color)
                    {
                        // updates the kingPos
                        kingPos = new Vector2Int(x, y);
                        break;
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

        // Checks if there are any moves left for the color.
        private bool NoMovesLeft(SideColor color)
        {
            // Makes a new list which will have all move counts
            List<int> movesLeftPerPiece = new();

            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    // if there is a piece and the piece is the color
                    if (!map[x, y].NoVal3 && map[x, y].Value3.Side == color)
                    {
                        // adds the amount of moves possible on piece 
                        movesLeftPerPiece.Add(map[x, y].Value3.Moves.Count);
                    }
                }
            }

            // if every value in list is 0
            return movesLeftPerPiece.All(q => q == 0);
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
                        // adds the piece to list
                        allPieces.Add(map[x, y].Value3);
                    }
                }
            }

            // return the list
            return allPieces;
        }

        // Removes invalid moves caused by check for a single piece TODO: Move into piece class
        public void RemoveInvalidMovesPiece(Piece piece)
        {
            var publicMoves = piece.GetPublicMoves(); // list for all public moves for the piece right now
            Vector2Int oldPos = new(piece.Position.X, piece.Position.Y);
            List<int> IntsToRemove = new();

            for (int j = 0; j < publicMoves.Count; j++)
            {
                // assigns a temporary piece for if some piece was taken and if piece was taken stores it into takenpiece
                Piece takenPiece = null;
                bool isEnemyPieceThere = !map[publicMoves[j].X, publicMoves[j].Y].NoVal3 && map[publicMoves[j].X, publicMoves[j].Y].Value3.Side == (1 - piece.Side);
                if (isEnemyPieceThere)
                {
                    takenPiece = map[publicMoves[j].X, publicMoves[j].Y].Value3;
                }

                // tries out the move and updates moves
                piece.Move(publicMoves[j]);
                piece.GetPublicMoves();

                // If the move caused check
                if (CheckForCheck(1 - piece.Side))
                {
                    IntsToRemove.Add(j);
                }

                // DeBuggerBoard();
                // forcefully undo the move and update moves
                piece.Move(oldPos, true);
                piece.GetPublicMoves();

                // if a piece was taken then get it back
                if (isEnemyPieceThere)
                {
                    map[publicMoves[j].X, publicMoves[j].Y].Value3 = takenPiece;
                }
                // DeBuggerBoard();
            }
            piece.GetPublicMoves();

            // Removes all moves which was invalid
            for (int j = IntsToRemove.Count - 1; j >= 0; j--)
            {
                piece.Moves.RemoveAt(IntsToRemove[j]);
            }
        }

        // Checks for checkmate
        private Pair<bool, SideColor> CheckForCheckmate(SideColor checkedColor)
        {
            // makes value which will be returned
            Pair<bool, SideColor> res = new();
            res.SetValue(false);
            // assigns the opposite color
            SideColor oppositeColor = 1 - checkedColor;

            // if color is checked
            if (check[(int)checkedColor])
            {
                // Updates moves for oppositecolor TODO: Check if actually needed.
                ListAllMoves(oppositeColor);
                // assigns and updates moves for checkedcolor
                var alliedPieces = ListAllPieces(checkedColor);

                // do simulation if check is removed
                for (int i = 0; i < alliedPieces.Count; i++)
                {
                    // removes all illegal moves
                    RemoveInvalidMovesPiece(alliedPieces[i]);
                }
                // If there are no moves left to play
                if (NoMovesLeft(checkedColor))
                {
                    // sets return value to true and which color is the winner
                    res.SetValue(true, oppositeColor);
                }
            }
            // returns the value
            return res;
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

        // Draw method for every frame
        public void Draw()
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    // Calculates position of where to draw.
                    int xPos = (sqWidth * x) + pos.X;
                    int yPos = (sqWidth * y) + pos.Y;

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
                            // Can move to the postion
                            Raylib.DrawRectangle(xPos, yPos, sqWidth, sqWidth, new Color(118, 135, 57, (int)(0.6f * 255)));
                            break;
                        case 2:
                            // Where the piece is 
                            Raylib.DrawRectangle(xPos, yPos, sqWidth, sqWidth, new Color(17, 208, 212, (int)(0.6f * 255)));
                            break;
                        case 3:
                            // Highlight for king when in check
                            Raylib.DrawRectangle(xPos, yPos, sqWidth, sqWidth, new Color(255, 0, 0, (int)(0.5f * 255)));
                            break;
                        case 99:
                            // debug purpose
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

            // views end popup
            if (checkmated.Value1)
            {
                // Gets the width of end popup
                float endWidth = width / 1.6f;
                float endHeight = width / 2.4f;

                // Gets the centerposition and makes a rectangle
                Vector2Int centerPos = new(pos.X + (width / 2), pos.Y + (width / 2));
                Rectangle endPopup = new(centerPos.X - (endWidth / 2), centerPos.Y - (endHeight / 2), endWidth, endHeight);
                playAgainPop = new((int)endPopup.x + 40, (int)endPopup.y + (int)endPopup.height - 200, (int)endPopup.width - 80, 80);
                exitMenuPop = new((int)endPopup.x + 40, (int)endPopup.y + (int)endPopup.height - 100, (int)endPopup.width - 80, 80);

                // Draws the rectangle
                Raylib.DrawRectangle((int)endPopup.x, (int)endPopup.y, (int)endPopup.width, (int)endPopup.height, new Color(77, 77, 77, (int)(255 * 0.95f)));
                Raylib.DrawRectangle((int)playAgainPop.x, (int)playAgainPop.y, (int)playAgainPop.width, (int)playAgainPop.height, new Color(65, 157, 250, (int)(255 * 0.6f)));
                Raylib.DrawRectangle((int)exitMenuPop.x, (int)exitMenuPop.y, (int)exitMenuPop.width, (int)exitMenuPop.height, new Color(240, 146, 58, (int)(255 * 0.6f)));

                // Draws the text alligned at center and the top of the popup
                int textWidth = Raylib.MeasureText($"{checkmated.Value2} won!", 60);
                Raylib.DrawText($"{checkmated.Value2} won!", centerPos.X - (textWidth / 2), centerPos.Y - (int)((endHeight / 2) - (endHeight / 10)), 60, Color.WHITE);
                int playagainTextWidth = Raylib.MeasureText($"Play Again", 50);
                Raylib.DrawText($"Play Again", centerPos.X - (playagainTextWidth / 2), centerPos.Y - (int)((endHeight / 2) - (endHeight / 10)) + 75, 50, Color.WHITE);
                int exitToMenuTextWidth = Raylib.MeasureText($"Exit to Menu", 50);
                Raylib.DrawText($"Exit to Menu", centerPos.X - (exitToMenuTextWidth / 2), centerPos.Y - (int)((endHeight / 2) - (endHeight / 10)) + 180, 50, Color.WHITE);
            }
        }

        // Method for converting FENstring to the map array.
        private void FENStringConverter(string fen)
        {
            // A vector2 for recording the current position
            Vector2Int currentPos = new(0, 0);

            // Loop through the whole string
            foreach (char item in fen)
            {
                // skips a row
                if (item == '/')
                {
                    // Resets x and adds y value
                    currentPos.X = 0;
                    currentPos.Y++;
                }
                else
                {
                    // skips a certain number to the right
                    if (char.IsDigit(item))
                    {
                        currentPos.X += (int)char.GetNumericValue(item);
                    }
                    // A piece which is white
                    else if (char.IsUpper(item))
                    {
                        // Gets the piece item is pointed to
                        map[currentPos.X, currentPos.Y].Value3 = Piece.GetPieceFromPieceType("0" + item.ToString().ToUpper(), currentPos, this);
                        // Adds x value
                        currentPos.X++;
                    }
                    // A piece which is black
                    else
                    {
                        // Gets the piece item is pointed to
                        map[currentPos.X, currentPos.Y].Value3 = Piece.GetPieceFromPieceType("1" + item.ToString().ToUpper(), currentPos, this);
                        // Adds x value
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
                    // if white square
                    if ((x % 2 == 0 && y % 2 == 0) || (x % 2 == 1 && y % 2 == 1))
                    {
                        // Sets values for white square
                        Triple<int, int, Piece> triple = new();
                        triple.SetValue(0, 0);

                        // Setting it in the map
                        map[x, y] = triple;
                    }
                    else // if not black square
                    {
                        // Sets values for black square
                        Triple<int, int, Piece> triple = new();
                        triple.SetValue(1, 0);

                        // Setting it in the map
                        map[x, y] = triple;
                    }
                }
            }
        }

        // Loads all piece images
        private void LoadImages()
        {
            // Fills the image array
            imageArray = new Image[2][];
            // Loads sides one by one
            for (int i = 0; i < 2; i++)
            {
                imageArray[i] = new Image[]
                {
                    // Loads images on one side
                    Raylib.LoadImage(basePath + i.ToString() + "P" + ".png"),
                    Raylib.LoadImage(basePath + i.ToString() + "N" + ".png"),
                    Raylib.LoadImage(basePath + i.ToString() + "B" + ".png"),
                    Raylib.LoadImage(basePath + i.ToString() + "R" + ".png"),
                    Raylib.LoadImage(basePath + i.ToString() + "Q" + ".png"),
                    Raylib.LoadImage(basePath + i.ToString() + "K" + ".png"),
                };
            }
        }

        // Loads all texture
        private void LoadTextures()
        {
            // Loads the images
            LoadImages();

            // Fills the texturearray with textures
            textureArray = new Texture2D[2][];
            for (int i = 0; i < imageArray.Length; i++)
            {
                textureArray[i] = new Texture2D[imageArray[i].Length];
                for (int j = 0; j < imageArray[i].Length; j++)
                {
                    // Loads texture and sets height and width
                    textureArray[i][j] = Raylib.LoadTextureFromImage(imageArray[i][j]);
                    textureArray[i][j].width = sqWidth;
                    textureArray[i][j].height = sqWidth;
                }
            }
        }

        private void UnloadAll()
        {
            // unlaod all textures
            for (int i = 0; i < textureArray.Length; i++)
            {
                for (int j = 0; j < textureArray[i].Length; j++)
                {
                    Raylib.UnloadTexture(textureArray[i][j]);
                }
            }

            // unloads all images
            for (int i = 0; i < imageArray.Length; i++)
            {
                for (int j = 0; j < imageArray[i].Length; j++)
                {
                    Raylib.UnloadImage(imageArray[i][j]);
                }
            }
        }

        // debug ----------------------
        private void DeBuggerBoard()
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    if (!map[x, y].NoVal3)
                    {
                        Console.Write(map[x, y].Value3.PieceType + " ");
                    }
                    else
                    {
                        Console.Write("## ");
                    }
                }
                Console.Write("\n");
            }
            Console.WriteLine();
        }
        // -----------------------------

        // Properties
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
