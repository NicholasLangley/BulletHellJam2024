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

    AudioSource hitSound;
    public AudioSource killSound;

    [SerializeField]
    GameObject explosionPrefab;

    public int level;
    public float spawnTimer;
    public float yPos;
    public float startPosY;

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        bulletSpawner = gameObject.AddComponent(typeof(BulletSpawner)) as BulletSpawner;
        bulletSpawner.bulletPrefab = bulletPrefab;
        bulletSpawner.bulletSpawnOffset = bulletOffset;
        hitSound = GetComponent<AudioSource>();
        spawnTimer = 0.0f;
    }

    public void Damage(float damage)
    {
        health -= damage;
        hitSound.Play();
        if (health <= 0.00) { Kill(); }
    }

    public void Kill()
    {
        killSound.Play();
        GameObject explosion = GameObject.Instantiate(explosionPrefab);
        Vector3 expTrans = explosion.transform.position;
        expTrans.x = transform.position.x;
        expTrans.y = transform.position.y;
        explosion.transform.position = expTrans;
        GameObject.Destroy(gameObject);
    }

    protected abstract void Attack();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("RedirectZone"))
        {
            Damage(2);
        }
        if (collision.CompareTag("DestroyZone"))
        {
            Damage(200);
        }
    }

    public void setStartPos()
    {
        yPos = transform.position.y;
        Vector3 spawnPos = transform.position;
        spawnPos.y = 10;
        transform.position = spawnPos;
        startPosY = 10;
    }

}
