using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent meshAgent;
    public Transform player;

    public List<Transform> Waypoints;

    private EnemyBase m_EnemyBase;
    private StacionaryState StacionaryState;
    private ChaseState chaseState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        chaseState = new ChaseState(transform, meshAgent, player);
        StacionaryState = new StacionaryState(transform, meshAgent, Waypoints);
        m_EnemyBase = StacionaryState;
        m_EnemyBase.OnEnter();
    }

    void Update()
    {
        m_EnemyBase.Update();
    }

    public void ChenckUpdate(EnemyBase From, EnemyBase To, bool CanGo)
    {
        if (From != m_EnemyBase) { return; }
        if (CanGo)
        {
            m_EnemyBase.OnExit();
            m_EnemyBase = To;
            m_EnemyBase.OnEnter();
        }
    }
}


public class EnemyBase
{
    protected Transform Enemy;
    protected NavMeshAgent meshAgent;

    public virtual void OnEnter()
    {

    }

    public virtual void Update()
    {

    }

    public virtual void OnExit()
    {

    }
}

public class ChaseState: EnemyBase
{
    private Transform Player;
    public ChaseState(Transform enemy, NavMeshAgent navMeshAgent, Transform player)
    {
        this.Enemy = enemy;
        this.meshAgent = navMeshAgent;
        Player = player;
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void Update()
    {
        base.Update();
        meshAgent.SetDestination(Player.position);
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}

public class StacionaryState : EnemyBase
{
    private List<Transform> Waypoints;
    private int index = 0;

    public StacionaryState(Transform enemy, NavMeshAgent navMeshAgent, List<Transform> Waypoints)
    {
        this.Enemy = enemy;
        this.meshAgent = navMeshAgent;
        this.Waypoints = Waypoints;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        meshAgent.SetDestination(Waypoints[index].position);
    }

    public override void Update()
    {
        base.Update();
        if (meshAgent.remainingDistance < meshAgent.stoppingDistance)
        {
            AddIndex();
        }
    }

    private void AddIndex()
    {
        if(index == Waypoints.Count-1)
        {
            index= 0;
            meshAgent.SetDestination(Waypoints[index].position);
            return;
        }
         index++;
        meshAgent.SetDestination(Waypoints[index].position);
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}