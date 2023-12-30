using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Island_move : MonoBehaviour
{
    [SerializeField] private Vector3 direction = Vector3.back;
    [SerializeField] private float speed = 8.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = direction.normalized * speed * Time.deltaTime;

        transform.position += movement;
    }
}
