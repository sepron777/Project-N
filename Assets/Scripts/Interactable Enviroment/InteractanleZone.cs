using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractanleZone : MonoBehaviour,IInteractable
{
    public UnityEvent<GameObject> Interacted;

    public virtual void Interact(GameObject Player)
    {
        Interacted.Invoke(Player);
    }

    public virtual void Use()
    {

    }

}
