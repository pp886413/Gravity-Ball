using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("PlayerMovement")]
    public float MoveSpeed;
    
    private float Move_Z = 0.0f;
    private float Move_X = 0.0f;

    private Rigidbody PlayerRigidBody;

    [Header("Jump")]
    /** Jump Speed */
    public float JumpPower;
    public AudioClip JumpSound;
    public AudioClip LandSound;

    private bool IsGround = true;

    [Header("Attract Laser")]
    /** Attract laser */
    public GameObject AttractLaser;
    public Transform ShootPosition;
    public AudioClip ShootSound;
    public AudioClip AttractSound;
    public AudioClip StopAttractSound;

    [Header("Attract laser shoot particle & particle pos")]
    public GameObject ShootParticle;
    public GameObject ShootParticlePos;
    
    /** Have been Spawned laser */
    private GameObject ExistAttractLaser;
    
    [Header("Control Laser")]
    /** Control laser */
    public GameObject ControlLaserObject;
    private GameObject ExistControlLaserObject;
    
    [Header("UI")]
    public GameObject ShootUI;
    public GameObject PauseUI;
    public GameObject ControlUI;
    public AudioClip OpenUISound;

    public static bool IsGamePause;

    private bool AttractMode = false;
    private AudioSource audioSource;
    private bool IsDrag = false;


    public void SetAttractMode(bool AttractMode)
    {
        this.AttractMode = AttractMode;
    }
    public void SetIsDrag(bool IsDrag)
    {
        this.IsDrag = IsDrag;
    }

    private void Awake()
    {
        PlayerRigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        ChangeScene.IsMainLevel = true;
    }
    private void Update()
    {
        if (!IsGamePause)
        {
            Jump();
            ShootAttractLaser();
            AttractEffect();

            ShootControlLaser();
        }
        PauseInput();
    }
    private void FixedUpdate()
    {
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        /** Input direction */
        Move_Z = Input.GetAxisRaw("Vertical") * Time.deltaTime;
        Move_X = Input.GetAxisRaw("Horizontal") * Time.deltaTime;

        Vector3 MoveDirection = transform.forward * Move_Z + transform.right * Move_X;

        MoveDirection.Normalize();
        MoveDirection = MoveDirection * MoveSpeed;

        if (!AttractMode)
        {
            PlayerRigidBody.velocity = new Vector3(MoveDirection.x * MoveSpeed, PlayerRigidBody.velocity.y, MoveDirection.z * MoveSpeed);
        }
    }
    private void Jump()
    {
        if (IsGround)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                /** Add jump force power */
                PlayerRigidBody.velocity = new Vector3(PlayerRigidBody.velocity.x, JumpPower, PlayerRigidBody.velocity.z);

                audioSource.PlayOneShot(JumpSound, 1.5f);

                IsGround = false;
            }
        }      
    }
    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.transform.tag)
        {
            case "Ground":
                if (!IsGround)
                {
                    audioSource.PlayOneShot(LandSound, 1.0f);

                    IsGround = true;
                    AttractMode = false;
                }
                break;
            case "Platform":
                this.gameObject.transform.SetParent(collision.transform);
                if (!IsGround)
                {
                    IsGround = true;
                }
                break;
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (AttractMode)
        {
            AttractMode = false;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        switch (collision.transform.tag)
        {
            case "Ground":
                if (AttractMode)
                {
                    IsGround = false;
                }
                break;
            case "Laser":
                IsGround = false;
                AttractMode = false;
                break;
            case "Platform":
                this.gameObject.transform.parent = null;
                break;
        }
    }

    private void ShootAttractLaser()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(ShootParticle, ShootParticlePos.transform.position, ShootParticlePos.transform.rotation);
            audioSource.PlayOneShot(ShootSound, 0.7f);

            if (ExistAttractLaser)
            {
                AttractLaser attractLaser = ExistAttractLaser.GetComponent<AttractLaser>();
                attractLaser.CancelInvoke("AttractRangeDetect");

                Destroy(ExistAttractLaser);
                ExistAttractLaser = null;
                ExistAttractLaser = Instantiate(AttractLaser, ShootPosition.transform.position, ShootPosition.transform.rotation);
            }
            else
            {
                ExistAttractLaser = Instantiate(AttractLaser, ShootPosition.transform.position, ShootPosition.transform.rotation);
            }
        }
    }
    private void AttractEffect()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (ExistAttractLaser)
            {
                AttractLaser attractLaser = ExistAttractLaser.GetComponent<AttractLaser>();

                if (!attractLaser.GetIsAttracting())
                {
                    attractLaser.InvokeRepeating("Attract", 0.01f, 0.01f);
                    audioSource.PlayOneShot(AttractSound, 1.0f);
                }
                else
                {
                    audioSource.PlayOneShot(StopAttractSound, 0.7f);
                    /** Cancel timer */
                    attractLaser.CancelInvoke("AttractRangeDetect");
                    attractLaser.CancelInvoke("Attract");

                    /** false IsAttracting */
                    attractLaser.SetIsAttracting(false);

                    /** Stop freeze attracted object's rotation */
                    for (int i = 0; i < attractLaser.GetAttractedGameObject().Count; i++)
                    {
                        if (attractLaser.GetAttractedGameObject()[i].transform.tag == "InteractObject")
                        {
                            attractLaser.GetAttractedGameObject()[i].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                        }
                    }

                    /** Clear list object*/
                    attractLaser.GetAttractedGameObject().Clear();

                    Destroy(ExistAttractLaser);
                    ExistAttractLaser = null;
                }
            }
        }       
    }

    private void ShootControlLaser()
    { 
        if(Input.GetKeyDown(KeyCode.E))
        {
            if (!IsDrag)
            {
                /** Spawn line trace to check can drag or not */
                ExistControlLaserObject = Instantiate(ControlLaserObject, ShootPosition.position, ShootPosition.rotation);
            }
            else
            {
                CancelControlLaser();
            }
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            /** If not traced for object, destory it */
            ControlLaser controlLaser = ExistControlLaserObject.GetComponent<ControlLaser>();
            if (controlLaser.GetTracedObject() == null)
            {
                Destroy(ExistControlLaserObject);
            }
        }
    }
    private void CancelControlLaser()
    {
        if (ExistControlLaserObject)
        {
            ControlLaser controlLaser = ExistControlLaserObject.GetComponent<ControlLaser>();
            IsDrag = false;

            if (controlLaser.GetTracedObject() != null)
            {
                /** Scale shoot ui to base scale */
                ShootUI.transform.localScale = new Vector3(4.0f, 4.0f, 4.0f);

                controlLaser.GetTracedObject().GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                controlLaser.GetTracedObject().GetComponent<Rigidbody>().velocity = Vector3.zero;

                /** Clear Drag object timer */
                controlLaser.CancelInvoke("DragObject");
                controlLaser.SetTracedObject(null);
            }
        }      
    } 
    private IEnumerator CameraShake(float ShakeDuration, float ShakeStrength)
    {
        Vector3 OriginPos = Camera.main.transform.localPosition;

        float ShakeTime = 0.0f;

        while (ShakeTime < ShakeDuration)
        {
            float x = Random.Range(-1.0f, 1.0f) * ShakeStrength;
            float y = Random.Range(-1.0f, 1.0f) * ShakeStrength;

            Camera.main.transform.localPosition = new Vector3(Mathf.PerlinNoise(0, x), OriginPos.y, OriginPos.z);

            ShakeTime += Time.deltaTime;

            yield return null;
        }
        Camera.main.transform.localPosition = OriginPos;
    }

    private void PauseInput()
    {
        if (Time.timeScale >= 1)
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !IsGamePause)
            {
                Pause();
            }
            else if (Input.GetKeyDown(KeyCode.Escape) && IsGamePause)
            {
                UnPause();
            }
        }
    }
    private void Pause()
    {
        IsGamePause = true;

        ShootUI.SetActive(false);
        PauseUI.SetActive(true);

        audioSource.PlayOneShot(OpenUISound, 0.5f);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void UnPause()
    {
        IsGamePause = false;

        audioSource.PlayOneShot(OpenUISound, 0.5f);

        ShootUI.SetActive(true);
        PauseUI.SetActive(false);
        ControlUI.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
