using System;
using UnityEngine;

public class DetectHitEnemies : MonoBehaviour
{
    private STargetController _targetController;

    private void Awake()
    {
        _targetController = GameObject.FindGameObjectWithTag("Target Controller").GetComponent<STargetController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && other.transform.parent.gameObject == _targetController.GetCurrentTarget())
        {
            other.transform.parent.GetComponent<SKillEnemy>().KillEnemy();
        }
    }
}
