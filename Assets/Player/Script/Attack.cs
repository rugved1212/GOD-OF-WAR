using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    // Axe_throw
    public Rigidbody axe;
    public float throwForce = 50;
    public Transform target;
    public Transform curve_point;
    private Vector3 old_pos;
    private bool isReturning = false;
    private float time = 0.0f;
    private Animator animator;
    public AxeThrow weapon_rotate;
    public Transform weapon_axe;
    public bool hasWeapon;
    public GameObject meleeWeaponPrefab;


    //AXE_BACK
    public Transform characterBack;
    public Transform characterHand;
    public Transform axe_melee;

    public Vector3 positionOffsetBack = new Vector3(0f, 0.15f, 0f);
    public Vector3 rotationOffsetBack = new Vector3(30f, 180f, -90f);
    public Vector3 positionOffsetHand = new Vector3(0f, 0f, 0f); 
    public Vector3 rotationOffsetHand = new Vector3(0f, 0f, 0f);

    public bool isAxeOnBack = true;

    //Attack
    public bool fight = false;
    public bool axe_fight = false;

    // Raycast
    public int damageAmount = 20;
    public float raycastRange = 10f;
    public Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        // Axe_throw
        animator = GetComponent<Animator>();
        weapon_rotate = weapon_axe.GetComponent<AxeThrow>();

        cam = FindFirstObjectByType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        // AXE_Throw
        if (Input.GetMouseButtonUp(1) && hasWeapon == true && isAxeOnBack == false)
        {
            animator.SetBool("throw", true);

        }
        else
        {
            animator.SetBool("throw", false);
        }


        if (Input.GetKeyDown(KeyCode.R) && hasWeapon == false && isAxeOnBack == false)
        {
            animator.SetBool("return", true);

        }
        else
        {
            animator.SetBool("return", false);
        }

        if (isReturning)
        {
            if (time < 1.0f)
            {
                axe.position = getBQCPoint(time, old_pos, curve_point.position, target.position);
                axe.rotation = Quaternion.Slerp(axe.transform.rotation, target.rotation, 50 * Time.deltaTime);
                time += Time.deltaTime;
            }
            else
            {
                ResetAxe();
            }
        }

        //AXE_back
        if (Input.GetMouseButtonDown(2) && hasWeapon == true)
        {
            // Toggle the axe state when the scroll button is clicked
            isAxeOnBack = !isAxeOnBack;

            if (isAxeOnBack)
            {
                animator.SetTrigger("putAxe 0");

            }
            else
            {
                animator.SetTrigger("getAxe 0");

            }
        }

        // Attack_Controll

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (hasWeapon == false || isAxeOnBack == true)
            {
                animator.SetTrigger("Fight_Move");
                fight = true;
                axe_fight = false;
            }
            else if (hasWeapon == true || isAxeOnBack == false)
            {
                animator.SetTrigger("Attacks");
                axe_fight = true;
                fight = false;
            }
            else
            {
                axe_fight = false;
                fight = false;
            }

        }
        
    }

    void ThrowAxe()
    {
        hasWeapon = false;

        isReturning = false;
        axe.transform.parent = null;
        axe.isKinematic = false;
        axe.AddForce(Camera.main.transform.TransformDirection(Vector3.forward) * throwForce, ForceMode.Impulse);
        axe.AddTorque(axe.transform.TransformDirection(Vector3.right) * 2000, ForceMode.Impulse);
        weapon_rotate.activated = true;
    }

    void ReturnAxe()
    {
        hasWeapon = false;

        time = 0.0f;
        old_pos = axe.position;
        isReturning = true;
        axe.velocity = Vector3.zero;
        axe.isKinematic = true;
        weapon_rotate.activated = false;
    }

    void ResetAxe()
    {
        hasWeapon = true;
        isReturning = false;
        axe.transform.parent = target;
        axe.position = target.position;
        axe.rotation = target.rotation;

    }


    Vector3 getBQCPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        Vector3 p = (uu * p0) + (2 * u * t * p1) + (tt * p2);
        return p;
    }

    void AttachAxeToBack()
    {
        // Set the axe's parent to the character's back and adjust its position and rotation
        axe_melee.transform.parent = characterBack;
        axe_melee.transform.localPosition = positionOffsetBack;
        axe_melee.transform.localRotation = Quaternion.Euler(rotationOffsetBack);
    }

    void AttachAxeToHand()
    {
        // Set the axe's parent to the character's hand and adjust its position and rotation
        axe_melee.transform.parent = characterHand;
        axe_melee.transform.localPosition = positionOffsetHand;
        axe_melee.transform.localRotation = Quaternion.Euler(rotationOffsetHand);
    }

    public void ApplyDamage()
    {
        // Get the position and direction of the raycast from this object's transform
        Vector3 raycastOrigin = cam.transform.position;
        Vector3 raycastDirection = cam.transform.forward;

        RaycastHit hit;

        // Perform the raycast
        if (Physics.Raycast(raycastOrigin, raycastDirection, out hit, raycastRange))
        {
            // Check if the object we hit has a Health script
            Enemy health = hit.collider.GetComponent<Enemy>();
            Lizard_script lizard_health = hit.collider.GetComponent<Lizard_script>();
            Predator predator_health = hit.collider.GetComponent<Predator>();
            
            
            // If the object has a Health script, apply damage
            if (health != null)
            {
                health.TakeDamage(damageAmount);
            }
            if (lizard_health != null)
            {
                lizard_health.TakeDamage(damageAmount);
            }
            if (predator_health != null)
            {
                predator_health.TakeDamage(damageAmount);
            }
        }
    }
}

