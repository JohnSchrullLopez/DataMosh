using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Windows;

public class AttachCamera : MonoBehaviour
{
    private Transform _camDefaultLocation;
    private Transform _camSlideLocation;
    private Transform _followPosition;
    [SerializeField] private float _followSpeed = 10f;

    void Awake()
    {
        _camDefaultLocation = GameObject.FindGameObjectWithTag("Player").transform.GetChild(1).GetComponent<Transform>();
        _camSlideLocation = GameObject.FindGameObjectWithTag("Player").transform.GetChild(2).GetComponent<Transform>();
    }

    void LateUpdate()
    {
        //Snap to x and z components of follow position and lerp to y component
        float currentY = Mathf.Lerp(transform.position.y, _followPosition.position.y, Time.deltaTime * _followSpeed);
        Vector3 currentLocation = new Vector3(_followPosition.position.x, currentY, _followPosition.position.z);
        transform.position = currentLocation;
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
