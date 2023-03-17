using System;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayer : MonoBehaviour
{
    [SerializeField] Slider healthSlider;
    Player playerRef = null;
    void Start()
    {
        if (!playerRef)
        {
            Debug.Log("no player");
            playerRef = GameLogic.Instance.GetPlayer;
        }
        playerRef.HealthSystem.OnHealthChanged += HealthChanged;
    }

    void HealthChanged(float _current, float _max)
    {
        healthSlider.value = _current / _max;
    }
}
