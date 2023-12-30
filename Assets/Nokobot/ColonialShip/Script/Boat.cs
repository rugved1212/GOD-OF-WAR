using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boat : MonoBehaviour
{

    public Vector3 MovingDistances = new Vector3(0.002f, 0.001f, 0f);
    public float speed = 1f;
    public Vector3 WaveRotations;
    public float WaveRotationsSpeed = 0.3f;
    public Vector3 AxisOffsetSpeed;
    Transform actualPos;

    // Health
    public int maxHealth = 1000;
    public int currentHealth;
    public GameObject gameoverScreen;
    public GameObject healthBarScreen;
    public Slider healthBar;
    public Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        actualPos = transform;
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mov = new Vector3(
            actualPos.position.x + Mathf.Sin(speed * Time.time) * MovingDistances.x,
            actualPos.position.y + Mathf.Sin(speed * Time.time) * MovingDistances.y,
            actualPos.position.z + Mathf.Sin(speed * Time.time) * MovingDistances.z
        );

        //change rotations
        transform.rotation = Quaternion.Euler(
            actualPos.rotation.x + WaveRotations.x * Mathf.Sin(Time.time * WaveRotationsSpeed),
            actualPos.rotation.y + WaveRotations.y * Mathf.Sin(Time.time * WaveRotationsSpeed),
            actualPos.rotation.z + WaveRotations.z * Mathf.Sin(Time.time * WaveRotationsSpeed)
        );

        //inject the changes
        transform.position = mov;

        //offset
        var tran = transform.position;

        tran.x += AxisOffsetSpeed.x * Time.deltaTime;
        tran.y += AxisOffsetSpeed.y * Time.deltaTime;
        tran.z += AxisOffsetSpeed.z * Time.deltaTime;

        transform.position = tran;

        // HEALTH
        healthBar.value = currentHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        animator.SetTrigger("drown");

        gameoverScreen.SetActive(true);
        healthBarScreen.SetActive(false);

    }
}
