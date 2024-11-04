using System;
using UnityEngine;
using UnityEngine.Serialization;

public class SEnemyAI : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private float _shootCooldown = 1f;
    [SerializeField] private float _bulletForce = 100f;
    [SerializeField] private Transform _rotationPoint;
    private float _currentCooldown = 1f;
    private bool _playerInRange = false;
    private Transform _player;
    [SerializeField] private Transform _bulletSpawnPoint;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        _currentCooldown = _shootCooldown;
    }

    private void Update()
    {
        if (_playerInRange)
        {
            if (_currentCooldown > 0) _currentCooldown -= Time.deltaTime;
            TargetPlayer();
            Shoot();
        }
    }

    private void Shoot()
    {
        if (_currentCooldown <= 0)
        {
            GameObject bullet = BulletPool.Instance.SpawnBullet(_bulletSpawnPoint.position, _bulletSpawnPoint.rotation);

            if (bullet != null)
            {
                bullet.GetComponent<Rigidbody>().AddForce(_bulletForce * _bulletSpawnPoint.forward, ForceMode.Impulse);
                _currentCooldown = _shootCooldown;
            }
        }
    }

    private void TargetPlayer()
    {
        _rotationPoint.LookAt(_player, Vector3.up);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInRange = false;
            _currentCooldown = _shootCooldown;
        }
    }
}
