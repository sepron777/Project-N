using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Interact : MonoBehaviour
{
    private bool Holding = false;
    public PlayerInput playerInput;
    private InputAction interact;
    // Start is called before the first frame update
    void Start()
    {
        interact = playerInput.actions.FindAction("Interact");
    }

    // Update is called once per frame
    void Update()
    {
        if(interact.IsPressed()) 
        {
            Debug.Log("pressed");
        }
    }
}
