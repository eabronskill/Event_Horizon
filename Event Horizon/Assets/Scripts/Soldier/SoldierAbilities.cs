using UnityEngine;
using UnityEngine.UI;

public class SoldierAbilities : MonoBehaviour
{
    private SoldierInput soldier;

    // Rapid Fire vars
    private float curAmmo;
    private float cnsmAmmoTimer = 0f;

    public float rapidFireDuration;
    public float rfFireRate;

    public float rapidFireCD;
    private float rapidFireTimer = 0f;
    public float rapidFireTimeRemaining = 0f;

    // Grenade vars
    public GameObject grenadePrefab;
    public GameObject grenadeStart;

    public float grenadeCD;
    public float throwForce;

    private float grenadeTimer = 0f;
    public float grenadeTimeRemaining = 0f;
    public bool canUseGrenade = true;

    //public GameObject stunSprite;
    private float spriteTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        soldier = GetComponent<SoldierInput>();
    }

    void Update()
    {
        rapidFireTimeRemaining = rapidFireTimer - Time.time;
        grenadeTimeRemaining = grenadeTimer - Time.time;
        if (rapidFireTimeRemaining <= 0)
        {
            rapidFireTimeRemaining = 0;
        }
        if (grenadeTimeRemaining <= 0)
        {
            grenadeTimeRemaining = 0;
        }

        if (Time.time > cnsmAmmoTimer)
        {
            soldier.cnsmAmmo = true;
            soldier.fireRate = soldier.baseFireRate;
        }
        if (Time.time > grenadeTimer)
        {
            canUseGrenade = true;
        }
        if (Time.time > rapidFireTimer)
        {
            soldier.canUseRF = true;
        }
    }

    /// <summary>
    /// Called by the TankInput script when 1 is pressed. Uses the shield plant ability.
    /// </summary>
    public void rapidFire()
    {
        // Animation
        // TODO

        // Don't consume ammo.
        cnsmAmmoTimer = Time.time + rapidFireDuration;
        soldier.cnsmAmmo = false;

        // Increase fire rate.
        soldier.fireRate = rfFireRate;

        // Set timers
        rapidFireTimer = Time.time + rapidFireCD;
        soldier.canUseRF = false;
        
    }

    /// <summary>
    /// Called by the TankInput script when 2 is pressed. Uses the ground pound ability.
    /// </summary>
    public void grenadeToss()
    {
        // Animation
        // TODO
        
        // Create a grenade and apply force
        GameObject grenade = Instantiate(grenadePrefab, grenadeStart.transform.position, grenadeStart.transform.rotation);
        Rigidbody rb = grenade.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * throwForce, ForceMode.VelocityChange);

        // Set timers
        grenadeTimer = Time.time + grenadeCD;
        canUseGrenade = false;
    }
}
