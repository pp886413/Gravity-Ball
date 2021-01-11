using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectTrigger : MonoBehaviour
{
    public float MoveValue;
    public UnityEvent EnterEvent;
    public UnityEvent ExitEvent;

    private float MoveLerpValue = 0.0f;
    private Vector3 BasePosition;
    private Vector3 MovedPosition;

    private List<GameObject> OverlapedObject = new List<GameObject>();

    private void Awake()
    {
        BasePosition = transform.localPosition;
        MovedPosition = new Vector3(BasePosition.x, BasePosition.y + MoveValue, BasePosition.z);
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.transform.tag)
        {
            case "Player":
                if (OverlapedObject.Count < 1)
                {
                    OverlapedObject.Add(collision.gameObject);
                    EnterEvent.Invoke();

                    InvokeRepeating("MoveToPosition", 0.01f, 0.01f);
                    CancelInvoke("BackToPosition");
                }
                break;            
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        switch (collision.transform.tag)
        {
            case "InteractObject":
                if (!collision.gameObject.GetComponent<Rigidbody>().freezeRotation)
                {
                    if (!OverlapedObject.Contains(collision.gameObject))
                    {
                        EnterEvent.Invoke();
                        CancelInvoke("BackToPosition");
                        OverlapedObject.Add(collision.gameObject);
                    }
                    else
                    {
                        MoveToPosition();
                    }
                }
                break;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        switch (collision.transform.tag)
        {
            case "Player":
                if (OverlapedObject.Count > 0)
                {
                    OverlapedObject.Remove(collision.gameObject);

                    if (OverlapedObject.Count == 0)
                    {
                        ExitEvent.Invoke();
                        CancelInvoke("MoveToPosition");
                        InvokeRepeating("BackToPosition", 0.01f, 0.01f);
                    }
                }
                break;
            case "InteractObject":
                if (OverlapedObject.Count > 0)
                {
                    OverlapedObject.Remove(collision.gameObject);

                    if (OverlapedObject.Count == 0)
                    {
                        ExitEvent.Invoke();
                        CancelInvoke("MoveToPosition");
                        InvokeRepeating("BackToPosition", 0.01f, 0.01f);
                    }
                }
                break;
        }
    }
    
    private void MoveToPosition()
    {
        if (MoveLerpValue < 1)
        {
            MoveLerpValue += Time.deltaTime;

            transform.localPosition = Vector3.Lerp(BasePosition, MovedPosition, MoveLerpValue);
        }
        if (transform.localPosition == MovedPosition)
        {
            CancelInvoke("MoveToPosition");
        }
    }
    private void BackToPosition()
    {
        if (MoveLerpValue > 0)
        {
            MoveLerpValue -= Time.deltaTime;

            transform.localPosition = Vector3.Lerp(BasePosition, MovedPosition, MoveLerpValue);
        }
        if (transform.localPosition == BasePosition)
        {
            CancelInvoke("BackToPosition");
        }
    }
}
