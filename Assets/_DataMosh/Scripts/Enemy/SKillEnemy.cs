using System;
using Unity.VisualScripting;
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
        _enemyAI = GetComponent<SEnemyAI>();
        _targetController = GameObject.FindGameObjectWithTag("Target Controller").GetComponent<STargetController>();
        _deadLayer = LayerMask.NameToLayer("ObjectMask");
    }

    public void KillEnemy()
    {
        Destroy(_rigidBody);
        _targetCollider.enabled = false;
        _enemyAI.enabled = false;
        _targetController.RemoveTarget(this.gameObject);

        var children = transform.root.GetComponentsInChildren<Transform>();
        foreach (var child in children)
        {
            child.gameObject.layer = _deadLayer;
            child.tag = "Dead Enemy";
        }
    }
}
