using UnityEngine;
using UnityEngine.SceneManagement;
using Rewired;

/// <summary>
/// Handles the Tank specific inputs.
/// </summary>
public class TankInput : Player
{
    private TankAbilities _abilities;

    // Ability vars
    [HideInInspector] public bool _canPound = true;
    public GameObject _shieldText;
    bool _canPlaceShield = true;

    // For HUD
    [HideInInspector] public float _shieldCD, _groundPoundCD;
    [HideInInspector] public bool _shieldDown;

    public ParticleSystem _particleGroundPound;

    // Sounds
    public AudioSource _shockwave;

    void Awake()
    {
        _curAmmo = _maxAmmo;
        _curClip = _maxClip;
        _curHealth = _maxHealth;
        _curMoveSpeed = _movementSpeed;
    }

    void Start()
    {
        Initialize();     
    }

    new void Update()
    {
        // Call the Player Update method.
        base.Update();

        if (Time.timeScale != 1) return;

        _shieldCD = _abilities._shieldTimeRemaining;
        _groundPoundCD = _abilities._shockwaveTimeRemaining;
        _shieldDown = _abilities._shieldDown;

        ShootingInput();
        
        // Ability 1: Shield Plant
        if (_canPlaceShield && _player.GetButtonDown("Ability1"))
        {
            _usedAbility1 = true;
            _abilities.ShieldPlant();
            _setItem.Play();
            _canPlaceShield = false;
            
        }

        // Ability 2: Ground Pound
        if (_player.GetButtonDown("Ability2") && _canPound)
        {
            _usedAbility2 = true;
            _abilities.Shockwave();
            _shockwave.Play();
            _particleGroundPound.Play();   
        }
    }

    void Initialize()
    {
        if (CharacterSelectController._finalSelection.ContainsKey("Tank Icon"))
        {
            _player = ReInput.players.GetPlayer(CharacterSelectController._finalSelection["Tank Icon"]);
            MultipleTargetCamera.targets.Add(this.gameObject);

            _playerID = _player.id;
            _controller = GetComponent<CharacterController>();
            if (UIEventCOntroller.players.Count == 0)
            {
                UIEventCOntroller.players.Add("Tank", this.gameObject);
            }
            if (SceneManager.GetActiveScene().name == "Level1")
            {
                Tutotrial.players.Add(this.gameObject);
            }

            _abilities = gameObject.GetComponent<TankAbilities>();
            _shieldText.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
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
            _shieldText.SetActive(true);
            // Pickup the shield.
            if (_player.GetButton("Interact"))
            {
                _canPlaceShield = true;
                _abilities.PickupShield();
                _setItem.Play();
            }
        }
    }

    /// <summary>
    /// Called when the Tank leaves the radius of an item or his shield.
    /// </summary>
    /// <param name="other"></param>
    new private void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
        if (other.gameObject.CompareTag("Shield"))
        {
            _shieldText.SetActive(false);
        }
    }

    /// <summary>
    /// Required for Tank Shield text to appear and dissapear.
    /// </summary>
    /// <param name="other"></param>
    new private void OnCollisionEnter(Collision other)
    {
        base.OnCollisionEnter(other);
    }

    public override void ResetAbilities()
    {
        _abilities._shieldTimer = 0;
        _abilities._shockwaveTimer = 0;
    }
}
