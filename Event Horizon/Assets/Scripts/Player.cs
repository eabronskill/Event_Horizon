using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Base class for all characters. Contains generic logic for the HUD, Movement, Rotation, Reloading, Melee, Shooting, and Item Interaction.
/// Also includes other helper functions used by enemies and triggers.
/// </summary>
public class Player : MonoBehaviour
{
    // Rewired Vars
    public Rewired.Player _player; // Set by Input Scripts. Needs to be public for triggers.

    /// <summary>
    /// ID of the player who is controlling this character.
    /// </summary>
    [HideInInspector] public int _playerID = -1;

    // Stat vars
    public float _curMoveSpeed;
    public float _movementSpeed;
    public float _curHealth;
    public float _maxHealth;
    public float _curClip;
    public float _maxClip;
    public float _curAmmo;
    public float _maxAmmo;
    [HideInInspector] public bool _dead;
    protected bool _canShoot = true;

    // Attacking vars
    public GameObject _attackPoint;
    public GameObject _bulletPrefab;
    public float _fireRate;
    protected float _strapTimer = 0;

    // Animation Vars
    public Animator _playerAnimator;

    // Aiming vars
    public Collider _rifleCol;
    Plane _mousePlane;
    Ray _cameraRay;
    float _intersectionDistance = 0f;

    // Item vars
    [HideInInspector] public Ammo _ammoItem;
    [HideInInspector] public Healing _healingItem;
    [HideInInspector] public bool _hasAmmo = false;
    [HideInInspector] public bool _hasHealing = false;
    float _itemBufferTimer;
    float _itemBufferCD = 0.5f;
    bool _cantUse = false;

    // Canvas Vars
    public GameObject _gameOverCanvas;
    public GameObject _pauseMenu;
    public GameObject _victoryCanvas;
    GameObject _controlsMenu;
    bool _gameOver = false;

    //Movement Vars
    protected CharacterController _controller;

    // Tutorial Vars
    [HideInInspector] public bool _moved, _shot, _meleed, _usedAbility1, _usedAbility2;

    // Sounds
    public AudioSource _gunshot;
    public AudioSource _setItem;
    public AudioSource _melee;

    public void Update()
    {
        if (_curHealth <= 0) Die();
        HUDLogic();

        if (Time.timeScale != 1) return; // Do not want to take any input while game is paused or over.

        MovementInput();
        RotationInput();
        ReloadInput();
        MeleeInput();
        ItemInteractionInput();
    }

    void HUDLogic()
    {
        // Game is Running
        if (!_gameOver)
        {
            if (!_controlsMenu) _controlsMenu = _pauseMenu.GetComponent<PauseMenuController>().ControlsMenu;
            if (_player.GetButtonDown("Menu"))
            {
                if (_controlsMenu.activeSelf)
                {
                    _controlsMenu.SetActive(false);
                    return;
                }

                if (_pauseMenu.activeSelf)
                {
                    Time.timeScale = 1;
                    _pauseMenu.SetActive(false);
                    _pauseMenu.GetComponent<PauseMenuController>().setPlayer(null);
                }
                else
                {
                    Time.timeScale = 0;
                    _pauseMenu.SetActive(true);
                    _pauseMenu.GetComponent<PauseMenuController>().setPlayer(_player);
                }
            }
        }

        // Players Lost
        if (_gameOverCanvas.activeSelf) // && player.controllers.joystickCount >= 1
        {
            _gameOver = true;
            if ((_player.GetButtonDown("Interact") || Input.GetKeyDown(KeyCode.E))) //&& player.controllers.joystickCount >= 1
            {
                MainMenuController._controllerIDToPlayerID = new System.Collections.Generic.Dictionary<int, int>();
                ChS_Model._idToCharacter = new System.Collections.Generic.Dictionary<int, ChS_Model.Character>();
                ChS_Controller._finalSelection = new System.Collections.Generic.Dictionary<string, int>();
                ChS_Controller._singlePlayer = false;
                UIEventCOntroller.players = new System.Collections.Generic.Dictionary<string, GameObject>();
                Time.timeScale = 1;
                SceneManager.LoadScene("MainMenu");
            }
        }
        
        // Players Won
        if ((_player.GetButtonDown("Interact") || Input.GetKeyDown(KeyCode.E)) && _victoryCanvas.activeSelf)
        {
            Time.timeScale = 1;
            if (SceneManager.GetActiveScene().name.Equals("Level1"))
            {
                UIEventCOntroller.players = new System.Collections.Generic.Dictionary<string, GameObject>();
                SceneManager.LoadScene("Level2");
            }
            else
            {
                UIEventCOntroller.players = new System.Collections.Generic.Dictionary<string, GameObject>();
                MainMenuController._controllerIDToPlayerID = new System.Collections.Generic.Dictionary<int, int>();
                ChS_Model._idToCharacter = new System.Collections.Generic.Dictionary<int, ChS_Model.Character>();
                ChS_Controller._finalSelection = new System.Collections.Generic.Dictionary<string, int>();
                ChS_Controller._singlePlayer = false;
                SceneManager.LoadScene("MainMenu");
            }
        }
    }

