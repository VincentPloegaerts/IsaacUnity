using System;
using UnityEngine;


[RequireComponent(typeof(HealthSystem))]
public class Enemy : MonoBehaviour
{
    protected Player playerRef = null;
    [SerializeField] protected bool isActive;
    public event Action OnDeathEvent; 
    [SerializeField] protected float moveSpeed = 1.0f;
    protected float damage = 1.0f;
    [SerializeField]HealthSystem healthSystem = null;

    protected virtual void Awake()
    {
        if (!healthSystem) healthSystem = GetComponent<HealthSystem>();
    }

    protected virtual void Start()
    {
        playerRef = GameLogic.Instance.GetPlayer;
    }

    public virtual void Activate()
    {
        isActive = true;
    }
    public virtual void Deactivate()
    {
        isActive = false;
    }

    protected void LerpToPos(Vector3 _pos)
    {
        Vector3 _dir = _pos - transform.position;
        _dir.Normalize();
        transform.position += moveSpeed * Time.deltaTime * _dir;
    }
    
    protected void LerpToPosLocal(Vector3 _pos)
    {
        Vector3 _currentPos = transform.position;
        Vector3 _dir = _pos - _currentPos;
        _dir.Normalize();
        transform.localPosition += moveSpeed * Time.deltaTime * _dir;
    }
}
