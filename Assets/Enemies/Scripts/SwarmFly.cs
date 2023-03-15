using System;
using UnityEngine;

[System.Serializable]
public class AngleData
{
    [SerializeField] public float angle;
    [SerializeField] public float distance;
    [SerializeField] public float angleSpeed;

    public AngleData(AngleData _data)
    {
        angle = _data.angle;
        distance = _data.distance;
        angleSpeed = _data.angleSpeed;
    }
    
    public AngleData()
    {
    }
}

public class SwarmFly : Enemy
{
    [SerializeField] SwarmFlyCenter toFollow = null;
    AngleData angleData;
    public void Init(AngleData _data) => angleData = new AngleData(_data);
    public void SetAngle(float _angle) => angleData.angle = _angle;

    protected override void Awake()
    {
        base.Awake();
        toFollow.AddFly(this);
    }

    void Update()
    {
        if (!isActive) return;
        angleData.angle += Time.deltaTime * angleData.angleSpeed;
        Vector3 _pos =new Vector3(Mathf.Cos(Mathf.Deg2Rad * angleData.angle),
                                  Mathf.Sin(Mathf.Deg2Rad * angleData.angle),
                                  0.0f) * angleData.distance + toFollow.transform.position;
        LerpToPos(_pos);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject == GameLogic.Instance.GetPlayer.gameObject)
        {
            GameLogic.Instance.GetPlayer.HealthSystem.TakeDamage(damage);
        }
    }
}
