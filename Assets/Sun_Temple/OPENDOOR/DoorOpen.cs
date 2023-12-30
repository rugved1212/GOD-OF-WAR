using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    public GameObject Door_prehab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Son"))
        {
            Door door = Door_prehab.GetComponent<Door>();
            door.OpenDoor();
        }
    }
}
