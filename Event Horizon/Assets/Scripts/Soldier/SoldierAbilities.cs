using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Holds the logic for all the Soldier Abilities.
/// </summary>
public class SoldierAbilities : MonoBehaviour
{
    SoldierInput _soldier;

    // Rapid Fire vars
    public float _rapidFireDuration;
    public float _rfFireRate;
    public float _rapidFireCD;
    [HideInInspector] public float _rapidFireTimer = 0f;
    [HideInInspector] public float _rapidFireTimeRemaining = 0f;
    float _consumeAmmoTimer = 0f;

    // Grenade vars
    public GameObject _grenadePrefab;
    public GameObject _grenadeStart;
    public float _grenadeCD;
    public float _throwForce;
    [HideInInspector] public float _grenadeTimer = 0f;
    [HideInInspector] public float _grenadeTimeRemaining = 0f;
    public bool _canUseGrenade = true;

    void Start() => _soldier = GetComponent<SoldierInput>();

    void Update()
    {
        _rapidFireTimeRemaining = _rapidFireTimer - Time.time;
        _grenadeTimeRemaining = _grenadeTimer - Time.time;

        if (_rapidFireTimeRemaining <= 0) _rapidFireTimeRemaining = 0;
        if (_grenadeTimeRemaining <= 0) _grenadeTimeRemaining = 0;

        if (Time.time > _consumeAmmoTimer)
        {
            _soldier._consumeAmmo = true;
            _soldier._fireRate = _soldier._baseFireRate;
        }
        if (Time.time > _grenadeTimer)
        {
            _canUseGrenade = true;
        }
        if (Time.time > _rapidFireTimer)
        {
            _soldier._canUseRF = true;
        }
    }

    /// <summary>
    /// Called by the TankInput script when 1 is pressed. Uses the shield plant ability.
    /// </summary>
    public void rapidFire()
    {
        // Don't consume ammo.
        _consumeAmmoTimer = Time.time + _rapidFireDuration;
        _soldier._consumeAmmo = false;

        // Increase fire rate.
        _soldier._fireRate = _rfFireRate;

        // Set timers
        _rapidFireTimer = Time.time + _rapidFireCD;
        _soldier._canUseRF = false;
        
    }

    /// <summary>
    /// Called by the TankInput script when 2 is pressed. Uses the ground pound ability.
    /// </summary>
    public void grenadeToss()
    {
        // Create a grenade and apply force
        GameObject grenade = Instantiate(_grenadePrefab, _grenadeStart.transform.position, _grenadeStart.transform.rotation);
        Rigidbody rb = grenade.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * _throwForce, ForceMode.VelocityChange);

        // Set timers
        _grenadeTimer = Time.time + _grenadeCD;
        _canUseGrenade = false;
    }
}
