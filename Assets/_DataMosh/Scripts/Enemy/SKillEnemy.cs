using System;
using UnityEngine;

public class SKillEnemy : MonoBehaviour
{
    private Rigidbody _rigidBody;
    private SphereCollider _targetCollider;
    private CapsuleCollider _capsuleCollider;
    private SEnemyAI _enemyAI;
    private STargetController _targetController;
    private LayerMask _deadLayer;
    
    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _targetCollider = GetComponent<SphereCollider>();
        _capsuleCollider = GetComponentInChildren<CapsuleCollider>();
        _enemyAI = GetComponent<SEnemyAI>();
        _targetController = GameObject.FindGameObjectWithTag("Target Controller").GetComponent<STargetController>();
        _deadLayer = LayerMask.NameToLayer("ObjectMask");
    }

    public void KillEnemy()
    {
        Destroy(_rigidBody);
        _targetCollider.enabled = false;
        _enemyAI.enabled = false;
        this.gameObject.layer = _deadLayer;
        transform.GetChild(1).gameObject.tag = "Dead Enemy";
        _capsuleCollider.enabled = false;
        _targetController.RemoveTarget(this.gameObject);
    }
}
