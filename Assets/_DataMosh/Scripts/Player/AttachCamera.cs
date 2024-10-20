using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Windows;

public class AttachCamera : MonoBehaviour
{
    private Transform _camDefaultLocation;
    private Transform _camSlideLocation;
    private Transform _followPosition;

    void Awake()
    {
        _camDefaultLocation = GameObject.FindGameObjectWithTag("Player").transform.GetChild(1).GetComponent<Transform>();
        _camSlideLocation = GameObject.FindGameObjectWithTag("Player").transform.GetChild(2).GetComponent<Transform>();
    }

    void Update()
    {
        transform.position = _followPosition.position;
    }

    public void MoveToSlidePosition()
    {
        _followPosition = _camSlideLocation;
    }

    public void MoveToDefaultPosition()
    {
        _followPosition = _camDefaultLocation;
    }
}
