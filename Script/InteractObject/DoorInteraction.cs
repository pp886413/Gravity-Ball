using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    public float OpenSpeed;
    public float MoveDistance;

    public AudioClip OpenDoorSound;
    public AudioClip CloseDoorSound;

    private AudioSource audioSource;

    private Vector3 BasePosition = new Vector3();
    private Vector3 MovedPosition = new Vector3();
    private float InteractionLerpValue = 0.0f;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        BasePosition = transform.position;
        MovedPosition = new Vector3(transform.position.x, transform.position.y + MoveDistance, transform.position.z);
    }

    public void OpenDoor()
    {
        audioSource.PlayOneShot(OpenDoorSound, 0.5f);
        StartCoroutine(OpenDoorTimer());
    }
    public void DelayOpenDoor()
    {
        Invoke("OpenDoor", 1.0f);
    }
    public void CloseDoor()
    {
        audioSource.PlayOneShot(CloseDoorSound, 0.5f);
        StartCoroutine(CloseDoorTimer());
    }

    private IEnumerator OpenDoorTimer()
    {
        while(InteractionLerpValue < 1.0f)
        {
            InteractionLerpValue += Time.deltaTime * OpenSpeed;
            transform.position = Vector3.Lerp(BasePosition, MovedPosition, InteractionLerpValue);
            
            yield return null;
        }
    }
    private IEnumerator CloseDoorTimer()
    {
        while (InteractionLerpValue > 0.0f)
        {
            InteractionLerpValue -= Time.deltaTime * OpenSpeed;
            transform.position = Vector3.Lerp(BasePosition, MovedPosition, InteractionLerpValue);

            yield return null;
        }
    }
}
