using UnityEngine;
public class SwordSway : MonoBehaviour
{
    private Vector3 _velocity = Vector3.zero;
    [SerializeField] private float _smoothTime = .2f;
    [SerializeField] private Transform _target;
    private Vector3 _currentDamp;
    [SerializeField] private float speed = 150f;

    private void Update()
    {
        /*transform.position = Vector3.SmoothDamp(transform.position, _target.TransformPoint(Vector3.zero), ref _velocity, _smoothTime); ;
        transform.rotation = Quaternion.Slerp(transform.rotation, _target.rotation, speed * Time.deltaTime);*/

        transform.position = Vector3.SlerpUnclamped(transform.position, _target.position, speed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, _target.rotation, speed * Time.deltaTime);
    }
}
