using System;
using UnityEngine;

public class DetectHitEnemies : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy") Debug.Log(other.gameObject.tag);
    }
}
