using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{
    public List<GameObject> enemies;
    public GameObject parent;
    private bool activate = false;

    void Start()
    {
        foreach (GameObject o in parent.GetComponentsInChildren<GameObject>())
        {
            enemies.Add(o);
        }
    }

    void Update()
    {
        if (activate)
        {
            foreach(GameObject enemy in enemies)
            {
                enemy.gameObject.GetComponent<Enemy>().active = true;
            }
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            activate = true;
        }
    }
}
