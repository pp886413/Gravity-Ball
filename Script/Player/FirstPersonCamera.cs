using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    /** Mouse move speed*/
    [Header("Mouse Sensity")]
    public float MouseX_Sensity;
    public float MouseY_Senesity;
   
    [Header("Player Reference")]
    public GameObject Player;

    private float MouseX;
    private float MouseY;
    private float CameraPitch;
    private float CameraYaw;

    private Quaternion CameraRot;

    private void Start()
    {
        /** Lock cursor on screen middle and disable visible */
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        if (!PlayerController.IsGamePause)
        {
            CameraRotation();
        }
    }
    private void CameraRotation()
    {
        /** Input axis */
        MouseX = Input.GetAxisRaw("Mouse X") * MouseX_Sensity;
        MouseY = Input.GetAxisRaw("Mouse Y") * MouseY_Senesity;

        /** Calculate camera rotation */
        CameraYaw += MouseX;
        CameraPitch -= MouseY;

        /** Limit camera pitch axis */
        CameraPitch = Mathf.Clamp(CameraPitch, -60.0f, 60.0f);

        /** Rotate camera direction base on mouse input */
        CameraRot = Quaternion.Euler(CameraPitch, CameraYaw, 0.0f);
        transform.rotation = CameraRot;

        /** Rotate player base on camera yaw rotation */
        Player.transform.Rotate(Vector3.up * MouseX);
    }
}
