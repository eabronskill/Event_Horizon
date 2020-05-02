﻿using UnityEngine;
using UnityEngine.SceneManagement;
using Rewired;

public class TankInput : Player
{
    private TankAbilities abilities;

    /// <summary>
    /// ID of the player who is controlling this character.
    /// </summary>
    public int playerID;

    // Attacking vars
    public GameObject attackPoint;
    public GameObject bulletPrefab;
    private float hammerTimer = 0;
    private float strapTimer = 0;
    private float strapWarmupTime = 1f;
    public float fireRate;

    // Ability vars
    private bool canPlaceShield = true;
    [HideInInspector]
    public bool canPound = true;

    // For Brett
    public float shieldCD;
    public float groundPoundCD;
    public bool shieldDown;

    public ParticleSystem particleGroundPound;

    //sounds
    public AudioSource gunshot;
    public AudioSource groundpound;
    public AudioSource setItem;
    public AudioSource melee;

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
        if (ChS_Controller.finalSelection.ContainsKey("Tank Icon"))
        {
            player = ReInput.players.GetPlayer(ChS_Controller.finalSelection["Tank Icon"]);
            MultipleTargetCamera.targets.Add(this.gameObject);
            
            playerID = player.id;
            controller = GetComponent<CharacterController>();
            if (UIEventCOntroller.players.Count == 0)
            {
                UIEventCOntroller.players.Add("Tank", this.gameObject);
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

        abilities = this.gameObject.GetComponent<TankAbilities>();
        
    }

    // Update is called once per frame
    new void Update()
    {
        shieldCD = abilities.shieldTimeRemaining;
        groundPoundCD = abilities.groundPoundTimeRemaining;
        shieldDown = abilities.shieldDown;

        // Call the Player FixedUpdate method.
        base.Update();

        // Shooting
        if (player.GetButton("Shoot") && Time.time >= strapTimer && base.curClip > 0)
        {
            strapTimer = Time.time + fireRate;
            Instantiate(bulletPrefab, attackPoint.transform.position, attackPoint.transform.rotation);
            shot = true;
            gunshot.Play();
            base.curClip--;
            
        }
        if (player.GetButton("Shoot") && base.curClip > 0)
        {
            base.curMoveSpeed = movementSpeed * 0.5f;
        }
        else
        {
            base.curMoveSpeed = movementSpeed;
        }

        // Melee
        if (player.GetButtonDown("Melee") && Time.time >= hammerTimer)
        {
            playerAnimator.SetTrigger("Melee");
            melee.Play();
        }

        // Ability 1: Shield Plant
        if (canPlaceShield && player.GetButtonDown("Ability1"))
        {
            // Call TankAbilities Script
            usedAbility1 = true;
            abilities.shieldPlant();
            setItem.Play();
            canPlaceShield = false;
            
        }

        // Ability 2: Ground Pound
        if (player.GetButtonDown("Ability2") && canPound)
        {
            // Call TankAbilities Script
            usedAbility2 = true;
            abilities.groundPound();
            groundpound.Play();
            particleGroundPound.Play();
            
        }
        
    }

    /// <summary>
    /// Called when the Tank can interact with things.
    /// </summary>
    /// <param name="other"></param>
    new private void OnTriggerStay(Collider other)
    {
        base.OnTriggerStay(other);

        if (other.gameObject.tag == "Shield")
        {
            // Pickup the shield.
            if (player.GetButtonDown("Interact"))
            {
                canPlaceShield = true;
                abilities.pickupShield();
                setItem.Play();
            }
            
        }

    }

    public void tankRepaired() //repaired by engineer
    {
        base.curHealth += base.maxHealth * .3f;
        if (base.curHealth > base.maxHealth)
        {
            base.curHealth = base.maxHealth;
        }
    }

    new private void OnCollisionEnter(Collision other)
    {
        base.OnCollisionEnter(other);
    }

    /// <summary>
    /// Used by the enemy script to cause damage to this player.
    /// </summary>
    /// <param name="damage"></param>
    public new void takeDamage(float damage)
    {
        base.takeDamage(damage);
    }
    
}
