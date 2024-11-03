using System;
using System.Collections.Generic;
using UnityEngine;

public class STargetController : MonoBehaviour
{
    public List<GameObject> targets;
    [SerializeField] private float maxDistance = 50f;
    [SerializeField] private float minDistance = 10f;
    private Transform _playerTransform;
    private GameObject _currentTarget;

    private void Start()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        minDistance = minDistance / maxDistance;
        _currentTarget = targets[0];
    }

    public float GetDistanceToTarget()
    {
        if (targets.Count > 0)
        {
            Vector3 distance = _playerTransform.position - _currentTarget.transform.position;
            return 1 - (Mathf.Clamp((distance.magnitude / maxDistance) - minDistance, 0, 1));
        }
        else
        {
            return 0;
        }
    }

    public void RemoveTarget(GameObject target)
    {
        targets.Remove(target);
        if (targets.Count > 0)
        {
            _currentTarget = targets[0];
        }
    }

    public GameObject GetCurrentTarget()
    {
        return _currentTarget;
    }
}
