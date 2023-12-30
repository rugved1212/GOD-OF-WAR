using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Dragon_script : MonoBehaviour
{
    // Flying_Script
    public float amplitude = 1.5f;
    public float frequency = 1.5f;

    private Vector3 startPos;
    private float timeCounter = 0f;

    // Dragon_AI
    public Transform boat;
    public float flyingDistance = 5f;
    public float movementSpeed = 5f;

    public float dashSpeed = 10f; 
    public float dashDuration = 1f;
    public float dashCooldown = 3f; 


    private bool isDashing = false;
    private float dashTimer = 0f;
    private float dashCooldownTimer = 0f;
    private float timer = 0f;
    public float attackingtime = 0f;
    private Rigidbody rb;

    public string sceneToLoad = "DemoScene";
    public GameObject loadingScreen;
    public Slider loadingBar;

    private bool isLoading = false;


    // Dragon_Health
    public int HP = 100;
    public Slider healthBar;
    private Animator animator;
    public bool enemy_died = false;

    // Start is called before the first frame update
    void Start()
    {
        // Flying_Script
        startPos = transform.position;

        // Dragon_AI
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Flying_Script
        timeCounter += Time.deltaTime * frequency;
        float newYPos = startPos.y + Mathf.Sin(timeCounter) * amplitude;
        transform.position = new Vector3(transform.position.x, newYPos, transform.position.z);

        // Dragon_AI
        attackingtime += Time.deltaTime;

        if (HP <= 0)
        {
            amplitude = 0f;
            frequency = 0f;

            Die();
            if (timer >= 10f)
            {
                StartCoroutine(LoadSceneAsync());
            }
        }

        if (isDashing && HP > 0)
        {
            DashAttack();
        }
        else
        {
            MoveAroundPlayer();
        }

        if (dashCooldownTimer > 0f)
        {
            dashCooldownTimer -= Time.deltaTime;
        }

        if (Vector3.Distance(transform.position, boat.position) >= flyingDistance)
        {
            if (dashCooldownTimer <= 0f)
            {
                StartDashAttack();
                Debug.Log("Dash");
                Boat health = boat.GetComponent<Boat>();
                if (health != null && attackingtime > 115f)
                {
                    if (health.currentHealth > 0)
                    {
                        health.TakeDamage(20);
                    }
                }
                
            }
        }

        healthBar.value = HP;
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
            Die();
        }
        else
        {
            animator.SetTrigger("damage");
        }
        Debug.Log("Damage taken: " + damageAmount);
    }
    void MoveAroundPlayer()
    {
        Vector3 toPlayer = boat.position - transform.position;
        toPlayer.y = 0f;
        Vector3 targetPosition = boat.position - toPlayer.normalized * flyingDistance;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
        transform.LookAt(boat);
    }

    void StartDashAttack()
    {
        isDashing = true;
        dashTimer = dashDuration;
        movementSpeed = 0f;
        dashCooldownTimer = dashCooldown;
    }

    void DashAttack()
    {

        // Vector3 dashMovement = transform.forward * dashSpeed * Time.deltaTime;

        // float altitudeChangeMagnitude = dashSpeed * -25f * Time.deltaTime;
        // float altitudeChange = Mathf.Sin((dashTimer / dashDuration) * Mathf.PI) * altitudeChangeMagnitude;

        // transform.position += dashMovement + Vector3.up * altitudeChange;

        transform.position += transform.forward * dashSpeed * Time.deltaTime;

        dashTimer -= Time.deltaTime;

        if (dashTimer <= 0f)
        {
            isDashing = false;
            movementSpeed = 5f;
        }
    }

    public void Die()
    {

        rb.isKinematic = false;
        rb.useGravity = true;
        timer += Time.deltaTime;
    }

    private IEnumerator LoadSceneAsync()
    {
        isLoading = true;
        loadingScreen.SetActive(true);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);
        while (!asyncLoad.isDone)
        {           
            float progress = Mathf.Clamp01(asyncLoad.progress);
            loadingBar.value = progress;
            yield return null;
        }
        isLoading = false;
        loadingScreen.SetActive(false);
    }
}
