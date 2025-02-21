using System;

namespace GameConsole;

public class Player
{
    public string PlayerName { get; }
    public char Marker { get; }

    public Player(string playerName, char marker)
    {
        PlayerName = playerName;
        Marker = marker;
    }
}