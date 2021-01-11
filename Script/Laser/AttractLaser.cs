using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractLaser : MonoBehaviour
{
    [Header("Laser Move speed")]
    /** Laser move speed */
    public float MoveSpeed;

    [Header("Trace Detect Radius")]
    public float Radius;
    [Header("Trace Type")]
    public LayerMask layerMask;

    [Header("Laser hit particle")]
    public GameObject HitParticle;

    [Header("Hit sound")]
    public AudioClip HitSound;

    /** Rigidbody */
    private Rigidbody LaserRigidBody;
    private Vector3 LastVelocity;

    /** Laser attract trace */
    private Vector3 TraceStart = Vector3.zero;
    private Vector3 TraceEnd = Vector3.zero;
    
    /** List for hit traced gameobject */
    private List<GameObject> AttractedGameObject = new List<GameObject>();

    private bool IsAttracting = false;

    private GameObject PlayerObject;

    private AudioSource audioSource;
    private bool HavePlayHitSound = false;

    /** AttractedGameObject get */
    public List<GameObject> GetAttractedGameObject()
    {
        return AttractedGameObject;
    }
    /** IsAttracting get set */
    public bool GetIsAttracting()
    {
        return IsAttracting;
    }
    public void SetIsAttracting(bool IsAttracting)
    {
        this.IsAttracting = IsAttracting;
    }


    private void Awake()
    {
        LaserRigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        PlayerObject = GameObject.FindGameObjectWithTag("Player");
    }
    private void Start()
    {
        /** Laser force */
        LaserRigidBody.AddForce(Camera.main.transform.forward * MoveSpeed);

        /** Loop sphere*/
        InvokeRepeating("AttractRangeDetect", 0.01f, 0.01f);
    }

    private void Update()
    {
        /** if distance is greater than specific number and it's on array , remove it */
        if (GetDistanceBetweenPlayer() >= 8.0f)
        {
            for (int i = 0; i < AttractedGameObject.Count; i++)
            {
                if (AttractedGameObject[i].gameObject.transform.tag == "Player")
                {
                    AttractedGameObject.RemoveAt(i);
                }
            }
        }
    }
    private void FixedUpdate()
    {
        LastVelocity = LaserRigidBody.velocity;
    }

    /** If laser collider other object , set laser to snap on it */
    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(HitParticle, transform.position, transform.rotation);
        if (!HavePlayHitSound)
        {
            audioSource.PlayOneShot(HitSound, 0.3f);
            HavePlayHitSound = true;
        }
        switch (collision.transform.tag)
        {
            case "ReboundWall":
                LaserRigidBody.velocity = collision.transform.forward * 4.0f;
                HavePlayHitSound = false;
                //Rebound(collision.contacts[0].normal);
                break;
            case "Platform":
                this.gameObject.transform.SetParent(collision.transform);
                LaserRigidBody.isKinematic = true;
                HavePlayHitSound = false;
                break;
            default:
                LaserRigidBody.isKinematic = true;
                break;
        }
    }

    /** Range to detect object who has specific layer mask and give it a attract effect */
    public void AttractRangeDetect()
    {
        TraceStart = this.transform.position;
        TraceEnd = this.transform.position;
        RaycastHit[] Hit = Physics.SphereCastAll(TraceStart, Radius, TraceEnd, Radius, layerMask);

        for (int i = 0; i < Hit.Length; i++)
        {
            if (!AttractedGameObject.Contains(Hit[i].collider.gameObject))
            {
                AttractedGameObject.Add(Hit[i].collider.gameObject);
            }
        }
    }

    /** Set ranged object slowly snap to laser */
    public void Attract()
    {
        IsAttracting = true;
        if (AttractedGameObject.Contains(PlayerObject))
        {
            PlayerObject.GetComponent<PlayerController>().SetAttractMode(true);
        }
        for (int i = 0; i < AttractedGameObject.Count; i++)
        {
            AttractedGameObject[i].GetComponent<Rigidbody>().AddExplosionForce(-2000.0f, TraceEnd, 25.0f,0.0f,ForceMode.Force);
            AttractedGameObject[i].transform.parent = null;
        }
    }

    /** Caculate distance between player */
    private float GetDistanceBetweenPlayer()
    {
        float Distance = Vector3.Distance(transform.position, PlayerObject.transform.position);
        return Distance;
    }

    private void Rebound(Vector3 CollisionNormal)
    {
        float ReBoundSpeed = LastVelocity.magnitude;
        Vector3 ReBoundDirection = Vector3.Reflect(LastVelocity.normalized, CollisionNormal);
        
        LaserRigidBody.velocity = ReBoundDirection * ReBoundSpeed;
    }

    /** attract trace debug */
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(TraceEnd, Radius);
    }
}
