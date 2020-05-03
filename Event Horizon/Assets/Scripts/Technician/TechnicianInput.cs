using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System;
using Rewired;

public class TechnicianInput : Player
{
    public GameObject strap;
    public Sword sword;
    
    public GameObject projectile;
    public GameObject attackPoint;
    public GameObject attackPoint2;
    public GameObject attackPoint3;

    private bool canJump;

    private float strapTimer;
    private float swordTimer;


    public Quaternion strapRot;
    public Quaternion finalRot;

    public Quaternion swordRot;
    public GameObject finalSwordRot;

    public ParticleSystem gunFlash;
    public TechnicianAbilities abilities;

    //sounds
    public AudioSource gunshot;
    public AudioSource melee;
    public AudioSource placeTurret;
    public AudioSource healsound;

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
        //if (testing)
        //{
        //    player = ReInput.players.GetPlayer(0);
        //    MultipleTargetCamera.targets.Add(this.gameObject);

        //    playerID = 2;
        //    controller = GetComponent<CharacterController>();
        //    if (UIEventCOntroller.players.Count == 0)
        //    {
        //        UIEventCOntroller.players.Add("Engineer", this.gameObject);
        //    }
        //    if (SceneManager.GetActiveScene().name == "Level1")
        //    {
        //        Tutotrial.players.Add(this.gameObject);
        //    }
        //}
        if (ChS_Controller.finalSelection.ContainsKey("Engineer Icon"))
        {
            player = ReInput.players.GetPlayer(ChS_Controller.finalSelection["Engineer Icon"]);
            MultipleTargetCamera.targets.Add(this.gameObject);
            
            playerID = player.id;
            controller = GetComponent<CharacterController>();

            if (UIEventCOntroller.players.Count == 0)
            {
                UIEventCOntroller.players.Add("Technician", this.gameObject);
            }
            if (SceneManager.GetActiveScene().name == "Level1")
            {
                Tutotrial.players.Add(this.gameObject);
            }
        }
        else
        {
            this.gameObject.SetActive(false);
        }
        



        GetComponent<Rigidbody>().freezeRotation = true;

        canJump = true;
        

        strapTimer = 0f;
        swordTimer = 0f;

        strapRot = strap.transform.localRotation;
        finalRot = strapRot;
        finalRot.x -= .6f;

        curAmmo = maxAmmo;
        curClip = maxClip;
        curHealth = maxHealth;
        curMoveSpeed = movementSpeed;

        abilities = this.gameObject.GetComponent<TechnicianAbilities>();

    }

    // Update is called once per frame
    void Update()
    {
        base.Update();

        if (Input.GetKeyDown("space") && canJump)
            jump();

        if (player.GetButtonDown("Ability1") && abilities.canSetTurret)
        {
            usedAbility1 = true;
            abilities.setTurret();
            placeTurret.Play();
        }
        if (player.GetButtonDown("Ability2") && abilities.canRepair && abilities.turretSet)
        {
            usedAbility2 = true;
            abilities.repair();
            healsound.Play();
        }
        
        if (canShooty)
        {
            if (player.GetButton("Shoot") && Time.time >= strapTimer && base.curClip > 0)
            {
                strapTimer = Time.time + .6f;
                Instantiate(projectile, attackPoint.transform.position, attackPoint.transform.rotation);
                Instantiate(projectile, attackPoint.transform.position, attackPoint2.transform.rotation);
                Instantiate(projectile, attackPoint.transform.position, attackPoint3.transform.rotation);

                shot = true;
                if (!gunshot.isPlaying)
                {
                    gunshot.Play();
                }

                base.curClip -= 3;
                if (base.curClip < 0)
                {
                    curClip = 0;
                }
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
        

    }

    public void engineerHeal()
    {
        curHealth += maxHealth * 0.5f;
        if (curHealth > maxHealth)
        {
            curHealth = maxHealth;
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
