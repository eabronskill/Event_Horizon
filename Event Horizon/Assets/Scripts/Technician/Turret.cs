using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private GameObject nearestEnemy;
    private bool targetAlive;
    public GameObject guns;
    public GameObject attackPoint;
    public GameObject projectile;
    private int itr;

    // Start is called before the first frame update
    void Start()
    {
        targetAlive = false;

        guns.transform.rotation.SetLookRotation(attackPoint.transform.forward);
        itr = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (nearestEnemy != null)
            targetAlive = true;
        else
            targetAlive = false;

        itr++;
        if (targetAlive)
        {
            /*Vector3 relativePos = nearestEnemy.transform.position - transform.position; ~~~~~~~~~~also worked
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            guns.transform.rotation = rotation;*/
            guns.transform.LookAt(nearestEnemy.transform.position);
            //attackPoint.transform.LookAt(nearestEnemy.transform.position);
            Vector3 rotatedVector = Quaternion.AngleAxis(-90, Vector3.up) * guns.transform.forward;

            guns.transform.forward = rotatedVector;

            if (itr == 20)
            {
                Instantiate(projectile, attackPoint.transform.position, attackPoint.transform.rotation);
                itr = 0;
            }
        }

        else
            nearestEnemy = GameObject.FindGameObjectWithTag("Enemy");
    }

    void Fire()
    {

    }
}
