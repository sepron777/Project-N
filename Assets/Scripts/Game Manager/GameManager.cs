using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public PlayerMovement Player;
    public CinemachineVirtualCamera VirtualCamera;
    public GameObject menu;
    public RawImage rawImage;
    public Transform startPos;
    public static GameManager instance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
        Player.SetMovement(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            TeleportToStartPos();
        }
    }

    public void TeleportToStartPos()
    {
        Player.GetComponent<CharacterController>().enabled = false;
        Player.transform.position = startPos.position;
        Player.GetComponent<CharacterController>().enabled = true;
    }

    [ContextMenu("startGame")]
    public void starFadeOu()
    {
        StartCoroutine(StartFadeOutCor());
    }

    IEnumerator StartFadeOutCor()
    {
        menu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
