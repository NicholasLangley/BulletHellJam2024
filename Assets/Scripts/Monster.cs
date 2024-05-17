using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : MonoBehaviour
{
    [Header ("Monster Base Class")]
    [SerializeField]
    public float health;
    [Header("Bullet Spawner Variables")]
    [SerializeField]
    Bullet bulletPrefab;
    [SerializeField]
    float bulletOffset;
    protected BulletSpawner bulletSpawner;

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        bulletSpawner = gameObject.AddComponent(typeof(BulletSpawner)) as BulletSpawner;
        bulletSpawner.bulletPrefab = bulletPrefab;
        bulletSpawner.bulletSpawnOffset = bulletOffset;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage(float damage)
    {
        health -= damage;
        if (health <= 0.00) { Kill(); }
    }

    public void Kill()
    {
        GameObject.Destroy(gameObject);
    }

    protected abstract void Attack();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("RedirectZone") || collision.CompareTag("DestroyZone"))
        {
            Damage(5);
        }
    }

}
