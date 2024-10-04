using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class BoxItem : ItemBase
{

    public override void Grab(Inventory inventory, Transform PickUpSpot)
    {
        base.Grab(inventory, PickUpSpot);
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
    }
}
