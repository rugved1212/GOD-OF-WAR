using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Son_script : MonoBehaviour
{

    public Transform[] waypoints;
    public Transform player;
    public float waitDistance = 10f;
    public float detectionRadius = 10f;


    private int currentWaypointIndex = 0;
    private NavMeshAgent agent;
    private Animator animator;
    private bool isWaiting = false;

    private Transform targetEnemy;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(waypoints[currentWaypointIndex].position);
    }

    // Update is called once per frame
    void Update()
    {
        float distance_of_player = Vector3.Distance(player.position, transform.position);

        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0f;

        if (isWaiting)
        {
            if (distance_of_player < waitDistance)
            {
                isWaiting = false;
                agent.SetDestination(waypoints[currentWaypointIndex].position);
            }
            else
            {
                Quaternion rotation = Quaternion.LookRotation(directionToPlayer);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5f);
            }
            return;
        }


        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending && targetEnemy == null)
        {
            // Move to the next waypoint.
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length - 1)
            {
                agent.isStopped = true;
                animator.SetBool("Move", false);
                return;
            }

            // Check if the player is nearby to start waiting.
            if (distance_of_player < waitDistance)
            {
                isWaiting = false;
                agent.SetDestination(waypoints[currentWaypointIndex].position);
            }
            else
            {
                // The player is not nearby, so start waiting.
                isWaiting = true;
                // Stop the AI from moving and play an idle animation while waiting.
                agent.ResetPath();
                animator.SetBool("Move", false);
            }
        }

        float velocityMagnitude = agent.velocity.magnitude;
        bool isMoving = velocityMagnitude > 0.1f;
        animator.SetBool("Move", isMoving);
    }

    
}
