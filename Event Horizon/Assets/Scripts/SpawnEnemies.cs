using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{

    public GameObject spawn1;
    public GameObject spawn2;
    public GameObject spawn3;
    public GameObject spawn4;
    public GameObject enemy;
    private int spawnNum = 1;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (timer < Time.time)
            SpawnMore();
    }

    void SpawnMore()
    {
        //timer = Time.time + 2f;
        //if (spawnNum == 0)
        //    Instantiate(enemy, spawn1.transform.position, spawn1.transform.rotation);
        //else if (spawnNum == 1)
        //    Instantiate(enemy, spawn2.transform.position, spawn2.transform.rotation);
        //else if (spawnNum == 2)
        //    Instantiate(enemy, spawn3.transform.position, spawn3.transform.rotation);
        //else
        //    Instantiate(enemy, spawn4.transform.position, spawn4.transform.rotation);

        //spawnNum = (spawnNum + 1) % 4;
    }
}
