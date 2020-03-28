using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{
    public List<GameObject> enemies;
    private bool activate = false;

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
