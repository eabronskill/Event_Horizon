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
        if (ChS_Controller._finalSelection.ContainsKey("Engineer Icon"))
        {
            _player = ReInput.players.GetPlayer(ChS_Controller._finalSelection["Engineer Icon"]);
            MultipleTargetCamera.targets.Add(this.gameObject);
            
            _playerID = _player.id;
            _controller = GetComponent<CharacterController>();

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

        _curAmmo = _maxAmmo;
        _curClip = _maxClip;
        _curHealth = _maxHealth;
        _curMoveSpeed = _movementSpeed;

        abilities = this.gameObject.GetComponent<TechnicianAbilities>();

    }

    // Update is called once per frame
    new void Update()
    {
        if (Time.timeScale != 1) return;
        base.Update();

        if (Input.GetKeyDown("space") && canJump)
            jump();

        if (_player.GetButtonDown("Ability1") && abilities.canSetTurret)
        {
            _usedAbility1 = true;
            abilities.setTurret();
            placeTurret.Play();
        }
        if (_player.GetButtonDown("Ability2") && abilities.canRepair && abilities.turretSet)
        {
            _usedAbility2 = true;
            abilities.repair();
            healsound.Play();
        }
        
        if (_canShoot)
        {
            if (_player.GetButton("Shoot") && Time.time >= strapTimer && base._curClip > 0)
            {
                strapTimer = Time.time + .6f;
                Instantiate(projectile, attackPoint.transform.position, attackPoint.transform.rotation);
                Instantiate(projectile, attackPoint.transform.position, attackPoint2.transform.rotation);
                Instantiate(projectile, attackPoint.transform.position, attackPoint3.transform.rotation);

                _shot = true;
                if (!gunshot.isPlaying)
                {
                    gunshot.Play();
                }

                base._curClip -= 3;
                if (base._curClip < 0)
                {
                    _curClip = 0;
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
        _curHealth += _maxHealth * 0.5f;
        if (_curHealth > _maxHealth)
        {
            _curHealth = _maxHealth;
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
        base.TakeDamage(damage);
    }

    public override void ResetAbilities()
    {
        abilities.turretDestroyedTime = 0;
        abilities.lastRepairTime = 0;
        abilities.canSetTurret = true;
        abilities.canRepair = true;
    }

}
