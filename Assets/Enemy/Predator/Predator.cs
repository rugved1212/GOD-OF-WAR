using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Predator : MonoBehaviour
{
    public int HP = 100;
    public Animator animator;
    public Slider healthBar;
    public bool enemy_died = false;
    public int damageAmount = 100;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.value = HP;
    }

    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;
        if (HP <= 0)
        {
            //Play Death Animation
            animator.SetTrigger("die");
            GetComponent<Collider>().enabled = false;

            Destroy(healthBar.gameObject);

            enemy_died = true;
        }
        else
        {
            //Play Get Hit Animation
            animator.SetTrigger("damage");
        }

        Debug.Log("Damage taken: " + damageAmount);
    }

    void Predator_Attack()
    {
        HealthScript health = player.GetComponent<HealthScript>();
        health.TakeDamage(damageAmount);
    }
}
