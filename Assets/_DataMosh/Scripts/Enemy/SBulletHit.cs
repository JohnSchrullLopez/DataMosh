using System;
using UnityEngine;

public class SBulletHit : MonoBehaviour
{
    [SerializeField] private float _despawnTime = 20f;
    private float _timer = 0f;
    private Rigidbody _rb;
    private SDataMoshEffect _dataMosh;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _dataMosh = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<SDataMoshEffect>();
    }

    private void OnEnable()
    {
        _timer = 0f;
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= _despawnTime)
        {
            BulletPool.Instance.Despawn(this.gameObject, _rb);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //SPlayerSpawnManager.Instance.RespawnPlayer();
            _dataMosh.RespawnWithEffect();
            BulletPool.Instance.Despawn(this.gameObject, _rb);
        }
    }
}
