using System;
using UnityEngine;

public class Stairs : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        GameLogic.Instance.ResetLevel();
    }
}
