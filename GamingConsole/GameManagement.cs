
using System.Collections;
using System.Collections.Generic;

namespace GameConsole
{
    public class GameManagement
    {
        private Stack<GameState> undoStack = new Stack<GameState>();
        private Stack<GameState> redoStack = new Stack<GameState>();
        private GameState currentState; 

        public void SaveState(GameState state)
        {
            currentState = state;
            undoStack.Push(state);
            redoStack.Clear();
        }

        public GameState Undo()
        {
            if (undoStack.Count > 1)
            {
                redoStack.Push(undoStack.Pop());
                currentState = undoStack.Peek();
                Console.WriteLine("Move undone");
                return currentState; 
            }
            else 
            {
                Console.WriteLine("No moves to undo.");
                return null;
            }
            
        }

        public GameState Redo()
        {
            if (redoStack.Count > 0)
            {
                GameState state = redoStack.Pop();
                undoStack.Push(state);
                currentState = state;
                Console.WriteLine("Move redone");
                return state;
            }
            Console.WriteLine("No moves to redo.");
            return null;
        }

        public GameState GetCurrentState()
        {
            return currentState; 
        }
    }

    public class GameState
    {
        public char[,] Board { get; set; }
        public Player CurrentPlayer { get; set; }

        public GameState(char[,] board, Player currentPlayer)
        {
            Board = (char[,])board.Clone();
            CurrentPlayer = currentPlayer;
        }
    }
}
