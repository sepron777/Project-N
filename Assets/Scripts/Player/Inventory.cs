using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject ItemHolder;
    public GameObject Item;
    public Transform PickUpSpot;
    public void Drop(Transform PickUpSpot)
    {
        Item.GetComponent<ItemBase>().Drop(PickUpSpot,this);
    }

    public GameObject GetHoldingItem()
    {
        return Item;
    }
    public void SetItem(GameObject gm)
    {
        Item = gm;
    }

    public bool IsInventoryFull()
    {
        return Item == null?false:true;
    }
}
