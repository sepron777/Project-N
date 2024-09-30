using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : MonoBehaviour,IInteractable
{
    public void Interact()
    {
        Debug.Log(gameObject.name +" interact");
    }

    public void Interact(GameObject Player)
    {
        Debug.Log(gameObject.name + " interact" + Player.name);
        PlayerMovement pl = Player.GetComponent<PlayerMovement>();
        Grab(pl.inventory,pl.PickUpSpot);
    }

    public void Grab(Inventory inventory, Transform PickUpSpot)
    {
        if (inventory.Item !=null) return;
        Destroy(GetComponent<Rigidbody>());
        inventory.Item = this.gameObject;
        GetComponent<BoxCollider>().enabled = false;
        transform.SetParent(PickUpSpot.transform);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Use()
    {
        throw new System.NotImplementedException();
    }
}
