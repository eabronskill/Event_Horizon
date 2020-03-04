using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private GameObject player;
    private float attackSpeed = 2f;
    private bool canAttack;
    private bool attackInvoked;
    private int maxHP = 10;
    private int currHP;

    private Quaternion initRot;
    private Quaternion fallRot;
    private Vector3 deathLoc;
    private Vector3 bulletImpact;

    private float stunnedTimer = 0f;
    private NavMeshAgent nav;
    // declare delegate 
    public delegate void MineHit();

    //declare event of type delegate
    public event MineHit mineExplosionEvent;


    bool dead;
    float deathTimer;

    // Start is called before the first frame update
    void Start()
    {
        //player = GameObject.Find("Tank Model");
        player = GameObject.FindGameObjectWithTag("Player");
        canAttack = true;
        attackInvoked = false;
        currHP = maxHP;
        nav = GetComponent<NavMeshAgent>();
        nav.speed = 8f;

        dead = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (dead && deathTimer > Time.time) //halt updates if dead
        {
            transform.position = deathLoc;
            transform.localRotation = Quaternion.Lerp(fallRot, transform.localRotation, deathTimer - Time.time);
        }

        else
        {
            if (Time.time > stunnedTimer)
            {
                if (canAttack == false && attackInvoked == false)
                {
                    attackInvoked = true;
                    Invoke("setAttack", attackSpeed);
                }
                GetComponent<NavMeshAgent>().SetDestination(player.transform.position);
            }
            else
            {
                GetComponent<NavMeshAgent>().ResetPath();
            }
        }




    }

    private void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "Player" && canAttack)
        {
            coll.collider.gameObject.SendMessage("takeDamage");
            canAttack = false;
            attackInvoked = false;
        }

        if (coll.gameObject.tag == "Bullet")
        {
            //currHP -= 10;
            // if (currHP <= 0)
            // {
            bulletImpact = coll.transform.position;

            deathLoc = transform.position;
            initRot = transform.localRotation;
            fallRot = initRot;

            if (bulletImpact.x > 0)
                fallRot.x -= .5f;
            else
                fallRot.x += .5f;

            if (bulletImpact.z > 0)
                fallRot.z -= .5f;
            else
                fallRot.z += .5f;

            deathTimer = Time.time + 1f;
            dead = true;
            Invoke("die", 1f);
            // }
        }

        if (coll.gameObject.tag == "Spike")
        {
            nav.speed = 2.5f;
            Invoke("resetSpeed", 3f);
        }



        //if (coll.gameObject.tag == "Damage")
        //{
        //    bulletImpact = coll.transform.position;

        //    deathLoc = transform.position;
        //    initRot = transform.localRotation;
        //    fallRot = initRot;

        //    if (bulletImpact.x > 0)
        //        fallRot.x -= .5f;
        //    else
        //        fallRot.x += .5f;

        //    if (bulletImpact.z > 0)
        //        fallRot.z -= .5f;
        //    else
        //        fallRot.z += .5f;

        //    deathTimer = Time.time + 1f;
        //    dead = true;
        //    Invoke("die", 1f);
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "SlowField")
        {
            this.gameObject.SetActive(false);
            print("In here");
        }

        if (other.gameObject.tag == "Mine")
        {
            print("collision");
            if (mineExplosionEvent != null)
            {
                print("event not null");
                mineExplosionEvent();
            }
        }
    }

    private void resetSpeed()
    {
        nav.speed *= 8f;
    }

    private void setAttack()
    {
        canAttack = true;

    }

    private void die()
    {
        Destroy(this.gameObject);
    }

    private void OnCollisionStay(Collision coll)
    {
        if (coll.gameObject.tag == "Player" && canAttack)
        {
            coll.collider.gameObject.SendMessage("takeDamage");
            canAttack = false;
            attackInvoked = false;
        }
    }

    public void stun()
    {
        stunnedTimer = Time.time + 3;
    }



}
