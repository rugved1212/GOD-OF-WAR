using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class HealthScript : MonoBehaviour
{

    public int maxHealth = 500;
    public int HP;
    public Slider healthslider;

    public Animator animator;

    public GameObject gameoverScreen;
    public GameObject healthBarScreen;
    public bool isDie = false;


    // Start is called before the first frame update
    void Start()
    {
        HP = maxHealth;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        healthslider.value = HP;
    }

    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;

        // Check if the player has died
        if (HP <= 0)
        {
            Die();
        }
        else
        {
            animator.SetTrigger("damage");
        }
        Debug.Log("Damage taken: " + damageAmount);
    }

    private void Die()
    {

        animator.SetTrigger("die");
        isDie = true;
        gameoverScreen.SetActive(true);
        healthBarScreen.SetActive(false);

        DisableControll();
    }

    private void DisableControll()
    {
        GetComponent<PlayerController>().enabled = false;
        GetComponent<Attack>().enabled = false;

    }
}
