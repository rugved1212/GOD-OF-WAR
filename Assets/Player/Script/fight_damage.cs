using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class fight_damage : MonoBehaviour
{
    public int damageAmount = 20;
    public Attack attackControl;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject attackControlObject = GameObject.FindGameObjectWithTag("Player");
        if (attackControlObject != null)
        {
            attackControl = attackControlObject.GetComponent<Attack>();
        }
        else
        {
            Debug.LogError("AttackControl object not found!");
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<Enemy>().TakeDamage(damageAmount);
        }
    }

    
}
