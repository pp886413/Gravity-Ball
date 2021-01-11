using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public string SceneName;
    public float WaitTime;
    public static bool IsMainLevel = false;
    public AudioClip TransitionSound;

    private Animator animator;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(TransitionSound, 0.4f);

        animator = transform.GetComponentInChildren<Animator>();
        if (IsMainLevel)
        {
            animator.Play("TransitionOutAnim");
            IsMainLevel = false;
        }
        ChangeSceneTimer();
    }
    private void changeScene()
    {
        if (SceneName != "None")
        {
            SceneManager.LoadScene(SceneName);
        }
        Destroy(this.gameObject, WaitTime);
    }
    public void ChangeSceneTimer()
    {
        Invoke("changeScene", WaitTime);
    }
}
