using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowVideoUI : MonoBehaviour
{
    public GameObject VideoObject;
    public GameObject MainUI;
    public AudioClip ClickSound;
    public int[] VideoIndex;
    public int VideoIndexQuantity;
    public bool ShowComfirmButton;

    private GameObject VideoUI;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.tag == "Player")
        {
            VideoUI = Instantiate(VideoObject);
            VideoUI.transform.SetParent(MainUI.transform);
            
            VideoUI.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
            VideoUI.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f,0.5f);
            VideoUI.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);

            Button[] Buttons = VideoUI.GetComponentsInChildren<Button>();

            for (int i = 0; i < Buttons.Length; i++)
            {
                if (Buttons[i].transform.tag == "Confirm")
                {
                    Buttons[i].onClick.AddListener(RemoveUI);
                    if (!ShowComfirmButton)
                    { 
                       Buttons[i].gameObject.SetActive(false);
                    }
                    break;
                }
            }

            /** Pause game */
            Time.timeScale = 0.0f;
            PlayerController.IsGamePause = true;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    public void RemoveUI()
    {
        if (VideoUI.GetComponent<Animator>())
        {
            audioSource.PlayOneShot(ClickSound, 0.3f);

            /** Set anim state */
            VideoUI.GetComponent<Animator>().SetBool("Fininsh", true);

            /** Disable cursor visible */
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            /** Return to normal game time speed */
            Time.timeScale = 1.0f;
            PlayerController.IsGamePause = false;


            ChangeVideo changedVideo = FindObjectOfType<ChangeVideo>();
           
            Destroy(changedVideo.GetInstVideoPlayer(), 1.5f);
            Destroy(VideoUI, 1.5f);
            Destroy(this.gameObject,0.2f);
        }
    }
}
