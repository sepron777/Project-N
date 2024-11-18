using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    private PlayerMovement PlayerMovement;

    private void Awake()
    {
        PlayerMovement = transform.root.GetComponent<PlayerMovement>();
    }

    public void ChangeMoveState()
    {
        PlayerMovement.SetMovement(!PlayerMovement.GetMovement());
    }

    public void EndAnimationevent()
    {
        PlayerMovement.EndAnimationevent();
    }
}
