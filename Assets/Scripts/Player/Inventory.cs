using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject ItemHolder;
    public GameObject Item;
    public Transform PickUpSpot;
    private Animator animator;
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void Drop(Transform PickUpSpot)
    {
        if(Item.TryGetComponent<ItemBase>(out ItemBase item))
        {
            item.Drop(PickUpSpot, this);
        }
    }

    public GameObject GetHoldingItem()
    {
        return Item;
    }

    public void SetItem(GameObject gm)
    {
        Item = gm;
        if(Item != null)
        {
            animator.SetLayerWeight(animator.GetLayerIndex("Arms"), 0.9f);
        }
        else
        {
            animator.SetLayerWeight(animator.GetLayerIndex("Arms"), 0f);
        }

    }

    public bool IsInventoryFull()
    {
        return Item == null?false:true;
    }
}
