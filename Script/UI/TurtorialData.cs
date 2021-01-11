using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurtorialData : MonoBehaviour
{
    public float ReverseTime;
    public AudioClip ShowSound;

    private Animator animator;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(ShowSound, 0.4f);

        animator = GetComponent<Animator>();
        Invoke("SetReverseAnim", ReverseTime);
    }
    private void SetReverseAnim()
    {
        animator.SetBool("Reverse", true);
    }
}
