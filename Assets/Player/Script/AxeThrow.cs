using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeThrow : MonoBehaviour
{
    public bool activated;
    public float rotationSpeed;

    public int damageAmount = 100;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (activated)
        {
            transform.localEulerAngles += Vector3.forward * rotationSpeed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            if (activated == true)
            {
                other.GetComponent<Enemy>().TakeDamage(damageAmount);
            }
            else if (Input.GetMouseButton(0))
            {
                other.GetComponent<Enemy>().TakeDamage(25);
            }
        }
        if (other.tag == "Predator")
        {
            if (activated == true)
            {
                other.GetComponent<Predator>().TakeDamage(damageAmount);
            }
            else if (Input.GetMouseButton(0))
            {
                other.GetComponent<Predator>().TakeDamage(25);
            }
        }
        if (other.tag == "Lizard")
        {
            if (activated == true)
            {
                other.GetComponent<Lizard_script>().TakeDamage(damageAmount);
            }
            else if (Input.GetMouseButton(0))
            {
                other.GetComponent<Lizard_script>().TakeDamage(25);
            }
        }
        if (other.tag == "Dragon")
        {
            if (activated == true)
            {
                other.GetComponent<Dragon_script>().TakeDamage(damageAmount);
            }
            else if (Input.GetMouseButton(0))
            {
                other.GetComponent<Dragon_script>().TakeDamage(25);
            }
        }
    }
}
