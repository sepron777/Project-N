using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject ItemHolder;
    public GameObject Item;
    public void Drop(Transform PickUpSpot)
    {
        RaycastHit hit;
        bool can =Physics.Raycast(PickUpSpot.position,PickUpSpot.up*-1,out hit,2);
        if (can)
        {
            Item.transform.SetParent(null);
            Item.GetComponent<BoxCollider>().enabled = true;
            Item.AddComponent<Rigidbody>();
            //Item.transform.position = hit.point;
            Item = null;
            Debug.Log("ide");
        }
        
    }
}
