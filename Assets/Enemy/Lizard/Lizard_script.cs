using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Lizard_script : MonoBehaviour
{
    public Transform boatTransform;
    public float swimSpeed = 5f;
    public float stoppingDistance = 3f;
    public float climbing_Speed = 1f;

    public Transform ladder;
    public Vector3 positionOffsetladder = new Vector3(1f, 0f, 0f);
    public Vector3 rotationOffsetladder = new Vector3(0f, 90f, 0f);

    public Transform boat;
    public Vector3 positionOffsetboat = new Vector3(0f, 0f, 0f);


    private float timer = 0;
    private Animator animator;
    private bool isSwimming = true;
    private bool hasTransformed = false;
    private bool Transform_Upon_Boat = false;

    private Transform player;
    private NavMeshAgent agent;
    public float chaseRange = 500f;
    public float moveSpeed = 5f;
    public float attackRange = 1.5f;
    public float attackInterval = 2f;
    private float attackTimer = 0f;

    public int damageAmount = 20;

    public float deathDelay = 5.0f;
    private bool isDead = false;

    public int HP = 100;
    public Slider healthBar;

    public bool enemy_died = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        boatTransform = GameObject.FindGameObjectWithTag("Boat").transform;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;
        HealthScript health = player.GetComponent<HealthScript>();
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.value = HP;


        if (isSwimming && !hasTransformed && HP > 0)
        {
            SwimToBoat();
        }
        else if (!isSwimming && hasTransformed && HP > 0)
        {
            Climbing();
        }
        else if (Transform_Upon_Boat && HP > 0)
        {
            ChaseAttack();
        }
        else if (HP <= 0)
        {
            StartCoroutine(DestroyAfterDelay());
        }
    }

    private void SwimToBoat()
    {
        Vector3 targetPosition = new Vector3(boatTransform.position.x, transform.position.y, boatTransform.position.z);
        Vector3 moveDirection = targetPosition - transform.position;

        if (moveDirection.magnitude > stoppingDistance)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * swimSpeed);

            transform.Translate(Vector3.forward * swimSpeed * Time.deltaTime);

            animator.SetBool("isPatrolling", true);
        }
        else
        {
            isSwimming = false;
            animator.SetBool("isPatrolling", false);

            transform.parent = ladder;
            transform.localPosition = positionOffsetladder;
            transform.localRotation = Quaternion.Euler(rotationOffsetladder);
            hasTransformed = true;
        }

    }

    private void Climbing()
    {
        timer += Time.deltaTime;

        if (timer < 2.5f)
        {
            transform.Translate(Vector3.up * climbing_Speed * Time.deltaTime);
            animator.SetBool("isClimbing", true);
        }
        else
        {
            isSwimming = false;
            hasTransformed = false;
            animator.SetBool("isChasing", true);

            // Set the enemy as the parent of the boat.
            transform.parent = boat;
            transform.localPosition = positionOffsetboat;
            Transform_Upon_Boat = true;
        }
    }

    public void ChaseAttack()
    {
        agent.enabled = true;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= chaseRange)
        {
            animator.SetBool("isChasing", true);
        }

        if (HP <= 0)
        {
            isDead = true;
            agent.isStopped = true;
        }

    }

    void Attack()
    {
        HealthScript health = player.GetComponent<HealthScript>();
        health.TakeDamage(damageAmount);
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(deathDelay);
        Destroy(gameObject);
    }

    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;
        if (HP <= 0)
        {
            animator.SetTrigger("die");
            GetComponent<Collider>().enabled = false;
            Destroy(healthBar.gameObject);
            enemy_died = true;
        }
        else
        {
            animator.SetTrigger("damage");
        }
        Debug.Log("Damage taken: " + damageAmount);
    }
}
