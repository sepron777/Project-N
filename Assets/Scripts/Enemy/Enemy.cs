using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent meshAgent;
    public Transform player;


    private EnemyBase m_EnemyBase;
    private ChaseState chaseState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        chaseState = new ChaseState(transform, meshAgent, player);
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