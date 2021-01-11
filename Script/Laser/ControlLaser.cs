using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlLaser : MonoBehaviour
{
    [Header("Traced type")]
    public LayerMask TraceLayerMask;
    [Header("Sound")]
    public AudioClip DragSound;

    public float TraceMaxDistance;
    private Vector3 TraceStart = Vector3.zero;
    private Vector3 TraceEnd = Vector3.zero;

    public GameObject ObjectHolder;

    private GameObject TracedObject;
    private float MouseZcoord;
    private Vector3 MouseOffset;

    private PlayerController Player;
    private AudioSource audioSource;

    public GameObject GetTracedObject()
    {
        return TracedObject;
    }
    public void SetTracedObject(GameObject TracedObject)
    {
        this.TracedObject = TracedObject;
    }
    
    private void Start()
    {
        Player = GameObject.FindObjectOfType<PlayerController>();
        audioSource = GetComponent<AudioSource>();
        ControlObjectTrace();
    }
    private void ControlObjectTrace()
    {
        TraceStart = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z + 0.3f);
        TraceEnd = Camera.main.transform.forward.normalized;

        RaycastHit TraceHit;
        
        if (Physics.Raycast(TraceStart, TraceEnd, out TraceHit, TraceMaxDistance, TraceLayerMask))
        {
            TracedObject = TraceHit.collider.gameObject;
            /** Scale player shoot ui */
            Player.ShootUI.transform.localScale = new Vector3(6.0f, 6.0f, 6.0f);
            
            /** Set is drag*/
            Player.SetIsDrag(true);

            audioSource.PlayOneShot(DragSound, 0.1f);

            InvokeRepeating("DragObject", 0.001f, 0.001f);

            Debug.DrawRay(TraceStart, TraceEnd, Color.red, 10.0f);
        }
        
        /** Disable box collider */
        gameObject.GetComponent<BoxCollider>().enabled = false;
    }

    private void DragObject()
    {
        TracedObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;       
        TracedObject.GetComponent<Rigidbody>().velocity = 
            (GameObject.FindGameObjectWithTag("ObjectHolder").transform.position - TracedObject.transform.position)*10.0f;
    }
}
