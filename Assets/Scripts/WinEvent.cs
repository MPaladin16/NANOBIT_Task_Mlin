using System;

public class WinEvent : EventArgs
{
    
    public int winner { get; set; }

    public WinEvent(int _winner) {
        winner = _winner;
    }
}
