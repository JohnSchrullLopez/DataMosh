using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class AnimateCircuits : MonoBehaviour
{
    private Material _material;
    private Color _emissiveColor;
    private float _accumulatedValue;
    private float _emissiveValue;
    [SerializeField] private float _intensity = 1.5f;
    [SerializeField] private float _speed = 1f;
    private float _offset;
    private int _emissionID;

    private void Awake()
    {
        _emissionID = Shader.PropertyToID("_EmissionColor");
        _material = GetComponent<MeshRenderer>().material;
        _emissiveColor = _material.GetColor(_emissionID);
        _offset = Random.Range(0f, 10f);
        _accumulatedValue = _offset;
    }

    private void Update()
    {
        _accumulatedValue += Time.deltaTime * _speed;
        _emissiveValue = 1 - (Mathf.Cos(_accumulatedValue) * _intensity);
        _material.SetColor(_emissionID, new Color(_emissiveColor.r + _emissiveValue, _emissiveColor.g + _emissiveValue, _emissiveColor.b + _emissiveValue));
    }
}
