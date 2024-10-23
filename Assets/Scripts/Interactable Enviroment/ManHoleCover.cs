using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class ManHoleCover : MonoBehaviour
{
    public Transform Point;
    private PlayerMovement player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact(GameObject Player)
    {
        player = Player.GetComponent<PlayerMovement>();
        player.SetMovement(false);
        StartCoroutine(GoToPoint());
    }

    IEnumerator GoToPoint()
    {
        while (Vector3.Distance(Point.position,player.transform.position) > 0.9f)
        {
            Vector3 moveDirection = new Vector3(Point.position.x - player.transform.position.x, player.characterController.transform.position.y, Point.position.z - player.transform.position.z).normalized;
            moveDirection.x *= 3;
            moveDirection.z *= 3;
            player.SetDirectionVisual(new Vector3(moveDirection.x,0,moveDirection.z));
            player.characterController.Move(moveDirection * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }
        player.SetMovement(true);
    }
}
