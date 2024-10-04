using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class BoxItem : ItemBase
{

    public override void Grab(Inventory inventory, Transform PickUpSpot)
    {
        base.Grab(inventory, PickUpSpot);
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

    public override void Drop(Transform PickUpSpot, Inventory inventory)
    {
        base.Drop(PickUpSpot, inventory);
        bool can = Physics.Raycast(PickUpSpot.position, PickUpSpot.up * -1, 2);
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
