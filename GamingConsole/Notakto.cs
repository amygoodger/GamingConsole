
using System;

namespace GameConsole
{
    public class GameNotakto : Game
    {
        private char[,] board1;
        private char[,] board2;
        private char[,] board3;
        private int size = 3;
        private Player currentPlayer;

        public override void GameIntroduction()
        {
            Console.WriteLine("Welcome to Notakto!\n");

            Console.WriteLine("How to play:");
            Console.WriteLine("1. The game is played on three 3x3 boards.");
            Console.WriteLine("2. Each player takes turns placing their marker ('X') on one of the boards.");
            Console.WriteLine("3. To make a move, you need to select which board (1-3) you want to play on.");
            Console.WriteLine("4. Then specify the row (1-3) and column (1-3) on the chosen board.");
            Console.WriteLine("5. The objective is to place your markers in a row, column, or diagonal on any board.");
            Console.WriteLine("\nHow to win:");
            Console.WriteLine("The player who makes the fills the last board with 3 X's in a row LOSES the game.");

            Console.WriteLine("\n(To request help again, enter '4' at any time during the game.)\n");

            Console.WriteLine("\nEnter 'yes' to begin!");
            string input = Console.ReadLine();
            while (input != "yes")
            {
                Console.WriteLine("Doesn't sound like you're ready...");
                Console.WriteLine("Enter 'yes' to begin!");
                input = Console.ReadLine().Trim().ToLower();
            }
        }
        private bool CheckWinningCondition(char[,] board)
        {
            // Check horizontal
            for (int i = 0; i < size; i++)
            {
                if (board[i, 0] == 'X' && board[i, 1] == 'X' && board[i, 2] == 'X')
                {
                    return true;
                }
            }
            // Check vertical
            for (int j = 0; j < size; j++)
            {
                if (board[0, j] == 'X' && board[1, j] == 'X' && board[2, j] == 'X')
                {
                    return true;
                }
            }
            // Check diagonals
            if (board[0, 0] == 'X' && board[1, 1] == 'X' && board[2, 2] == 'X')
            {
                return true;
            }
            if (board[0, 2] == 'X' && board[1, 1] == 'X' && board[2, 0] == 'X')
            {
                return true;
            }

            return false;
        }


        public override void Initialize()
        {
            board1 = new char[size, size];
            board2 = new char[size, size];
            board3 = new char[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    board1[i, j] = '.';
                    board2[i, j] = '.';
                    board3[i, j] = '.';
                }
            }
            currentPlayer = null;
            SaveState(GetCombinedBoards(), currentPlayer);
        }

        public override bool IsGameOver()
        {
            return CheckWinningCondition(board1) && CheckWinningCondition(board2) && CheckWinningCondition(board3);
        }


        public override void DisplayBoard()
        {
            Console.WriteLine("NOTAKTO\n");

            Console.WriteLine("Board 1:");
            DisplaySingleBoard(board1);
            Console.WriteLine("Board 2:");
            DisplaySingleBoard(board2);
            Console.WriteLine("Board 3:");
            DisplaySingleBoard(board3);
        }

