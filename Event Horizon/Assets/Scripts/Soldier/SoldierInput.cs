using UnityEngine;
using UnityEngine.SceneManagement;
using Rewired;

/// <summary>
/// Handles the Soldier specific inputs.
/// </summary>
public class SoldierInput : Player
{
    private SoldierAbilities _abilities;

    // Ability vars
    [HideInInspector] public float _baseFireRate;
    [HideInInspector] public bool _consumeAmmo = true;
    [HideInInspector] public bool _canUseGrenade = true;
    [HideInInspector] public bool _canUseRF = true;

    // For HUD
    [HideInInspector] public float _rapidFireCD, _grenadeCD;

    // Sounds
    public AudioSource _powerup;

    void Awake()
    {
        _curAmmo = _maxAmmo;
        _curClip = _maxClip;
        _curHealth = _maxHealth;
        _curMoveSpeed = _movementSpeed;
    }
    
    void Start() => Initialize();

    new void Update()
    {
        // Call the Player Update
        base.Update();

        if (Time.timeScale != 1) return;

        _rapidFireCD = _abilities._rapidFireTimeRemaining;
        _grenadeCD = _abilities._grenadeTimeRemaining;

        ShootingInput();

        // Ability 1: Rapid Fire
        if (_player.GetButton("Ability1") && _canUseRF)
        {
            _usedAbility1 = true;
            _abilities.rapidFire();
            _powerup.Play();
            
        }

        // Ability 2: Grenade 
        if (_player.GetButtonDown("Ability2") && _abilities._canUseGrenade)
        {
            _playerAnimator.SetTrigger("Grenade");
            _abilities.grenadeToss();
            _usedAbility2 = true;
        }
    }

    void Initialize()
    {
        if (ChS_Controller._finalSelection.ContainsKey("Soldier Icon"))
        {
            _player = ReInput.players.GetPlayer(ChS_Controller._finalSelection["Soldier Icon"]);
            MultipleTargetCamera.targets.Add(gameObject);

            _playerID = _player.id;
            _controller = GetComponent<CharacterController>();
            if (UIEventCOntroller.players.Count == 0)
            {
                UIEventCOntroller.players.Add("Soldier", gameObject);
            }
            if (SceneManager.GetActiveScene().name == "Level1")
            {
                Tutotrial.players.Add(gameObject);
            }
            _baseFireRate = _fireRate;
            _abilities = gameObject.GetComponent<SoldierAbilities>();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Soldier specific shooting logic.
    /// </summary>
    protected override void ShootingInput()
    {
        if (!_canShoot) return;

        if (_player.GetButton("Shoot") && Time.time >= _strapTimer && _curClip > 0)
        {
            _strapTimer = Time.time + _fireRate;
            _shot = true;
            Instantiate(_bulletPrefab, _attackPoint.transform.position, _attackPoint.transform.rotation);

            if (_consumeAmmo)
            {
                _curClip--;
                _gunshot.Play();
            }
        }
        if (_player.GetButton("Shoot") && _curClip > 0)
        {
            _curMoveSpeed = _movementSpeed * 0.5f;
        }
        else
        {
            _curMoveSpeed = _movementSpeed;
        }
    }

    new void OnTriggerStay(Collider other)
    {
        base.OnTriggerStay(other);
    }

    /// <summary>
    /// Used by the enemy script to cause damage to this player.
    /// </summary>
    /// <param name="damage"></param>
    public new void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
    }

    new private void OnCollisionEnter(Collision other)
    {
        base.OnCollisionEnter(other);
    }

    public override void ResetAbilities()
    {
        _abilities._grenadeTimer = 0;
        _abilities._rapidFireTimer = 0;
    }
    
}
