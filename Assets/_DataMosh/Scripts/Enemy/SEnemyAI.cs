using System;
using UnityEngine;
using UnityEngine.Serialization;

public class SEnemyAI : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private float _shootCooldown = 1f;
    [SerializeField] private float _bulletForce = 100f;
    private bool _playerInRange = false;
    private Transform _player;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void Update()
    {
        Debug.Log(_shootCooldown);

        if (_shootCooldown > 0) _shootCooldown -= Time.deltaTime;
        
        if (_playerInRange)
        {
            TargetPlayer();
            Shoot();
        }
    }

    private void Shoot()
    {
        if (_shootCooldown <= 0)
        {
            GameObject bullet = Instantiate(_bulletPrefab, transform.position, transform.rotation);
            bullet.GetComponent<Rigidbody>().AddForce(_bulletForce * transform.forward, ForceMode.Impulse);
            _shootCooldown = 5f;
        }
    }

    private void TargetPlayer()
    {
        transform.LookAt(_player);
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
        }
    }
}
