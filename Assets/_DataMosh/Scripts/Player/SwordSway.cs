using UnityEngine;
public class SwordSway : MonoBehaviour
{
    private Vector3 _velocity = Vector3.zero;
    [SerializeField] private float _smoothTime = .1f;
    [SerializeField] private Transform _target;
    private Vector3 _currentDamp;
    [SerializeField] private float speed = 150f;
    [SerializeField] private float _maxDistance = 0.2f;

    private void Update()
    {
        transform.position = Vector3.SlerpUnclamped(transform.position, _target.position, speed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, _target.rotation, speed * Time.deltaTime);
    }
}