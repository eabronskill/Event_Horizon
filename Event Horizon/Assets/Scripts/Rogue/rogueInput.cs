using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System;
using Rewired;

public class rogueInput : Player
{
    public GameObject strap;
    public Sword sword;

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

    /// <summary>
    /// ID of the player who is controlling this character.
    /// </summary>
    public int playerID;


    void Awake()
    {
        curAmmo = maxAmmo;
        curClip = maxClip;
        curHealth = maxHealth;
        curMoveSpeed = movementSpeed;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (ChS_Controller.finalSelection.ContainsKey("Rogue Icon"))
        {
            player = ReInput.players.GetPlayer(ChS_Controller.finalSelection["Rogue Icon"]);
            MultipleTargetCamera.targets.Add(this.gameObject);
            Tutotrial.players.Add(this.gameObject);
            playerID = player.id;
            controller = GetComponent<CharacterController>();
            if (UIEventCOntroller.players.Count == 0)
            {
                UIEventCOntroller.players.Add("Rogue", this.gameObject);
            }

        }
        else
        {
            this.gameObject.SetActive(false);
        }

        //GetComponent<Rigidbody>().freezeRotation = true;
        

        canJump = true;
        

        strapTimer = 0f;
        swordTimer = 0f;

        strapRot = strap.transform.localRotation;
        finalRot = strapRot;
        finalRot.x -= .6f;

        abilities = this.gameObject.GetComponent<RogueAbilities>();

    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();

        if (Input.GetKeyDown("space") && canJump)
            jump();
        if (Input.GetKeyDown("v") && canJump)
            dash();
        if (player.GetButtonDown("Ability1") && abilities.canSetSpikes)
        {
            abilities.setSpikes();
        }
        if (player.GetButtonDown("Ability2") && abilities.canSetMine && !abilities.mineSet)
        {
            abilities.setMine();
        }
        else if (player.GetButtonDown("Ability2") && abilities.mineSet)
        {
            abilities.detonate();
        }

        if (player.GetButton("Shoot") && Time.time >= strapTimer && base.curClip > 0)
        {
            strapTimer = Time.time + .6f;
            Instantiate(projectile, attackPoint.transform.position, attackPoint.transform.rotation);
            gunshot.Play();
            //gunFlash.Play();

            base.curClip--;
        }

        

        if (strapTimer > Time.time) //gun movement
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
    
    //public void resetCharacter()
    //{
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    //}

    /// <summary>
    /// Used by the enemy script to cause damage to this player.
    /// </summary>
    /// <param name="damage"></param>
    public new void takeDamage(float damage)
    {
        base.takeDamage(damage);
    }

}
