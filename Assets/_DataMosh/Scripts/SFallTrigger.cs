using System;
using UnityEngine;

public class SFallTrigger : MonoBehaviour
{
    [SerializeField] private SDataMoshEffect _DataMoshManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _DataMoshManager.SetFalling(true);
        }
    }
}
