using System;
using System.Text.Json;

namespace GameConsole
{
    class SaveState
    {
        public List<string[]> CurrentBoardState { get; set; }

        public Player CurrentPlayer { get; set; }

        public bool IsComputerOpponent { get; set; }

        public SaveState(List<string[]> currentBoardState, Player currentPlayer, bool isComputerOpponent)
        {
            CurrentBoardState = currentBoardState;
            CurrentPlayer = currentPlayer;
            IsComputerOpponent = isComputerOpponent;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("WELCOME TO GRID GAMES!!!!!\n");


            while (true)
            {
                Console.WriteLine("Choose a game:");
                Console.WriteLine("Enter 1 for Gomoku");
                Console.WriteLine("Enter 2 for Notakto");
                Console.Write(">> ");
                string choice = Console.ReadLine();

                if (choice != "1" && choice != "2")
                {
                    Console.WriteLine("\nInvalid Choice. Try Again...\n");
                    continue;
                }

                Game game;
                SaveState saveState = null;
                GameLoop loop;
                Player currentPlayer;
                bool undoOrRedoLastMove = false;
                string filePath = "";

                if (choice == "1")
                {
                    // Look for existing save
                    try
                    {
                        filePath = "Gomoku.json";
                        string gomokuSaveJsonString = File.ReadAllText(filePath);
                        saveState = JsonSerializer.Deserialize<SaveState>(gomokuSaveJsonString);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine($"\nNo saved game found. Starting a new game.\n");
                    }

                    game = new GameGomoku();
                }
                else
                {
                    // Look for existing save
                    try
                    {
                        filePath = "Notakto.json";
                        string notaktoSaveJsonString = File.ReadAllText(filePath);
                        File.WriteAllText(filePath, "");
                        saveState = JsonSerializer.Deserialize<SaveState>(notaktoSaveJsonString);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine($"\nNo saved game found. Starting a new game.\n");
                    }

                    game = new GameNotakto();
                }

                if (saveState != null)
                {
                    Console.WriteLine("A saved game was found.");
                    Console.WriteLine("Enter 1 to resume the saved game.");
                    Console.WriteLine("Enter 2 to start a new game.");
                    Console.Write(">> ");
                    string loadChoice = Console.ReadLine();

                    if (loadChoice == "1")
                    {
                        loop = new GameLoop(game, saveState.IsComputerOpponent);
                        game.CreateBoardFromSave(saveState.CurrentBoardState, saveState.CurrentPlayer);
                        currentPlayer = saveState.CurrentPlayer;
                        File.WriteAllText(filePath, "");
                    }
                    else
                    {
                        saveState = null; // Discard saved state and start a new game
                    }
                }

                if (saveState == null)
                {
                    while (true)
                    {
                        Console.WriteLine("Choose your opponent:");
                        Console.WriteLine("Enter 1 for Human");
                        Console.WriteLine("Enter 2 for Computer");
                        Console.Write(">> ");
                        string modeChoice = Console.ReadLine();

                        if (modeChoice == "1")
                        {
                            loop = new GameLoop(game, false);
                            break;
                        }
                        else if (modeChoice == "2")
                        {
                            loop = new GameLoop(game, true);
                            break;
                        }
                        else 
                        {
                            Console.WriteLine("\nInvalid Choice. Please enter 1 or 2.");
                        }
                    }
                    
                    Console.Clear();
                    game.GameIntroduction();
                    game.Initialize();

                    currentPlayer = loop.Player1;
                }
                else
                {
                    loop = new GameLoop(game, saveState.IsComputerOpponent);
                    game.CreateBoardFromSave(saveState.CurrentBoardState, saveState.CurrentPlayer);
                    currentPlayer = saveState.CurrentPlayer;

                }

                while (!game.IsGameOver())
                {
                    Console.Clear();
                    game.DisplayBoard();
                    Console.WriteLine($"\n{currentPlayer.PlayerName}'s turn");

                    if (currentPlayer == loop.Player2 && loop.IsComputerOpponent && !undoOrRedoLastMove)
                    {
                        // Automatically make the computer's move
                        game.MakeComputerMove(loop.Player2);
                        currentPlayer = loop.Player1;  // Switch back to the human player after the computer's move
                    }
                    else
                    {
                        undoOrRedoLastMove = false;  // Reset the undo/redo flag for human players
                        Console.WriteLine("\nOptions: ");
                        Console.WriteLine("1. Make a Move");
                        Console.WriteLine("2. Undo Last Move");
                        Console.WriteLine("3. Redo Last Move");
                        Console.WriteLine("4. Get Help Instructions");
                        Console.WriteLine("5. Save Game");
                        Console.WriteLine("6. Exit Game");
                        Console.Write("Enter your choice >> ");

                        string actionChoice = Console.ReadLine();

                        if (actionChoice == "1")
                        {
                            game.MakeMove(currentPlayer);
                            currentPlayer = (currentPlayer == loop.Player1) ? loop.Player2 : loop.Player1;
                        }
                        else if (actionChoice == "2")
                        {
                            if (loop.IsComputerOpponent)
                            {
                                // Undo both player & computer's moves
                                if (game.UndoMove() && game.UndoMove())
                                {
                                    undoOrRedoLastMove = true;
                                    // Keep the current player the same, as it should remain the player's turn after undoing
                                }
                                else
                                {
                                    Console.WriteLine("No more moves to undo.");
                                }
                            }
                            else
                            {
                                // Standard single undo for human vs human
                                if (game.UndoMove())
                                {
                                    undoOrRedoLastMove = true;
                                    currentPlayer = (currentPlayer == loop.Player1) ? loop.Player2 : loop.Player1;
                                }
                                else
                                {
                                    Console.WriteLine("No more moves to undo.");
                                }
                            }
                        }
                        else if (actionChoice == "3")
                        {
                            if (loop.IsComputerOpponent)
                            {
                                // Redo both player & computer's moves
                                if (game.RedoMove() && game.RedoMove())
                                {
                                    undoOrRedoLastMove = true;
                                    // Keep the current player the same, as it should remain the player's turn after redoing
                                }
                                else
                                {
                                    Console.WriteLine("No more moves to redo.");
                                }
                            }
                            else
                            {
                                // Standard single redo for human vs human
                                if (game.RedoMove())
                                {
                                    undoOrRedoLastMove = true;
                                    currentPlayer = (currentPlayer == loop.Player1) ? loop.Player2 : loop.Player1;
                                }
                                else
                                {
                                    Console.WriteLine("No more moves to redo.");
                                }
                            }
                        }

                        else if (actionChoice == "4")
                        {
                            Console.Clear();
                            game.GameIntroduction();
                            game.DisplayBoard();
                        }
                        else if (actionChoice == "5")
                        {
                            var json = JsonSerializer.Serialize(new SaveState(game.GetBoardState(), currentPlayer, loop.IsComputerOpponent));
                            Console.WriteLine(json);
                            filePath = $"{game.GetGameName()}.json";
                            File.WriteAllText(filePath, json);
                        }
                        else if (actionChoice == "6")
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid choice. Please try again.");
                            continue;
                        }
                    }

                    // Check if the game is over after every move
                    if (game.IsGameOver())
                    {
                        break;
                    }
                }
                if (game.GetWinner() != null)
                {
                    Player winner = game.GetWinner();
                    if (winner != null)
                    {
                        Console.Clear();
                        game.DisplayBoard();
                        Console.WriteLine();
                        Console.WriteLine($"{winner.PlayerName} wins the game!\n");  // Print winner's name
                    }
                    else
                    {
                        Console.Clear();
                        game.DisplayBoard();
                        Console.WriteLine();
                        Console.WriteLine("It's a draw!");
                    }
                }
                if (game.GetLoser() != null)
                {
                    Player loser = game.GetLoser();
                    if (loser != null)
                    {
                        Console.Clear();
                        game.DisplayBoard();
                        Console.WriteLine();
                        Console.WriteLine($"{loser.PlayerName} loses the game!\n");  // Print winner's name
                    }
                    else
                    {
                        Console.Clear();
                        game.DisplayBoard();
                        Console.WriteLine();
                        Console.WriteLine("It's a draw!");
                    }
                }


                while (true)
                {
                    Console.WriteLine("\nDo you want to end the game or return to the main menu?");
                    Console.WriteLine("Enter 1 to leave Grid Games");
                    Console.WriteLine("Enter 2 to Return to Main Menu");
                    Console.Write(">> ");

                    string endChoice = Console.ReadLine();
                    if (endChoice == "1")
                    {
                        Console.WriteLine("Exiting Grid Games. See ya!");
                        return; // Exit entire application
                    }
                    else if (endChoice == "2")
                    {
                        break; //Breaks loop and returns to main menu 
                    }
                    else 
                    {
                        Console.WriteLine("Invalid choice. Please enter 1 or 2.");
                    }
                }

                Console.Clear();
                
            }
        }
    }
}
