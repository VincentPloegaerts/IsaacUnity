using System.Collections.Generic;
using UnityEngine;

public class SwarmFlyCenter : Enemy
{
    List<SwarmFly> swarmFollowers = new List<SwarmFly>();
    [SerializeField] AngleData angleData = new AngleData();

    void Update()
    {
        if (!isActive) return;
        LerpToPos(playerRef.transform.position);
    }

    public void AddFly(SwarmFly _fly)
    {
        swarmFollowers.Add(_fly);
        _fly.Init(angleData);
        _fly.OnDeathEvent += SetFliesAngles;
    }

    void SetFliesAngles()
    {
        int _max = swarmFollowers.Count;
        if (_max ==0)return;
        float _angle = 360.0f / _max;
        for (int i = 0; i < _max; i++)
        {
            swarmFollowers[i].SetAngle(_angle * i);
        }
    }

    public override void Activate()
    {
        base.Activate();
        foreach (SwarmFly _fly in swarmFollowers)
        {
            _fly.Activate();
        }
        SetFliesAngles();
    }
    public override void Deactivate()
    {
        base.Deactivate();
        foreach (SwarmFly _fly in swarmFollowers)
        {
            _fly.Deactivate();
        }
    }

    public void RemoveFly(SwarmFly _fly)
    {
        swarmFollowers.Remove(_fly);
    }
}