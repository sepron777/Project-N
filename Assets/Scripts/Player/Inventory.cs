using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Transform ItemHolder;
    public GameObject Item;
    public Transform PickUpSpot;
    public Animator animator;
    private bool canInteract =true;
    private string animatioLayer;
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void Drop()
    {
        if(Item.TryGetComponent<ItemBase>(out ItemBase item))
        {
            item.Drop(ItemHolder.transform, this);
        }
    }

    public GameObject GetHoldingItem()
    {
        return Item;
    }
    public Animator GetAnimator()
    {
        return animator;
    }

    public bool CanInteract()
    {
        return canInteract;
    }
    public void SetInteractSate(bool can)
    {
        canInteract = can;
    }

    public void SetItem(GameObject gm,bool withAnimation)
    {
        if (!canInteract) return;
        canInteract = false;
        Item = gm;
        if(Item != null)
        {
            //animator.SetLayerWeight(animator.GetLayerIndex("Arms"), 0.9f);
            //StopAllCoroutines();
            animatioLayer = gm.GetComponent<IAnimation>().Name;
            StartCoroutine(Animation(0.9f, withAnimation, animatioLayer));
        }
        else
        {
            //animator.SetLayerWeight(animator.GetLayerIndex("Arms"), 0f);
            //StopAllCoroutines();
            StartCoroutine(Animation(0, withAnimation, animatioLayer));
        }

    }

    IEnumerator Animation(float Weight,bool withAnimation,string AnimationLayer)
    {
        float diffrance = (animator.GetLayerWeight(animator.GetLayerIndex(AnimationLayer)) < Weight ? 0.1f : -0.1f);
        Debug.Log(diffrance);
        while (diffrance== 0.1f? animator.GetLayerWeight(animator.GetLayerIndex(AnimationLayer))<= Weight: animator.GetLayerWeight(animator.GetLayerIndex(AnimationLayer)) >= Weight && withAnimation)
        {
            animator.SetLayerWeight(animator.GetLayerIndex(AnimationLayer), animator.GetLayerWeight(animator.GetLayerIndex(AnimationLayer))+diffrance);
            for (int i = 1; i < animator.layerCount; i++)
            {
                if (animator.GetLayerName(i) != AnimationLayer)
                {
                    animator.SetLayerWeight(i, animator.GetLayerWeight(i)-(diffrance));
                }
            }
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
