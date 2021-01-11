using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    public float ClearedTime;

    private void Start()
    {
        Destroy(gameObject, ClearedTime);
    }
}
