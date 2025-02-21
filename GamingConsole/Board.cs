using System;

public class Board
{
    private char[,] grid;
    private int rows, cols;

    public Board(int rows, int cols)
    {
        this.rows = rows;
        this.cols = cols;
        grid = new char[rows, cols];
    }

    public bool placePiece(int row, int col, char piece)
    {
        if (grid[row, col] == '\0')
        {
            grid[row, col] = piece;
            return true;
        }
        return false;
    }

    public bool checkThreeInRow(char piece)
    {
        for (int i = 0; i < rows; i++)
        {
            if (grid[i, 0] == piece && grid[i, 1] == piece && grid[i, 2] == piece)
                return true;
        }
        for (int i = 0; i < cols; i++)
        {
            if (grid[0, i] == piece && grid[1, i] == piece && grid[2, i] == piece)
                return true;
        }
        if (grid[0, 0] == piece && grid[1, 1] == piece && grid[2, 2] == piece)
            return true;
        if (grid[0, 2] == piece && grid[1, 1] == piece && grid[2, 0] == piece)
            return true;

        return false;
    }
}