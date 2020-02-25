using UnityEngine;

public class SoldierInput : Player
{
    private SoldierAbilities abilities;
    private Melee melee;

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

    // Start is called before the first frame update
    void Start()
    {
        baseFireRate = fireRate;
        abilities = this.gameObject.GetComponent<SoldierAbilities>();
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
            if (cnsmAmmo)
            {
                base.ammo--;
            }
        }

        // Melee
        if (Input.GetMouseButtonDown(1) && Time.time >= hammerTimer)
        {
            hammerTimer = Time.time + 1f;
            melee.soldierMelee();
        }

        // Ability 1: Rapid Fire
        if (Input.GetKey(KeyCode.Alpha1))
        {
            // Call SoldierAbilities Script
            abilities.rapidFire();
        }

        // Ability 2: Grenade 
        if (Input.GetKey(KeyCode.Alpha2) && canUseGrenade)
        {
            // Call TankAbilities Script
            abilities.grenadeToss();
        }
        
    }

}
