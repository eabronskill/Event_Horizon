using UnityEngine;
using Rewired;

public class SoldierInput : Player
{
    private SoldierAbilities abilities;
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
    public float fireRate;

    // Ability vars
    [HideInInspector]
    public float baseFireRate;
    [HideInInspector]
    public bool cnsmAmmo = true;
    [HideInInspector]
    public bool canUseGrenade = true;
    [HideInInspector]
    public bool canUseRF = true;

    // For Brett
    public float rapidFireCD;
    public float grenadeCD;

    new void Awake()
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
    }

    // Start is called before the first frame update
    void Start()
    {
        baseFireRate = fireRate;
        abilities = this.gameObject.GetComponent<SoldierAbilities>();
        melee = this.gameObject.GetComponent<Melee>();
        playerID = player.id;
    }

    // Update is called once per frame
    new void Update()
    {
        rapidFireCD = abilities.rapidFireTimeRemaining;
        grenadeCD = abilities.grenadeTimeRemaining;
        // Call the Player FixedUpdate method.
        base.Update();
        
        // Shooting
        if (player.GetButton("Shoot") && Time.time >= strapTimer && base.curClip > 0)
        {
            strapTimer = Time.time + fireRate;
            Instantiate(bulletPrefab, attackPoint.transform.position, attackPoint.transform.rotation);
            if (cnsmAmmo)
            {
                base.curAmmo--;
                base.curClip--;
            }
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
        if (player.GetButton("Melee") && Time.time >= hammerTimer)
        {
            hammerTimer = Time.time + 1f;
            melee.soldierMelee();
        }

        // Ability 1: Rapid Fire
        if (player.GetButton("Ability1") && canUseRF)
        {
            // Call SoldierAbilities Script
            abilities.rapidFire();
        }

        // Ability 2: Grenade 
        if (player.GetButton("Ability2") && canUseGrenade)
        {
            // Call TankAbilities Script
            abilities.grenadeToss();
        }
        
    }

    new void OnTriggerStay(Collider other)
    {

        base.OnTriggerStay(other);
    }
}
