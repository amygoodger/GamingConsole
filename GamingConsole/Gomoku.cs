
using System;

namespace GameConsole
{
    public class GameGomoku : Game
    {
        private char[,] board;
        private Player currentPlayer;

        public override void GameIntroduction()
        {
            Console.WriteLine("Welcome to Gomoku!\n");

            Console.WriteLine("How to play:");
            Console.WriteLine("1. The game board is a 15x15 grid.");
            Console.WriteLine("2. Players take turns to place their marker on the board.");
            Console.WriteLine("4. To make a move, specify the row (1-15) and column (1-15) on the chosen board.");
            Console.WriteLine("5. The objective is to place your markers in a row, column, or diagonal on any board.");
            Console.WriteLine("\nHow to win:");
            Console.WriteLine("The first player to get 5 markers in a row (horizontally, vertically, or diagonally) wins.");

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

        public override void Initialize()
        {
            board = new char[15, 15];
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    board[i, j] = '.';
                }
            }
            currentPlayer = null;
            SaveState(board, currentPlayer); // Save the initial state
        }

        public override bool IsGameOver()
        {
            // Check if there is a winner
            if (CheckForWinningCondition())
            {
                return true;
            }

            // Check if the board is full
            for (int row = 0; row < board.GetLength(0); row++)
            {
                for (int col = 0; col < board.GetLength(1); col++)
                {
                    if (board[row, col] == '.')
                    {
                        return false;  // The game is not over if there is at least one empty spot
                    }
                }
            }

            // If no empty spots are found, the board is full and the game is over
            return true;
        }


        public override void DisplayBoard()
        {
            Console.WriteLine("GOMOKU\n");

            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 15; j++)
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
                Console.WriteLine("Enter row and column (e.g., 1 1): ");
                string input = Console.ReadLine().Trim();

                string[] inputs = input.Split(' ');

                if (inputs.Length != 2)
                {
                    Console.WriteLine("Invalid input. Please enter two integers separated by a space.");
                    continue;
                }
                
                if (!int.TryParse(inputs[0], out int row) || !int.TryParse(inputs[1], out int col))
                {
                    Console.WriteLine("Invalid input. Please ensure both inputs are numbers.");
                    continue;
                }


                // Adjust the row and column to be 0-based for internal processing
                row -= 1;
                col -= 1;

                if (row < 0 || row >= board.GetLength(0) || col < 0 || col >= board.GetLength(1))
                {
                    Console.WriteLine("Invalid move. The coordinates are out of bounds. Try again.");
                    continue;
                }

                if (board[row, col] != '.')
                {
                    Console.WriteLine("Invalid move. The spot is already occupied. Choose another spot.");
                    continue;
                }

                // Place player's marker & mark the move as valid
                board[row, col] = player.Marker;
                currentPlayer = player;
                SaveState(board, currentPlayer);
                validMove = true;
            }
        }


        public override void MakeComputerMove(Player computerPlayer)
        {
            Thread.Sleep(1250);
            Random rand = new Random();
            int row, col;
            do
            {
                row = rand.Next(0, 15);
                col = rand.Next(0, 15);
            }
            while (board[row, col] != '.');

            board[row, col] = computerPlayer.Marker;
            currentPlayer = computerPlayer;
            SaveState(board, currentPlayer);
        }
        public override Player GetLoser()
        {

            return null;
        }

        public override Player GetWinner()
        {
            if (CheckForWinningCondition())
            {
                return currentPlayer;  // Return the player who just made the winning move
            }

            return null;  // Return null if no winner
        }

        // Example placeholder method to illustrate where the winning condition logic would go
        private bool CheckForWinningCondition()
        {
            // Check for horizontal win
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j <= 10; j++)
                {
                    if (board[i, j] != '.' && board[i, j] == board[i, j + 1] && board[i, j] == board[i, j + 2] &&
                        board[i, j] == board[i, j + 3] && board[i, j] == board[i, j + 4])
                    {
                        return true;
                    }
                }
            }
            // Check for vertical win
            for (int i = 0; i <= 10; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    if (board[i, j] != '.' && board[i, j] == board[i + 1, j] && board[i, j] == board[i + 2, j] &&
                        board[i, j] == board[i + 3, j] && board[i, j] == board[i + 4, j])
                    {
                        return true;
                    }
                }
            }

            // Check for diagonal win (top-left to bottom-right)
            for (int i = 0; i <= 10; i++)
            {
                for (int j = 0; j <= 10; j++)
                {
                    if (board[i, j] != '.' && board[i, j] == board[i + 1, j + 1] && board[i, j] == board[i + 2, j + 2] &&
                        board[i, j] == board[i + 3, j + 3] && board[i, j] == board[i + 4, j + 4])
                    {
                        return true;
                    }
                }
            }

            // Check for diagonal win (bottom-left to top-right)
            for (int i = 4; i < 15; i++)
            {
                for (int j = 0; j <= 10; j++)
                {
                    if (board[i, j] != '.' && board[i, j] == board[i - 1, j + 1] && board[i, j] == board[i - 2, j + 2] &&
                        board[i, j] == board[i - 3, j + 3] && board[i, j] == board[i - 4, j + 4])
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public override List<string[]> GetBoardState()
        {
            List<string[]> state = new List<string[]>();

            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if (board[i, j] != '.')
                    {
                        string[] coords = { board[i, j].ToString(), i.ToString(), j.ToString() };
                        state.Add(coords);
                    }
                }
            }

            return state;
        }

        public override void CreateBoardFromSave(List<string[]> boardSaveState, Player currentPlayerFromSave)
        {
            board = new char[15, 15];
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    board[i, j] = '.';
                }
            }

            for (int i = 0; i < boardSaveState.Count; i++)
            {
                int x = int.Parse(boardSaveState[i][1]);
                int y = int.Parse(boardSaveState[i][2]);
                char marker = boardSaveState[i][0][0];
                board[x, y] = marker;
            }

            currentPlayer = currentPlayerFromSave;
            SaveState(board, currentPlayer); ;
        }

        public override string GetGameName()
        {
            return "Gomoku";
        }

        protected override void RestoreState(GameState state)
        {
            board = state.Board;
            currentPlayer = state.CurrentPlayer;
        }

        protected override char[,] GetBoard()
        {
            return board;
        }

        protected override Player GetCurrentPlayer()
        {
            return currentPlayer;
        }


    }
}
