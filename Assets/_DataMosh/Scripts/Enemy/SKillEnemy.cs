using System;
using UnityEngine;

public class SKillEnemy : MonoBehaviour
{
    private Rigidbody _rigidBody;
    private SphereCollider _collider;
    private SEnemyAI _enemyAI;
    private STargetController _targetController;
    
    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _collider = GetComponent<SphereCollider>();
        _enemyAI = GetComponent<SEnemyAI>();
        _targetController = GameObject.FindGameObjectWithTag("Target Controller").GetComponent<STargetController>();
    }

    public void KillEnemy()
    {
        Destroy(_rigidBody);
        _collider.enabled = false;
        _enemyAI.enabled = false;
        this.gameObject.layer = LayerMask.NameToLayer("ObjectMask");
        transform.GetChild(1).gameObject.tag = "Dead Enemy";
        GetComponentInChildren<CapsuleCollider>().enabled = false;
        _targetController.RemoveTarget(this.gameObject);
    }
}
