using UnityEngine;

public class TankInput : Player
{
    private TankAbilities abilities;
    private Melee melee;

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

    // Start is called before the first frame update
    void Start()
    {
        abilities = this.gameObject.GetComponent<TankAbilities>();
        melee = this.gameObject.GetComponent<Melee>();
    }

    // Update is called once per frame
    new void FixedUpdate()
    {
        // Call the Player FixedUpdate method.
        base.FixedUpdate();

        // Shooting
        if (Input.GetMouseButton(0) && Time.time >= strapTimer && ammo > 0)
        {
            strapTimer = Time.time + fireRate;
            Instantiate(bulletPrefab, attackPoint.transform.position, attackPoint.transform.rotation);
            base.ammo--;
        }

        // Melee
        if (Input.GetMouseButtonDown(1) && Time.time >= hammerTimer)
        {
            hammerTimer = Time.time + 1f;
            melee.tankMelee();
        }

        // Ability 1: Shield Plant
        if (canPlaceShield && Input.GetKey(KeyCode.Alpha1))
        {
            // Call TankAbilities Script
            abilities.shieldPlant();
            canPlaceShield = false;
        }

        // Ability 2: Ground Pound
        if (Input.GetKey(KeyCode.Alpha2) && canPound)
        {
            // Call TankAbilities Script
            abilities.groundPound();
        }
        
    }

    /// <summary>
    /// Called when the Tank can interact with things.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Shield")
        {
            // Pickup the shield.
            if (Input.GetKey(KeyCode.E))
            {
                canPlaceShield = true;
                abilities.pickupShield();
            }
            
        }

    }
    
}
