using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTriggerLineColor : MonoBehaviour
{
    public List<GameObject> Objects;

    public Material ChangedMaterial;
    public Material BaseMaterial;
    public int ID;

    private List<Material> Materials = new List<Material>();

    private void Awake()
    {
        Debug.Log(transform.childCount);
        for (int i = 0; i < transform.childCount; i++)
        {
            Materials.Add(GetComponentInChildren<Renderer>().material);
        }
    }
    public void ChangeToNewMaterial()
    {
        for (int i = 0; i < Materials.Count; i++)
        {
            Materials[i] = ChangedMaterial;
            transform.GetChild(i).GetComponent<Renderer>().material = Materials[i];
        }
    }
    public void ChangeToBaseMaterial()
    {
        for (int i = 0; i < Materials.Count; i++)
        {
            Materials[i] = BaseMaterial;
            transform.GetChild(i).GetComponent<Renderer>().material = Materials[i];
        }
    }
}
