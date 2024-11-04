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
        if (other.CompareTag("Enemy") && other.transform.root.gameObject == _targetController.GetCurrentTarget())
        {
            other.transform.root.GetComponent<SKillEnemy>().KillEnemy();
            other.enabled = false;
        }
    }
}
