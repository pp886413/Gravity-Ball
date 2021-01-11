using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetInteractObjectPos : MonoBehaviour
{
    private Vector3 BasePosition = new Vector3();

    private void Awake()
    {
        BasePosition = transform.position;
    }
    public Vector3 GetBasePosition()
    {
        return BasePosition;
    }
}
