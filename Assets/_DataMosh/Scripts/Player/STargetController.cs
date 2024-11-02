using System;
using System.Collections.Generic;
using UnityEngine;

public class STargetController : MonoBehaviour
{
    public List<GameObject> targets;
    [SerializeField] private float maxDistance = 50f;
    [SerializeField] private float minDistance = 10f;
    private Transform _playerTransform;

    private void Start()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        minDistance = minDistance / maxDistance;
    }

    private void Update()
    {
        Debug.Log(GetDistanceToTarget());
    }

    public float GetDistanceToTarget()
    {
        if (targets.Count > 0)
        {
            Vector3 distance = _playerTransform.position - targets[0].transform.position;
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
    }
}
