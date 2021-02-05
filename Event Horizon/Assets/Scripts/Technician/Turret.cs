using TMPro;
using UnityEngine;

/// <summary>
/// Controls the Engineer's Turret. Calculates a target and rotates towards that target. Shoots every {_turretShotCD} seconds. 
/// </summary>
public class Turret : MonoBehaviour
{
    public GameObject _guns, _attackPoint, _projectile, _text;
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
        print("alive");
        if (_timeRemaining <= 0) Destroy(gameObject);

        UpdateCountdownText();
        
        if (_target)
        {
            print(_target);
            // Check if the Target is dead, out of range, or LOSed
            _ray = new Ray(_attackPoint.transform.position, _target.transform.position - transform.position);
            if (!_target.GetComponent<Enemy>())
            {
                _target = null;
                return;
            }
            Physics.Raycast(_ray, out _hit, _attackRange);
            if (_hit.transform == null || _target.GetComponent<Enemy>().dead || _hit.collider.gameObject.CompareTag("Untagged"))
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
            foreach (Collider enemy in Physics.OverlapSphere(transform.position, _attackRange))
            {
                if (!enemy.gameObject.CompareTag("Enemy")) continue;

                _ray = new Ray(_attackPoint.transform.position, enemy.gameObject.transform.position - transform.position);
                distance = Vector3.Distance(enemy.gameObject.transform.position, gameObject.transform.position);
                if (distance <= _attackRange)
                {
                    if (Physics.Raycast(_ray, out _hit, closestSoFar) && _hit.collider.gameObject.CompareTag("Enemy"))
                    {
                        bestSoFar = enemy.gameObject;
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

    public void InvokeKill()
    {
        Invoke(nameof(Kill), 0f);
    }

    public void Kill()
    {
        Destroy(gameObject);
    }
}
