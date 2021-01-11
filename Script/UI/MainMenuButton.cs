using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuButton : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerDownHandler,IPointerUpHandler
{
    [Header("Sound")]
    public AudioClip SelectSound;
    public AudioClip PressSound;

    [Header("UI")]
    public GameObject ControlUI;
    public static GameObject DisabledUI;

    private Animator animator;
    private AudioSource audioSource;
    private GameObject InstUI;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        animator.keepAnimatorControllerStateOnDisable = true;
        audioSource = GetComponent<AudioSource>();

        DisabledUI = GameObject.FindGameObjectWithTag("MainCanvas");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        animator.SetBool("IsSelect", true);
        audioSource.PlayOneShot(SelectSound, 0.5f);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        animator.SetBool("IsSelect", false);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        animator.SetBool("IsPressDown", true);
        audioSource.PlayOneShot(PressSound, 0.5f);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        animator.SetBool("IsPressDown", false);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    /*public void CreateControlUI()
    {
        audioSource.PlayOneShot(PressSound, 0.5f);

        InstUI = Instantiate(ControlUI);

        DisabledUI.SetActive(false);       
    } 
    public void GoToPreviousUI()
    {
        AudioSource.PlayClipAtPoint(PressSound, Vector3.zero);

        for (int i = 0; i < DisabledUI.GetComponentsInChildren<MainMenuButton>().Length; i++)
        {
            if (DisabledUI.GetComponentsInChildren<MainMenuButton>()[i].InstUI)
            {
                Destroy(DisabledUI.GetComponentsInChildren<MainMenuButton>()[i].InstUI);

                DisabledUI.SetActive(true);
                break;
            }
        }
        for(int j = 0;j< DisabledUI.GetComponentsInChildren<Button>().Length;j++)
        {
            DisabledUI.GetComponentsInChildren<Button>()[j].transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
            Debug.Log(DisabledUI.GetComponentsInChildren<Button>()[j].transform.GetChild(0).GetComponent<RectTransform>().gameObject.name);
        }
    }*/
}
