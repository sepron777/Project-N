using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using static ItemBase;
using static UnityEngine.Rendering.DebugUI;

public class ManHoleCover : MonoBehaviour
{
    public Transform Point;
    public Transform cover;
    private PlayerMovement player;

    public void Interact(GameObject Player)
    {
        player = Player.GetComponent<PlayerMovement>();
        player.SetMovement(false);
        StartCoroutine(GoToPoint());
    }

    IEnumerator GoToPoint()
    {
        float trunSmoothVeleocity = 0;
        Vector3 dir = new Vector3(Point.position.x - player.transform.position.x, 0, Point.position.z - player.transform.position.z).normalized;
        while (Vector3.Angle(player.Visual.transform.forward, dir) > 1.0f)
        {
            float tar = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + player.GetCamera().transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(player.Visual.transform.eulerAngles.y, tar, ref trunSmoothVeleocity, player.smoothTime);
            player.Visual.transform.rotation = Quaternion.Euler(0, angle, 0);
            yield return new WaitForFixedUpdate();
        }

        while (Vector3.Distance(Point.position, player.transform.position) > 0.9f)
        {
            Vector3 moveDirection = new Vector3(Point.position.x - player.transform.position.x, player.characterController.transform.position.y, Point.position.z - player.transform.position.z).normalized;
            moveDirection.x *= 3;
            moveDirection.z *= 3;
            player.SetDirectionVisual(new Vector3(moveDirection.x, 0, moveDirection.z));
            SetAnimatorMovemnt2D(0.5f,0.3f);
            player.characterController.Move(moveDirection * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }
        player.animator.SetFloat("Value", 0);

        dir = Point.forward.normalized;
        while (Vector3.Angle(player.Visual.transform.forward, dir) > 1.0f)
        {
            float tar = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + player.GetCamera().transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(player.Visual.transform.eulerAngles.y, tar, ref trunSmoothVeleocity, player.smoothTime);
            player.Visual.transform.rotation = Quaternion.Euler(0, angle, 0);
            Debug.Log(dir.magnitude);
            yield return new WaitForFixedUpdate();
        }

        float y = cover.position.y + 1;
        while (cover.position.y<y)
        {
            cover.position = new Vector3(cover.position.x, cover.position.y+0.01f, cover.position.z);
            yield return new WaitForFixedUpdate();
        }

        player.SetMovement(true);
    }

    private void SetAnimatorMovemnt2D(float Value, float DampTime)
    {
        player.animator.SetFloat("Value", Value, DampTime, Time.deltaTime);
    }
}
