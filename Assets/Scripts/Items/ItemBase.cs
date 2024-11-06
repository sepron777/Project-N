using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : MonoBehaviour, IInteractable , IAnimation
{
    public string objectName;
    public int weight;
    public enum Animation {
    Arm,
    Arms,
    Base
    }

    public Animation AnimationLayerInProgress;

    public string Name { get => AnimationLayerInProgress.ToString();}

    // Start is called before the first frame update
    public virtual void Start()
    {
        Debug.Log(Name);
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }

    public virtual void Grab(Inventory inventory, Transform PickUpSpot)
    {

    }

    public virtual void Drop(Transform PickUpSpot, Inventory inventory)
    {

        
    }

    public virtual void Interact(GameObject Player)
    {
        Debug.Log(gameObject.name + " interact" + Player.name);
        PlayerMovement pl = Player.GetComponent<PlayerMovement>();
        Grab(pl.inventory, pl.PickUpSpot);
    }

    public virtual void Use()
    {
        throw new System.NotImplementedException();
    }
}