        private void DisplaySingleBoard(char[,] board)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Console.Write(board[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

        public override void MakeMove(Player player)
        {
            bool validMove = false;

            while (!validMove)
            {
                Console.WriteLine("Choose a board (1, 2, or 3): ");
                int boardChoice;

                // Validate board selection input
                while (!int.TryParse(Console.ReadLine(), out boardChoice) || boardChoice < 1 || boardChoice > 3)
                {
                    Console.WriteLine("Invalid input. Please enter 1, 2, or 3 to select a board.");
                }

                char[,] board = boardChoice switch
                {
                    1 => board1,
                    2 => board2,
                    3 => board3,
                };

                // Prevent move if the board already has three X's in a row
                if (CheckWinningCondition(board))
                {
                    Console.WriteLine($"Board {boardChoice} is complete with three X's in a row and cannot be used. Please choose a different board.");
                    continue; // Prompt the player to choose another board
                }

                Console.WriteLine("Enter row and column (e.g., 1 1): ");
                string[] input = Console.ReadLine().Split(' ');

                if (input.Length != 2 || !int.TryParse(input[0], out int row) || !int.TryParse(input[1], out int col) || row < 1 || row > 3 || col < 1 || col > 3)
                {
                    Console.WriteLine("Invalid input. Please enter the row and column as two numbers between 1 and 3, separated by a space.");
                    continue;
                }

                row -= 1; // Convert to zero-based index
                col -= 1; // Convert to zero-based index

                if (board[row, col] == '.')
                {
                    board[row, col] = 'X';
                    currentPlayer = player;
                    SaveState(GetCombinedBoards(), currentPlayer);  // Save the state after the move is completed
                    validMove = true;
                }
                else
                {
                    Console.WriteLine("Invalid move. That spot is already taken. Try again.");
                }
            }
        }
        
        public override void MakeComputerMove(Player computerPlayer)
        {
            Thread.Sleep(1250);
            Random rand = new Random();
            int boardChoice;
            char[,] board;

            do
            {
                boardChoice = rand.Next(1, 4);
                board = boardChoice switch
                {
                    1 => board1,
                    2 => board2,
                    3 => board3,
                    _ => board1,
                };
            } while (CheckWinningCondition(board));  // Keep choosing until a valid board is found

            int row, col;
            do
            {
                row = rand.Next(0, size);
                col = rand.Next(0, size);
            } while (board[row, col] != '.');

            board[row, col] = 'X';
            currentPlayer = computerPlayer;
            SaveState(GetCombinedBoards(), currentPlayer);  // Save the state after the move is completed
        }

        public override Player GetLoser()
        {
            // Check if any board has a winning condition (three 'X' in a row)
            if (CheckWinningCondition(board1) || CheckWinningCondition(board2) || CheckWinningCondition(board3))
            {
                return currentPlayer;  // Return the player who just made the last move
            }

            return null;  // No loser yet (game is not over)
        }
        public override Player GetWinner()
        {
            return null;
        }

        private bool IsBoardFull(char[,] board)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (board[i, j] == '.')
                    {
                        return false; // Found an empty spot
                    }
                }
            }
            return true; // No empty spots found
        }

        private char[,] GetCombinedBoards()
        {
            char[,] combined = new char[3 * size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    combined[i, j] = board1[i, j];
                    combined[i + size, j] = board2[i, j];
                    combined[i + 2 * size, j] = board3[i, j];
                }
            }
            return combined;
        }

        public override List<string[]> GetBoardState()
        {
            List<string[]> state = new List<string[]>();

            // Save state for board 1
            for (int i = 0; i < board1.GetLength(0); i++)
            {
                for (int j = 0; j < board1.GetLength(1); j++)
                {
                    if (board1[i, j] != '.')
                    {
                        string[] coords = { board1[i, j].ToString(), i.ToString(), j.ToString(), "1" };
                        state.Add(coords);
                    }
                }
            }

            // Save state for board 2
            for (int i = 0; i < board2.GetLength(0); i++)
            {
                for (int j = 0; j < board2.GetLength(1); j++)
                {
                    if (board2[i, j] != '.')
                    {
                        string[] coords = { board2[i, j].ToString(), i.ToString(), j.ToString(), "2" };
                        state.Add(coords);
                    }
                }
            }

            // Save state for board 3
            for (int i = 0; i < board3.GetLength(0); i++)
            {
                for (int j = 0; j < board3.GetLength(1); j++)
                {
                    if (board3[i, j] != '.')
                    {
                        string[] coords = { board3[i, j].ToString(), i.ToString(), j.ToString(), "3" };
                        state.Add(coords);
                    }
                }
            }

            return state;
        }

        public override void CreateBoardFromSave(List<string[]> boardSaveState, Player currentPlayerFromSave)
        {
            // Initialize the boards to empty
            board1 = new char[3, 3];
            board2 = new char[3, 3];
            board3 = new char[3, 3];

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    board1[i, j] = '.';
                    board2[i, j] = '.';
                    board3[i, j] = '.';
                }
            }

            // Restore the state for each board
            foreach (var item in boardSaveState)
            {
                char marker = item[0][0];
                int x = int.Parse(item[1]);
                int y = int.Parse(item[2]);
                int boardNumber = int.Parse(item[3]);

                if (boardNumber == 1)
                {
                    board1[x, y] = marker;
                }
                else if (boardNumber == 2)
                {
                    board2[x, y] = marker;
                }
                else if (boardNumber == 3)
                {
                    board3[x, y] = marker;
                }
            }

            currentPlayer = currentPlayerFromSave;
            SaveState(board1, currentPlayer); // Save the state for board 1
            SaveState(board2, currentPlayer); // Save the state for board 2
            SaveState(board3, currentPlayer); // Save the state for board 3
        }


        public override string GetGameName()
        {
            return "Notakto";
        }

        protected override void RestoreState(GameState state)
        {
            char[,] combined = state.Board;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    board1[i, j] = combined[i, j];
                    board2[i, j] = combined[i + size, j];
                    board3[i, j] = combined[i + 2 * size, j];
                }
            }
            currentPlayer = state.CurrentPlayer;
        }

        protected override char[,] GetBoard()
        {
            return GetCombinedBoards();
        }

        protected override Player GetCurrentPlayer()
        {
            return currentPlayer;
        }

    }
}
