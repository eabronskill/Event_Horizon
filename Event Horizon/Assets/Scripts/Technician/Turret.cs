using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Turret : MonoBehaviour
{
    private GameObject nearestEnemy;
    private bool targetAlive;
    public GameObject guns;
    public GameObject attackPoint;
    public GameObject projectile;
    public GameObject text;
    private int itr;
    private int currHP;
    private int maxHP;
    private float timer;
    public float turretShotCD = 1;

    public float aliveTimer; // Gets set in the TechnicianAbilities script;

    private float timeRemaining;

    public AudioSource sound;


    // Start is called before the first frame update
    void Start()
    {
        targetAlive = false;

        guns.transform.rotation.SetLookRotation(attackPoint.transform.forward);
        itr = 0;
        maxHP = 100;
        currHP = maxHP;
        timeRemaining = Time.time + aliveTimer;
    }

    // Update is called once per frame
    void Update()
    {
        int i = (int)(timeRemaining - Time.time);
        text.GetComponent<TextMeshPro>().text = "" + i;
        if (text.GetComponent<Billboard>().cam == null)
        {
            text.GetComponent<Billboard>().cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
        }
        
        if (nearestEnemy != null)
            targetAlive = true;
        else
            targetAlive = false;

        if (targetAlive)
        {
            /*Vector3 relativePos = nearestEnemy.transform.position - transform.position; ~~~~~~~~~~also worked
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            guns.transform.rotation = rotation;*/
            guns.transform.LookAt(nearestEnemy.transform.position);
            //attackPoint.transform.LookAt(nearestEnemy.transform.position);
            Vector3 rotatedVector = Quaternion.AngleAxis(-90, Vector3.up) * guns.transform.forward;

            guns.transform.forward = rotatedVector;

            if (Time.time > timer)
            {
                Instantiate(projectile, attackPoint.transform.position, attackPoint.transform.rotation);
                timer = Time.time + turretShotCD;
                sound.Play();
            }
        }

        else
        {
            nearestEnemy = GameObject.FindGameObjectWithTag("Enemy");
            if (!nearestEnemy.GetComponent<Enemy>().active)
            {
                nearestEnemy = null;
            }
        }
    }

           

    void repair()
    {
        currHP = maxHP;
    }
}
