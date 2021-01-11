using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidDispear : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "InteractObject")
        {
            SetInteractObjectPos LostObject = collision.gameObject.GetComponent<SetInteractObjectPos>();

            LostObject.gameObject.transform.position = LostObject.GetBasePosition();
        }
    }
}
