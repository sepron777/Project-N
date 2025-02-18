using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Ladder : MonoBehaviour
{
    [RequiredField]
    public Transform endPoint;

    public void interacted(GameObject Player)
    {
        StartCoroutine(StartFadeOutCor());
    }

    IEnumerator StartFadeOutCor()
    {
        GameManager.instance.Player.SetMovement(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        while (GameManager.instance.rawImage.color.a < 1)
        {
            GameManager.instance.rawImage.color = new Color(GameManager.instance.rawImage.color.r, GameManager.instance.rawImage.color.g, GameManager.instance.rawImage.color.b, GameManager.instance.rawImage.color.a + 0.1f);
            yield return new WaitForSeconds(0.1f);
        }
        TeleportPlyer();
    }
    public void TeleportPlyer()
    {
        GameManager.instance.Player.GetComponent<CharacterController>().enabled = false;
        GameManager.instance.Player.transform.position = endPoint.position;
        GameManager.instance.Player.GetComponent<CharacterController>().enabled = true;
        GameManager.instance.Player.SetMovement(true);
        StartCoroutine(StartFadeInCor());
    }

    IEnumerator StartFadeInCor()
    {
        yield return new WaitForSeconds(0.5f);
        while (GameManager.instance.rawImage.color.a > 0)
        {
            GameManager.instance.rawImage.color = new Color(GameManager.instance.rawImage.color.r, GameManager.instance.rawImage.color.g, GameManager.instance.rawImage.color.b, GameManager.instance.rawImage.color.a - 0.1f);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
