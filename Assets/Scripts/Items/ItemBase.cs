using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : MonoBehaviour, IInteractable
{
    public string objectName;
    public int weight;
    // Start is called before the first frame update
    public virtual void Start()
    {
        
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }

    public virtual void Grab(Inventory inventory, Transform PickUpSpot)
    {
        if (inventory.Item != null) return;
        Destroy(GetComponent<Rigidbody>());
        inventory.Item = this.gameObject;
        GetComponent<BoxCollider>().enabled = false;
        transform.SetParent(PickUpSpot.transform);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public virtual void Drop(Transform PickUpSpot, Inventory inventory)
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

    public virtual void Interact()
    {
        throw new System.NotImplementedException();
    }

    public virtual void Interact(GameObject Player)
    {
        Debug.Log(gameObject.name + " interact" + Player.name);
        PlayerMovement pl = Player.GetComponent<PlayerMovement>();
        Grab(pl.inventory, pl.PickUpSpot);
    }

    public virtual void Use()
    {
        throw new System.NotImplementedException();
    }
}
