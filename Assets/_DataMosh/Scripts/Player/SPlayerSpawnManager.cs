using System;
using UnityEngine;

public class SPlayerSpawnManager : MonoBehaviour
{
    public static SPlayerSpawnManager Instance;
    private static Transform _spawnTransform;
    private static Rigidbody _playerRigidbody;
    private static SDataMoshEffect _dmEffect;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this && Instance != null)
        {
            Destroy(gameObject);
        }
        
        _spawnTransform = GameObject.FindGameObjectWithTag("Respawn").transform;
        _playerRigidbody = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        _dmEffect = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<SDataMoshEffect>();
    }

    public void RespawnPlayer()
    {
        _playerRigidbody.position = _spawnTransform.position;
        _playerRigidbody.rotation = _spawnTransform.rotation;
        _dmEffect.SetFalling(false);
    }
}
