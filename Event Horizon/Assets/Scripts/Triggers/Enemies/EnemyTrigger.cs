using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{
    public List<GameObject> enemies;
    public GameObject parent;
    private bool activate = false;
    private bool triggered = false;

    void Start()
    {
        if (parent != null)
        {
            foreach (Enemy o in parent.GetComponentsInChildren<Enemy>())
            {
                enemies.Add(o.gameObject);
            }
        }
        
    }

    void Update()
    {
        if (activate && !triggered)
        {
            triggered = true;
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
