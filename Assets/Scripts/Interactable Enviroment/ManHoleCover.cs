using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ManHoleCover : MonoBehaviour,IInteractable
{
    public Transform Point;
    private PlayerMovement plyer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Interact(GameObject Player)
    {
        plyer = Player.GetComponent<PlayerMovement>();
        plyer.SetMovement(false);
        StartCoroutine(idk());
    }

    IEnumerator idk()
    {
        plyer.NavMeshAgent.enabled = true;
        plyer.NavMeshAgent.isStopped = false;
        plyer.NavMeshAgent.SetDestination(Point.position);
        while (plyer.NavMeshAgent.remainingDistance > plyer.NavMeshAgent.stoppingDistance)
        {
            yield return new WaitForFixedUpdate();
        }
        plyer.NavMeshAgent.isStopped =true;
        plyer.SetMovement(true);
        plyer.NavMeshAgent.enabled = false;
    }

    public virtual void Use()
    {

    }
}
