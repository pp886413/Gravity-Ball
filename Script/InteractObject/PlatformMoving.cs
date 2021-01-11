using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMoving : MonoBehaviour
{
    public GameObject[] MoveDestination;
    public float MoveSpeed;

    private int DestinationIndex = 0;
    private int MaxDestinationIndex = 0;

    private bool Reverse = false;
    private bool Active = false;

    public void SetActive(bool Active)
    {
        this.Active = Active;
    }

    private void Awake()
    {
        if (MoveDestination.Length > 0)
        {
            MaxDestinationIndex = MoveDestination.Length;
        }
    }
    private void Update()
    {
        if (MoveDestination.Length > 0 && Active)
        {
            MoveTo();
        }
    }
    private void MoveTo()
    {
        if (Vector3.Distance(transform.position, MoveDestination[DestinationIndex].transform.position) < 0.001f)
        { 
            if (!Reverse)
            {
                if (DestinationIndex == MaxDestinationIndex-1)
                {
                    Reverse = true;
                    DestinationIndex -= 1;
                }
                DestinationIndex += 1;
            }
            else
            {
                if (DestinationIndex == 0)
                {
                    Reverse = false;
                    DestinationIndex += 1;
                }
                else
                {
                    DestinationIndex -= 1;
                }
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, MoveDestination[DestinationIndex].transform.position, MoveSpeed * Time.deltaTime);
        }
    }
}
