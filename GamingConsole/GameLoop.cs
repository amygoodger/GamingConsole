
using System;

namespace GameConsole
{
    public class GameLoop
    {
        private Game _game;
        public Player Player1 { get; private set; }
        public Player Player2 { get; private set; }
        public bool IsComputerOpponent { get; private set; }

        public GameLoop(Game game, bool isComputerOpponent)
        {
            _game = game;
            IsComputerOpponent = isComputerOpponent;

            Player1 = new Player("Player 1", 'X');

            if (IsComputerOpponent)
            {
                Player2 = new Player("Computer", 'O');
            }
            else
            {
                Player2 = new Player("Player 2", 'O');
            }
        }

        public Player StartGame()
        {
            Console.Clear();
            _game.GameIntroduction();
            _game.Initialize();

            Player currentPlayer = Player1;
            while (!_game.IsGameOver())
            {
                Console.Clear();
                _game.DisplayBoard();
                Console.WriteLine($"\n{currentPlayer.PlayerName}'s turn:");

                if (currentPlayer == Player2 && IsComputerOpponent)
                {
                    _game.MakeComputerMove(Player2);
                }
                else
                {
                    _game.MakeMove(currentPlayer);
                }

                if (currentPlayer == Player1)
                {
                    currentPlayer = Player2;
                }
                else
                {
                    currentPlayer = Player1;
                }
            }

            return _game.GetWinner();
        }
    }
}
