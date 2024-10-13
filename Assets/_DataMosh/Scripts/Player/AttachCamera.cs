using UnityEngine;

public class AttachCamera : MonoBehaviour
{
    private Transform _camLocation;

    void Awake()
    {
        _camLocation = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = _camLocation.position;
    }
}
