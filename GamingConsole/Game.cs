
using System;

namespace GameConsole
{
    public abstract class Game
    {
        protected GameManagement gameManagement = new GameManagement();
        
        public abstract void GameIntroduction();
        public abstract void Initialize();
        public abstract bool IsGameOver();
        public abstract void DisplayBoard();
        public abstract void MakeMove(Player player);
        public abstract void MakeComputerMove(Player computerPlayer);
        public abstract Player GetWinner();
        public abstract Player GetLoser();

        public abstract List<string[]> GetBoardState();
        public abstract void CreateBoardFromSave(List<string[]> boardSaveState, Player currentPlayerFromSave);
        public abstract string GetGameName();

        public bool UndoMove()
        {
            GameState previousState = gameManagement.Undo();
            if (previousState != null)
            {
                RestoreState(previousState);
                return true;  // Undo was successful
            }
            else
            {
                Console.WriteLine("No moves to undo.");
                return false;  // No more moves to undo
            }
        }

        public bool RedoMove()
        {
            GameState nextState = gameManagement.Redo();
            if (nextState != null)
            {
                RestoreState(nextState);
                return true;
            }
            else
            {
                Console.WriteLine("No moves to redo.");
                return false;
            }
        }

        protected abstract void RestoreState(GameState state);

        protected void SaveState(char[,] board, Player currentPlayer)
        {
            GameState state = new GameState(board, currentPlayer);
            gameManagement.SaveState(state);
        }



        protected abstract char[,] GetBoard();  // Method to get the current board state
        protected abstract Player GetCurrentPlayer();  // Method to get the current player

    }
}
