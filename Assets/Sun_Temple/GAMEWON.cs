using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GAMEWON : MonoBehaviour
{
    public GameObject GameWon;
    public Enemy enemy;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy.HP <= 0)
        {
            GameWon.SetActive(true);
        }
    }
}
