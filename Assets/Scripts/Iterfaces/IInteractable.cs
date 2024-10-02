using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public void Interact();
    public void Interact(GameObject Player);
    public void Use();
    public void Grab(Inventory inventory, Transform PickUpSpot);
    public void Drop(Transform PickUpSpot,Inventory inv);
}
