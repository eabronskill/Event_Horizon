using TMPro;
using UnityEngine;

/// <summary>
/// Controls the Engineer's Turret. Calculates a target and rotates towards that target. Shoots every {_turretShotCD} seconds. 
/// </summary>
public class Turret : MonoBehaviour
{
    public GameObject _guns, _attackPoint, _projectile, _text;
    GameObject[] _enemies;
    GameObject _target;

    public float _turretShotCD = 1;
    public int _attackRange = 60;

    public float _aliveTime; // Gets set in the TechnicianAbilities script;
    float _shotTimer, _timeRemaining;

    public AudioSource _sound;

    Ray _ray;
    RaycastHit _hit;
    int _countdownNumber;

    void Start()
    {
        _guns.transform.rotation.SetLookRotation(_attackPoint.transform.forward);
        _timeRemaining = Time.time + _aliveTime;
    }

    void Update()
    {
        UpdateCountdownText();
        
        if (_target)
        {
            // Check if the Target is dead, out of range, or LOSed
            _ray = new Ray(_attackPoint.transform.position, (_target.transform.position - transform.position));
            if (Physics.Raycast(_ray, out _hit, _attackRange) && (_target.GetComponent<Enemy>().dead || _hit.collider.gameObject.CompareTag("Untagged")))
            {
                _target = null;
                return;
            }

            // Rotate the turret
            _guns.transform.LookAt(_target.transform.position);
            Vector3 rotatedVector = Quaternion.AngleAxis(-90, Vector3.up) * _guns.transform.forward;
            _guns.transform.forward = rotatedVector;

            // Shoot
            if (Time.time > _shotTimer)
            {
                Instantiate(_projectile, _attackPoint.transform.position, _attackPoint.transform.rotation);
                _shotTimer = Time.time + _turretShotCD;
                _sound.Play();
            }
        }
        else
        {
            // Find the closest, valid enemy.
            float closestSoFar = 10000000f;
            float distance;
            GameObject bestSoFar = null;
            _enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in _enemies)
            {
                _ray = new Ray(_attackPoint.transform.position, (enemy.transform.position - transform.position));
                distance = Vector3.Distance(enemy.transform.position, gameObject.transform.position);
                if (distance <= _attackRange)
                {
                    if (Physics.Raycast(_ray, out _hit, closestSoFar) && _hit.collider.gameObject.CompareTag("Enemy"))
                    {
                        bestSoFar = enemy;
                        closestSoFar = distance;
                    }
                }
            }
            _target = bestSoFar;
        }
    }

    void UpdateCountdownText()
    {
        _countdownNumber = (int)(_timeRemaining - Time.time);
        _text.GetComponent<TextMeshPro>().text = "" + _countdownNumber;
        if (_text.GetComponent<Billboard>().cam == null)
        {
            _text.GetComponent<Billboard>().cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
        }
    }

    public void Kill()
    {
        Destroy(gameObject);
    }
}
