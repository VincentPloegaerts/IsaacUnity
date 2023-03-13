using System;
using UnityEngine;

public class Door : MonoBehaviour
{
    public event Action OnDoorCollided = null; //tried to do trigger box to switch camera state but ended up using one inside the room

    void OnTriggerEnter2D(Collider2D col)
    {
        OnDoorCollided?.Invoke();
    }
}
