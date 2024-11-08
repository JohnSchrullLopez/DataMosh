using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class AnimateMonoliths : MonoBehaviour
{
    [SerializeField] private float _amplitude = 0.2f;
    [SerializeField] private float _offset = 0f;
    private float yChange = 0f;

    private void Awake()
    {
        _offset = Random.Range(0f, 10f);
        yChange = _offset;
    }

    private void Update()
    {
        yChange += Time.deltaTime;
        transform.position += new Vector3(0f, Mathf.Sin(yChange) * _amplitude, 0f);
    }
}
