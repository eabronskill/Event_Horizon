using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System;

public class rogueInput : Player
{
    //public float m_speed = 5f;

    private Vector3 startPos;

    public int startHP = 100;
    public int currHP;

    //public GameObject HPtext;
    public GameObject strap;
    //public GameObject melee;
    //public GameObject swordObj;
    public Sword sword;

    //public GameObject hammer;

    public GameObject projectile;
    public GameObject attackPoint;

    private bool canJump;

    private float strapTimer;
    private float swordTimer;


    public Quaternion strapRot;
    public Quaternion finalRot;

    public Quaternion swordRot;
    public GameObject finalSwordRot;

    public AudioSource gunshot;
    public AudioSource music;

    public ParticleSystem gunFlash;
    public RogueAbilities abilities;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;

        currHP = startHP;
        //HPtext.GetComponent<Text>().text = currHP.ToString() + " HP";

        // strap.transform.position.Set(transform.position.x + (float)1.19, transform.position.y + (float)1.59, transform.position.z + (float)1.1);
        //strap.transform.rotation.Set(0, 0, 0, 0);

        //attackPoint.transform.position.Set(transform.position.x + (float)1.19, transform.position.y + (float)2.101, transform.position.z + (float)3);

        //attackPoint.transform.position.Set(transform.position.x + (float)1.19, transform.position.y + (float)2.101, transform.position.z + (float)3);
        GetComponent<Rigidbody>().freezeRotation = true;

        canJump = true;

        strapTimer = 0f;
        swordTimer = 0f;

        strapRot = strap.transform.localRotation;
        finalRot = strapRot;
        finalRot.x -= .6f;

        abilities = this.gameObject.GetComponent<RogueAbilities>();

        //sword = sword.GetComponent<Sword>();

    }

    // Update is called once per frame
    void Update()
    {
        base.Update();

        if (Input.GetKeyDown("space") && canJump)
            jump();
        if (Input.GetKeyDown("v") && canJump)
            dash();
        if (Input.GetKey(KeyCode.Alpha1) && abilities.canSetSpikes)
        {
            abilities.setSpikes();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && abilities.canSetMine && !abilities.mineSet)
        {
            abilities.setMine();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && abilities.mineSet)
        {
            abilities.detonate();
        }

        if (Input.GetMouseButtonDown(0) && Time.time > strapTimer)
        {
            strapTimer = Time.time + .6f;
            Instantiate(projectile, attackPoint.transform.position, attackPoint.transform.rotation);
            gunshot.Play();
            gunFlash.Play();
        }

        if (Input.GetMouseButtonDown(1))
        {
            sword.SwordAttack();
        }

        if (strapTimer > Time.time ) //gun movement
        {
            strapRecoil(strapRot, finalRot);
        }
        else if (strap.transform.localRotation.x != 0)
        {
            strapRotateBack(strap.transform.localRotation, strapRot);
        }

    }

    private void jump()
    {
        canJump = false;
        Invoke("jumpTimer", 1.8f);
        for (int i = 0; i < 6; i++)
        {
            Vector3 movementVec = Vector3.zero;
            movementVec.y = 200f;

            GetComponent<Rigidbody>().AddForce(movementVec);
        }
    }

    private void dash()
    {
        canJump = false;
        Invoke("jumpTimer", 1.8f);

        Vector3 movementVec = Vector3.zero;
        movementVec.x = Input.GetAxis("Horizontal");
        movementVec.z = Input.GetAxis("Vertical");

        for (int i = 0; i < 6; i++)
        {
            GetComponent<Rigidbody>().AddForce(movementVec * 2);
        }
    }

    private void jumpTimer()
    {
        canJump = true;
    }



    private void strapRecoil(Quaternion initRot, Quaternion finalRot)
    {
        strap.transform.localRotation = Quaternion.Lerp(initRot, finalRot, strapTimer - Time.time);
    }

    private void strapRotateBack(Quaternion initRot, Quaternion finalRot)
    {
        strap.transform.localRotation = Quaternion.Lerp(initRot, finalRot, .1f);
    }

    private void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.tag == "LevelEnd")
        {
            Invoke("resetCharacter", 2);
        }
    }


    public void resetCharacter()
    {
        //currHP = startHP;
        //transform.position = startPos;
        //rstEvent.Invoke();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void takeDamage()
    {
        currHP -= 10;

        if (currHP <= 0)
            Invoke("resetCharacter", 0.5f);

        //HPtext.GetComponent<Text>().text = currHP.ToString() + " HP";
    }


}
