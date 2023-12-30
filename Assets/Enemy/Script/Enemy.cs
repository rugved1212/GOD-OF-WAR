using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    // Enemy AI

    public Transform[] waypoints;
    public float walkSpeed = 2f;
    public float chaseRange = 10f;
    private Transform player;
    private NavMeshAgent agent;
    private int currentWaypoint;
    private Animator animator;
    public int damageAmount = 20;

    // Enemy Health
    public int HP = 100;
    public Slider healthBar;

    public bool enemy_died = false;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentWaypoint = Random.Range(0, waypoints.Length);
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Enemy Health
        healthBar.value = HP;

        // Enemy AI

        HealthScript healthScript = player.GetComponent<HealthScript>();

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= chaseRange && !healthScript.isDie)
        {
            animator.SetBool("isChasing", true);
        }
        else
        {
            animator.SetBool("isPatrolling", true);

            agent.speed = walkSpeed;
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
                agent.SetDestination(waypoints[currentWaypoint].position);
            }
        }

    }

    public void PerformAttack()
    {
        HealthScript healthScript = player.GetComponent<HealthScript>();
        healthScript.TakeDamage(damageAmount);
    }

    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;
        if (HP <= 0)
        {
            animator.SetTrigger("die");
            GetComponent<Collider>().enabled = false;
            Destroy(healthBar.gameObject);
            agent.isStopped = true;
            enemy_died = true;
        }
        else
        {
            animator.SetTrigger("damage");
        }
        Debug.Log("Damage taken: " + damageAmount);
    }

}
