using System.Collections;
using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class BulletPool : MonoBehaviour
{
    public GameObject BulletPrefab;
    public static List<GameObject> Pool;
    public int TotalBullets = 20;
    public static BulletPool Instance;
    [SerializeField] private float _despawnTime = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        GameObject bullet;
        Pool = new List<GameObject>();
        
        for (int i = 0; i < TotalBullets; i++)
        {
            bullet = Instantiate(BulletPrefab, transform);
            Pool.Add(bullet);
            bullet.SetActive(false);
        }
    }

    public GameObject SpawnBullet(Vector3 position, Quaternion rotation)
    {
        foreach (GameObject bullet in Pool)
        {
            if (!bullet.activeInHierarchy)
            {
                bullet.SetActive(true);
                bullet.transform.position = position;
                bullet.transform.rotation = rotation;
                StartCoroutine(DespawnAfterSeconds(_despawnTime, bullet));
                Debug.Log("Despawn timer started");
                return bullet;
            }
        }

        Debug.LogError("No bullet available to spawn");
        return null;
    }

    private IEnumerator DespawnAfterSeconds(float seconds, GameObject bullet)
    {
        yield return new WaitForSecondsRealtime(seconds);
        bullet.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        bullet.SetActive(false);
        Debug.Log("Despawn timer ended");
    }
}