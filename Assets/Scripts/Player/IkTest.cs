using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IkTest : MonoBehaviour
{
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnAnimatorIK(int layerIndex)
    {
        Vector3 pos = animator.GetBoneTransform(HumanBodyBones.LeftFoot).position;
        Debug.Log("sedfsd");
        animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot,0.6f);
        animator.SetIKPosition(AvatarIKGoal.LeftFoot,new Vector3(pos.x, pos.y-.2f, pos.z));
    }
}
