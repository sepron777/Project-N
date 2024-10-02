using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class BoxItem : ItemBase, IInteractable
{
    public void Interact()
    {
        Debug.Log(gameObject.name + " interact");
    }

    public void Interact(GameObject Player)
    {
        Debug.Log(gameObject.name + " interact" + Player.name);
        PlayerMovement pl = Player.GetComponent<PlayerMovement>();
        Grab(pl.inventory, pl.PickUpSpot);
    }

    public void Grab(Inventory inventory, Transform PickUpSpot)
    {
        if (inventory.Item != null) return;
        Destroy(GetComponent<Rigidbody>());
        inventory.Item = this.gameObject;
        GetComponent<BoxCollider>().enabled = false;
        transform.SetParent(PickUpSpot.transform);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }
    public void Use()
    {
        throw new System.NotImplementedException();
    }

    public void Drop(Transform PickUpSpot,Inventory inventory)
    {
        RaycastHit hit;
        bool can = Physics.Raycast(PickUpSpot.position, PickUpSpot.up * -1, out hit, 2);
        if (can)
        {
            inventory.GetHoldingItem().transform.SetParent(null);
            inventory.GetHoldingItem().GetComponent<BoxCollider>().enabled = true;
            inventory.GetHoldingItem().AddComponent<Rigidbody>();
            inventory.SetItem(null);
            Debug.Log("ide");
        }
    }
}
