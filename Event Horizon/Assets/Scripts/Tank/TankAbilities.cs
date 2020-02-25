using UnityEngine;
using UnityEngine.UI;

public class TankAbilities : MonoBehaviour
{
    private TankInput tank;

    // Shield Plant vars
    public GameObject shield;
    public GameObject shoulderShield;
    public GameObject shieldPosition;
    public GameObject shieldLookatPos;
    public Slider shieldCDBar;

    public float shieldCooldown;
    private float shieldCDTimer = 0f;

    // Ground Pound vars
    public GameObject stunSprite;
    public float groundPoundCD;
    private float groundPoundTimer;
    private float spriteTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        tank = GetComponent<TankInput>();
        shield.SetActive(false);
        stunSprite.SetActive(false);
    }

    void FixedUpdate()
    {
        if (Time.time > spriteTimer)
        {
            stunSprite.SetActive(false);
        }
        if (Time.time > groundPoundTimer)
        {
            tank.canPound = true;
        }
    }
    
    /// <summary>
    /// Called by the TankInput script when 1 is pressed. Uses the shield plant ability.
    /// </summary>
    public void shieldPlant()
    {
        // Animation
        // TODO

        // Set the position of the Shield, then its rotation.
        shield.transform.position = shieldPosition.transform.position;
        shield.transform.LookAt(shieldLookatPos.transform.position);

        // Make the Shield active and the ShoulderShield inactive.
        shield.SetActive(true);
        shoulderShield.SetActive(false);
    }

    /// <summary>
    /// Called by the TankInput script when E is pressed next to the shield. Picks up the
    /// shield GameObject, deactivating it.
    /// </summary>
    public void pickupShield()
    {
        // Animation
        // TODO

        // Make the Shield inactive and the ShoulderShield active.
        shield.SetActive(false);
        shoulderShield.SetActive(true);
    }
    
    /// <summary>
    /// Called by the TankInput script when 2 is pressed. Uses the ground pound ability.
    /// </summary>
    public void groundPound()
    {
        // Animation
        // TODO
        stunSprite.SetActive(true);
        spriteTimer = Time.time + 0.5f;

        // Check if distance from Tank to Enemy is within range
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, 10f);
        int i = 0;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].tag == "Damage")
            {
                hitColliders[i].SendMessage("stun");
            }
            i++;
        }

        groundPoundTimer = Time.time + groundPoundCD;
        tank.canPound = false;
    }
}