    void MovementInput()
    {
        float x = _player.GetAxis("Move Horizontal");
        float z = _player.GetAxis("Move Vertical");

        Vector3 movementVec = new Vector3(x, 0f, z);
        if (x != 0 && z != 0)
        {
            _moved = true;
        }

        _controller.Move(movementVec * _curMoveSpeed * Time.deltaTime);

        // Movement Anims
        if (_playerAnimator != null)
        {
            if (movementVec.z >= 0.3f || movementVec.x <= -0.3f || movementVec.x >= 0.3f || movementVec.z <= -0.3f)
            {
                _playerAnimator.SetFloat("Blend", 1.0f);
                _playerAnimator.SetBool("Running", true);
            }
            else
            {
                _playerAnimator.SetFloat("Blend", 0.0f);
                _playerAnimator.SetBool("Running", false);
            }
        }
    }

    void RotationInput()
    {
        if (_player.controllers.joystickCount >= 1)
        {
            Vector3 rotateVec = new Vector3(0, Mathf.Atan2(_player.GetAxis("Rotate Horizontal"), _player.GetAxis("Rotate Vertical")) * 180 / Mathf.PI, 0);
            if (_player.GetAxis("Rotate Horizontal") != 0 || _player.GetAxis("Rotate Vertical") != 0)
            {
                transform.eulerAngles = rotateVec;
            }
        }
        else
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 1000))
            {
                transform.LookAt(new Vector3(hit.point.x, transform.position.y, hit.point.z));
            }
        }
    }

    void ReloadInput()
    {
        if ((_curClip != _maxClip && _curAmmo != 0) && _player.GetButtonDown("Reload"))
        {
            if (_curAmmo < (_maxClip - _curClip))
            {
                _curClip += _curAmmo;
                _curAmmo = 0;
            }
            else
            {
                _curAmmo -= (_maxClip - _curClip);
                _curClip = _maxClip;
            }
        }
    }

    void MeleeInput()
    {
        if (_player.GetButtonDown("Melee"))
        {
            _playerAnimator.SetTrigger("Melee");
            _rifleCol.enabled = true;
            _canShoot = false;
            Invoke(nameof(DisableRifleCol), 1.5f);
            _meleed = true;
        }
    }

    /// <summary>
    /// Basic shooting. Override for Soldier and Engineer
    /// </summary>
    protected virtual void ShootingInput()
    {
        if (!_canShoot) return;

        if (_player.GetButton("Shoot") && Time.time >= _strapTimer && _curClip > 0)
        {
            _strapTimer = Time.time + _fireRate;
            Instantiate(_bulletPrefab, _attackPoint.transform.position, _attackPoint.transform.rotation);
            _shot = true;
            _gunshot.Play();
            _curClip--;

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

    void ItemInteractionInput()
    {
        if (_itemBufferTimer > Time.time) return; // Prevents unwanted spawm use/pickup.
        if (_cantUse) return; // Player is standing on another item
        // Ammo
        if (_hasAmmo && (_player.GetButtonDown("Interact") || Input.GetKeyDown(KeyCode.E)))
        {
            _ammoItem.use();
            _hasAmmo = false;
        }
        else if (_hasAmmo && (_player.GetButtonDown("DropItem") || Input.GetKeyDown(KeyCode.B)))
        {
            _ammoItem.gameObject.SetActive(true);
            _ammoItem.transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
            _ammoItem.player = null;
            _ammoItem = null;
            _hasAmmo = false;
        }
        // Healing
        if (_hasHealing && (_player.GetButtonDown("Interact") || Input.GetKeyDown(KeyCode.E)))
        {
            _healingItem.use();
            _hasHealing = false;
        }
        else if (_hasHealing && (_player.GetButtonDown("DropItem") || Input.GetKeyDown(KeyCode.B)))
        {
            _healingItem.gameObject.SetActive(true);
            _healingItem.transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
            _healingItem.player = null;
            _healingItem = null;
            _hasHealing = false;
        }
    }

    void Die()
    {
        _dead = true;
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Called in an Invoke. Ignore C#'s insistence that it has no references.
    /// </summary>
    void DisableRifleCol()
    {
        _rifleCol.enabled = false;
        _canShoot = true;
    }

    /// <summary>
    /// Called by the enemy script to cause damage to this player.
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(float damage)
    {
        _curHealth = _curHealth - damage;
        if(_curHealth < 0)
        {
            _curHealth = 0;
        }
    }

    /// <summary>
    /// Used for item pickup.
    /// </summary>
    /// <param name="other"></param>
    protected void OnTriggerStay(Collider other)
    {
        if (_itemBufferTimer > Time.time) return; // Prevents unwanted spam use/pickup.
        if (other.CompareTag("Ammo"))
        {
            _cantUse = true;
            if ((_player.GetButtonDown("Interact") || Input.GetKeyDown(KeyCode.E)) && !(_hasAmmo || _hasHealing))
            {
                _ammoItem = other.GetComponent<Ammo>();
                _ammoItem.player = this;
                _ammoItem.gameObject.SetActive(false);
                _hasAmmo = true;
                _itemBufferTimer = Time.time + _itemBufferCD;
                _cantUse = false;
            }
            else if ((_player.GetButtonDown("Interact") || Input.GetKeyDown(KeyCode.E)) && _hasHealing)
            {
                _healingItem.gameObject.SetActive(true);
                _healingItem.transform.position = new Vector3(transform.position.x, transform.position.y + 5f, transform.position.z);
                _healingItem.player = null;
                _healingItem = null;
                _hasHealing = false;


                _ammoItem = other.GetComponent<Ammo>();
                _ammoItem.player = this;
                _ammoItem.gameObject.SetActive(false);
                _hasAmmo = true;
                _itemBufferTimer = Time.time + _itemBufferCD;
                _cantUse = false;
            }

        }
        if (other.CompareTag("Healing"))
        {
            _cantUse = true;
            if ((_player.GetButtonDown("Interact") || Input.GetKeyDown(KeyCode.E)) && !(_hasAmmo || _hasHealing))
            {
                _healingItem = other.GetComponent<Healing>();
                _healingItem.player = this;
                _healingItem.gameObject.SetActive(false);
                _hasHealing = true;
                _itemBufferTimer = Time.time + _itemBufferCD;
                _cantUse = false;
            }
            else if ((_player.GetButtonDown("Interact") || Input.GetKeyDown(KeyCode.E)) && _hasAmmo)
            {
                _ammoItem.gameObject.SetActive(true);
                _ammoItem.transform.position = new Vector3(transform.position.x, transform.position.y + 5f, transform.position.z);
                _ammoItem.player = null;
                _ammoItem = null;
                _hasAmmo = false;

                _healingItem = other.GetComponent<Healing>();
                _healingItem.player = this;
                _healingItem.gameObject.SetActive(false);
                _hasHealing = true;
                _itemBufferTimer = Time.time + _itemBufferCD;
                _cantUse = false;
            }
        }
    }

    /// <summary>
    /// Used for item use.
    /// </summary>
    /// <param name="other"></param>
    protected void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ammo") || other.CompareTag("Healing"))
        {
            print("Exit");
            _cantUse = false;
        }
    }

    /// <summary>
    /// Used to sense enemy bullets.
    /// </summary>
    /// <param name="coll"></param>
    protected void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "Enemy Bullet")
        {
            TakeDamage(30);
        }
    }

    // Must be implemented by Input classes.
    public virtual void ResetAbilities() { }
}
