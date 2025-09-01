using System;

public interface IOptions
{
    public event Action<int> NotifyDifficulty;
    public event Action<bool> NotifyRemoveSolveNumbers;
    public event Action<bool> NotifySoundState;
}
