using System;
using UnityEngine;

public class SSwordColliderController : MonoBehaviour
{
    private Collider _collider;
    private void Awake()
    {
        _collider = GetComponent<CapsuleCollider>();
    }

    public void EnableCollider()
    {
        _collider.enabled = true;
    }

    public void DisableCollider()
    {
        _collider.enabled = false;
    }
}
