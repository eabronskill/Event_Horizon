﻿using UnityEngine;

public class TankAbilities : MonoBehaviour
{
    private TankInput tank;

    // Shield Plant vars
    public GameObject shield;
    public GameObject shoulderShield;
    public GameObject shieldPosition;
    public GameObject shieldLookatPos;

    public float shieldCooldown;
    public float shieldTimer = 0f;
    public float shieldTimeRemaining = 0;
    public bool shieldDown = false;

    // Ground Pound vars
    public GameObject stunSprite;
    public float groundPoundCD;
    public float groundPoundTimer = 0f;
    public float groundPoundTimeRemaining = 0f;
    private float spriteTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        tank = GetComponent<TankInput>();
        shield.SetActive(false);
        stunSprite.SetActive(false);
    }

    void Update()
    {
        groundPoundTimeRemaining = groundPoundTimer - Time.time;
        shieldTimeRemaining = shieldTimer - Time.time;
        if (groundPoundTimeRemaining <= 0)
        {
            groundPoundTimeRemaining = 0;
        }
        if (shieldTimeRemaining <= 0)
        {
            shieldTimeRemaining = 0;
        }
        
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

        shieldDown = true;
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

        shieldDown = false;
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
            if (hitColliders[i].tag == "Enemy")
            {
                print(i);
                hitColliders[i].gameObject.GetComponent<Enemy>().stun();
            }
            i++;
        }

        groundPoundTimer = Time.time + groundPoundCD;
        tank.canPound = false;
    }
}
