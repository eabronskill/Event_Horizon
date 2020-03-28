using UnityEngine;
using Rewired;

public class TankInput : Player
{
    private TankAbilities abilities;
    private Melee melee;

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

    void Awake()
    {
        // TRY CATCH FOR TESTING.
        try
        {
            player = ReInput.players.GetPlayer(ChS_Controller.finalSelection["Tank Icon"]);
        }
        catch
        {
            player = ReInput.players.GetPlayer(0);
            testing = true;
        }
        curAmmo = maxAmmo;
        curClip = maxClip;
        curHealth = maxHealth;
        curMoveSpeed = movementSpeed;
    }

    // Start is called before the first frame update
    void Start()
    {
        abilities = this.gameObject.GetComponent<TankAbilities>();
        melee = this.gameObject.GetComponent<Melee>();
        playerID = player.id;
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
            hammerTimer = Time.time + 1f;
            //melee.tankMelee();
        }

        // Ability 1: Shield Plant
        if (canPlaceShield && player.GetButtonDown("Ability1"))
        {
            // Call TankAbilities Script
            abilities.shieldPlant();
            canPlaceShield = false;
        }

        // Ability 2: Ground Pound
        if (player.GetButtonDown("Ability2") && canPound)
        {
            // Call TankAbilities Script
            abilities.groundPound();
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
            }
            
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
