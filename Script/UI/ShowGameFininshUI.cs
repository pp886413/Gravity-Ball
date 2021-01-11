using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowGameFininshUI : MonoBehaviour
{
    public GameObject GameFininshUI;
    public GameObject ShootUI;
    public AudioClip OpenSound;

    private AudioSource audioSource;
    private bool IsEnter = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsEnter)
        {
            if (other.transform.tag == "Player")
            {
                Instantiate(GameFininshUI);
                ShootUI.SetActive(false);

                audioSource.PlayOneShot(OpenSound, 0.5f);

                PlayerController.IsGamePause = true;

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                IsEnter = true;
            }
        }
    }
}
