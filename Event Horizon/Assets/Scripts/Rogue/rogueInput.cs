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

    public ParticleSystem gunFlash;
    public RogueAbilities abilities;

    public AudioSource gunshot;
    public AudioSource setItem;
    public AudioSource detonateMine;
    public AudioSource melee;


    void Awake()
    {
        _curAmmo = _maxAmmo;
        _curClip = _maxClip;
        _curHealth = _maxHealth;
        _curMoveSpeed = _movementSpeed;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (CharacterSelectController._finalSelection.ContainsKey("Rogue Icon"))
        {
            _player = ReInput.players.GetPlayer(CharacterSelectController._finalSelection["Rogue Icon"]);
            MultipleTargetCamera.targets.Add(this.gameObject);
            
            _playerID = _player.id;
            _controller = GetComponent<CharacterController>();
            if (UIEventCOntroller.players.Count == 0)
            {
                UIEventCOntroller.players.Add("Rogue", this.gameObject);
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
        if (Time.timeScale != 1) return;

        if (Input.GetKeyDown("space") && canJump)
            jump();
        if (Input.GetKeyDown(KeyCode.V) && canJump)
            dash();
        if (_player.GetButton("Ability1") && abilities.canSetSpikes)
        {
            _usedAbility1 = true;
            abilities.setSpikes();
            setItem.Play();
        }
        if (_player.GetButton("Ability2") && abilities.canSetMine && !abilities.mineSet)
        {
            _usedAbility2 = true;
            abilities.setMine();
            setItem.Play();
        }
        else if (_player.GetButtonDown("Ability2") && abilities.mineSet)
        {
            
            abilities.detonate();
            detonateMine.Play();

        }

        ShootingInput();
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

        print(_controller.attachedRigidbody);
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
        base.TakeDamage(damage);
    }

    public override void ResetAbilities()
    {
        abilities.mineSetTime = 0;
        abilities.spikeSetTime = 0;
        abilities.canSetMine = true;
        abilities.canSetSpikes = true;
    }

}
