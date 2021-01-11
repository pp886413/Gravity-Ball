using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoorTrigger : MonoBehaviour
{
    public UnityEvent OpenDoorEvent;
    public UnityEvent CloseDoorEvent;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.tag == "Player")
        {
            CloseDoorEvent.Invoke();
            OpenDoorEvent.Invoke();
            Destroy(this.gameObject);
        }
    }

}
