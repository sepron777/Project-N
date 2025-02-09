using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbingRope : MonoBehaviour,IInteractable
{
    public Transform bottom;
    public Transform top;
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
        PlayerMovement pl = Player.GetComponent<PlayerMovement>();
       if(pl.inventory.IsInventoryFull())return;
        pl.inventory.SetItem(this.gameObject,false);
        pl.SetRope(true);
    }
}
