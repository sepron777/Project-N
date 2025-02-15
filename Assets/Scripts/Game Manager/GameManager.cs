using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public PlayerMovement Player;
    public CinemachineVirtualCamera VirtualCamera;
    public RawImage rawImage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Player.SetMovement(false);
    }

    [ContextMenu("startGame")]
    public void starFadeOu()
    {
        StartCoroutine(StartFadeOutCor());
    }

    IEnumerator StartFadeOutCor()
    {
        while (rawImage.color.a < 1)
        {
            rawImage.color = new Color(rawImage.color.r, rawImage.color.g, rawImage.color.b, rawImage.color.a + 0.1f);
            yield return new WaitForSeconds(0.1f);
        }
        SetCameraToPlayer();
    }

    public void SetCameraToPlayer()
    {
        VirtualCamera.LookAt = Player.transform;
        VirtualCamera.Follow = Player.transform;
        Player.SetMovement(true);
        StartCoroutine(StartFadeInCor());
    }

    IEnumerator StartFadeInCor()
    {
        yield return new WaitForSeconds(0.5f);
        while (rawImage.color.a > 0)
        {
            rawImage.color = new Color(rawImage.color.r, rawImage.color.g, rawImage.color.b, rawImage.color.a - 0.1f);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
