using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject ItemHolder;
    public GameObject Item;
    public Transform PickUpSpot;
    private Animator animator;
    private bool canInteract =true;
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

    public void SetItem(GameObject gm,bool withAnimation)
    {
        if (!canInteract) return;
        canInteract = false;
        Item = gm;
        if(Item != null && withAnimation)
        {
            //animator.SetLayerWeight(animator.GetLayerIndex("Arms"), 0.9f);
            //StopAllCoroutines();
            StartCoroutine(Animation(0.9f));
        }
        else
        {
            //animator.SetLayerWeight(animator.GetLayerIndex("Arms"), 0f);
            //StopAllCoroutines();
            StartCoroutine(Animation(0));
        }
    }

    IEnumerator Animation(float Weight)
    {
        float diffrance = (animator.GetLayerWeight(animator.GetLayerIndex("Arms")) < Weight ? 0.1f : -0.1f);
        Debug.Log(diffrance);
        while (diffrance== 0.1f? animator.GetLayerWeight(animator.GetLayerIndex("Arms"))<= Weight: animator.GetLayerWeight(animator.GetLayerIndex("Arms")) >= Weight)
        {
            animator.SetLayerWeight(animator.GetLayerIndex("Arms"), animator.GetLayerWeight(animator.GetLayerIndex("Arms"))+diffrance);
            yield return new WaitForFixedUpdate();
        }
        Debug.Log("finish");
        canInteract = true;
    }

    public bool IsInventoryFull()
    {
        return Item == null?false:true;
    }
}
