using System;
using System.Collections.Generic;
using UnityEngine;

public class STargetController : MonoBehaviour
{
    public List<GameObject> targets;
    [SerializeField] private float maxDistance = 50f;
    private float minDistance = .2f;
    private Transform _playerTransform;
    private GameObject _currentTarget;
    float minDistanceModifier;

    private void Start()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        minDistanceModifier = minDistance;
        minDistanceModifier = Mathf.Clamp(minDistance, 0, 1);
        _currentTarget = targets[0];
    }

    public float GetDistanceToTarget()
    {
        if (targets.Count > 0)
        {
            Vector3 distance = _playerTransform.position - _currentTarget.transform.position;
            return 1 - (Mathf.Clamp(((distance.magnitude) / maxDistance) - minDistanceModifier, 0, 1));
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
