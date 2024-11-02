using System;
using UnityEngine;
using System.Collections.Generic;

public class SDetectAttackableEnemies : MonoBehaviour
{
    private SPlayerMovement _movementScript;
    private SPlayerInput _input;
    private SPlayerAnimationController _animController;
    private GameObject _attackableEnemy;

    private void Start()
    {
        _movementScript = GameObject.FindGameObjectWithTag("Player").GetComponent<SPlayerMovement>();
        _input = GameObject.Find("Player").GetComponent<SPlayerInput>();
    }

    private void Update()
    {
        if (_attackableEnemy == null) return;
        
        if (_input.Attacking && !_attackableEnemy.CompareTag("Dead Enemy"))
        {
            _movementScript.DashToTarget(_attackableEnemy.transform);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && _attackableEnemy == null)
        {
            _attackableEnemy = other.gameObject;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            _attackableEnemy = null;
        }
    }
}
