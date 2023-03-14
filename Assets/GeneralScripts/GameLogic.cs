using System;
using UnityEngine;

public class GameLogic : MySingleton<GameLogic>
{
    public event Action OnResetLevel = null; 
    [SerializeField] Player playerRef = null;

    public Player GetPlayer => playerRef;

    public void ResetLevel()
    {
        OnResetLevel?.Invoke();
    }
}
