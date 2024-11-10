using System;
using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    private SDataMoshEffect _dataMosh;
    private void Awake()
    {
        _dataMosh = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<SDataMoshEffect>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && SGameManager.GetAllEnemiesDead())
        {
            _dataMosh.StartTransitionToEnd();
        }
    }
}
