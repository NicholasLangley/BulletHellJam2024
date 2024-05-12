using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    [SerializeField]
    Bullet bulletPrefab;

    public float bulletSpawnOffset = 0.2f;

    float timer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 1.0f) { spawnBullet(Vector3.down + Vector3.left, 10, true, 10); timer = 0.0f; }
    }

    public void spawnBullet(Vector3 direction, float speed, bool reflects, int damage)
    {
        Bullet bullet = GameObject.Instantiate(bulletPrefab);
        bullet.Init(direction, speed, reflects, damage);
        bullet.transform.position = transform.position + direction * bulletSpawnOffset;
        bullet.transform.SetParent(transform);
    }
}
