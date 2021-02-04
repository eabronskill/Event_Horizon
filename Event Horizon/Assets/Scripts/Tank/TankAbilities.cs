using UnityEngine;

/// <summary>
/// Holds the logic for all of the Tank abilities.
/// </summary>
public class TankAbilities : MonoBehaviour
{
    TankInput _tank;

    // Shield Plant vars
    public GameObject _shield;
    public GameObject _shoulderShield;
    public GameObject _shieldPosition;
    public GameObject _shieldLookatPos;

    public float _shieldCooldown;
    [HideInInspector] public float _shieldTimeRemaining = 0;
    [HideInInspector] public bool _shieldDown = false;
    [HideInInspector] public float _shieldTimer = 0f;

    // Ground Pound vars
    public float _shockwaveCD;
    [HideInInspector] public float _shockwaveTimer = 0f;
    [HideInInspector] public float _shockwaveTimeRemaining = 0f;

    void Start()
    {
        _tank = GetComponent<TankInput>();
        _shield.SetActive(false);
    }

    void Update()
    {
        _shockwaveTimeRemaining = _shockwaveTimer - Time.time;
        _shieldTimeRemaining = _shieldTimer - Time.time;

        if (_shockwaveTimeRemaining <= 0) _shockwaveTimeRemaining = 0;
        if (_shieldTimeRemaining <= 0) _shieldTimeRemaining = 0;

        if (Time.time > _shockwaveTimer) _tank._canPound = true;
    }
    
    /// <summary>
    /// Called by the TankInput script when 1 is pressed. Uses the shield plant ability.
    /// </summary>
    public void ShieldPlant()
    {
        // Set the position of the Shield, then its rotation.
        _shield.transform.position = _shieldPosition.transform.position;
        _shield.transform.LookAt(_shieldLookatPos.transform.position);

        // Make the Shield active and the ShoulderShield inactive.
        _shield.SetActive(true);
        _shoulderShield.SetActive(false);

        _shieldDown = true;
    }

    /// <summary>
    /// Called by the TankInput script when E is pressed next to the shield. Picks up the
    /// shield GameObject, deactivating it.
    /// </summary>
    public void PickupShield()
    {
        // Make the Shield inactive and the ShoulderShield active.
        _shield.SetActive(false);
        _shoulderShield.SetActive(true);

        _shieldDown = false;
    }
    
    /// <summary>
    /// Called by the TankInput script when 2 is pressed. Uses the ground pound ability.
    /// </summary>
    public void Shockwave()
    {
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

        _shockwaveTimer = Time.time + _shockwaveCD;
        _tank._canPound = false;
    }
}
