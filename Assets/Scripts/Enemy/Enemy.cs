using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent meshAgent;
    public Transform player;
    public Transform Wheel;
    public float coneAngle;
    public float coneRange;
    public LayerMask layerMask;

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
        ChenckUpdate(StacionaryState, chaseState, SeePlayer());
        ChenckUpdate(chaseState, StacionaryState, !SeePlayer());

    }

    private void FixedUpdate()
    {
        Wheel.Rotate(meshAgent.velocity.magnitude, 0, 0,Space.Self);
    }

    public void ChenckUpdate(EnemyBase From, EnemyBase To, bool CanGo)
    {
        if (From != m_EnemyBase)  return; 
        if (CanGo)
        {
            m_EnemyBase.OnExit();
            m_EnemyBase = To;
            m_EnemyBase.OnEnter();
        }
    }

    private bool SeePlayer()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        float angleY = Vector3.SignedAngle(transform.transform.forward, direction, Vector3.up);
        float angleZ = Vector3.SignedAngle(transform.transform.forward, direction, Vector3.right);
        if ((angleY < coneAngle / 2 && angleY > -coneAngle / 2) && (angleZ < coneAngle / 2 && angleZ > -coneAngle / 2))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, direction, out hit, coneRange, layerMask))
            {
                //only works for player detecion for now idk if it works for enemies
                if (hit.collider.gameObject == player.transform.gameObject)
                {
                    Debug.DrawRay(transform.transform.position, direction * coneRange, Color.green);
                    return true;
                }
            }
            return false;
        }
        return false;
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