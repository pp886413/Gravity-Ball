using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetIsMainLevel : MonoBehaviour
{
    private void Awake()
    {
        ChangeScene.IsMainLevel = true;
    }
}
