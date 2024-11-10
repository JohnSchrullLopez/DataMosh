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
                Rigidbody rb = bullet.GetComponent<Rigidbody>();
                //rb.isKinematic = false;
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                return bullet;
            }
        }

        Debug.LogError("No bullet available to spawn");
        return null;
    }

    public void Despawn(GameObject bullet, Rigidbody rb)
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        bullet.SetActive(false);
    }
    
    /*private IEnumerator DespawnAfterSeconds(float seconds, GameObject bullet, Rigidbody rb)
    {
        yield return new WaitForSecondsRealtime(seconds);
        //rb.isKinematic = true;
        bullet.SetActive(false);
    }*/
}