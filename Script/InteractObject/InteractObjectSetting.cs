using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractObjectSetting : MonoBehaviour
{
    public AudioClip DropSound;

    private Rigidbody rb;
    private AudioSource audioSource;

    private bool FirstDrop = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.transform.tag)
        {
            case "Laser":
                rb.freezeRotation = true;
                break;
            case "Platform":
                if (collision.collider.GetType() == typeof(BoxCollider))
                {
                    this.gameObject.transform.SetParent(collision.transform);
                }
                audioSource.PlayOneShot(DropSound, 0.1f);
                break;
            case "Ground":
                if (!FirstDrop)
                {
                    audioSource.PlayOneShot(DropSound, 0.1f);
                }
                break;
            default:
                FirstDrop = false;
                break;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        switch (collision.transform.tag)
        {
            case "Laser":
                rb.freezeRotation = false;
                break;
            case "Platform":
                this.gameObject.transform.parent = null;
                break;
        }
    }
}
