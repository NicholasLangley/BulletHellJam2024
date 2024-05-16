using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    struct futureBullet
    {
        public Vector3 direction;
        public float speed;
        public bool reflects;
        public int damage;
        public float delayTilNextBullet;

        public futureBullet(Vector3 direction, float speed, bool reflects, int damage, float delayTilNextBullet)
        {
            this.direction = direction;
            this.speed = speed;
            this.reflects = reflects;
            this.damage = damage;
            this.delayTilNextBullet = delayTilNextBullet;
        }
    }

    Queue<futureBullet> bulletsToFire;

    [SerializeField]
    Bullet bulletPrefab;

    public float bulletSpawnOffset = 0.2f;

    float bulletFireTimer;
    float bulletFireDelay;
    //float timer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        bulletsToFire = new Queue<futureBullet>();
        bulletFireTimer = 0.0f;
        bulletFireDelay = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //timer += Time.deltaTime;
        //if (timer > 1.0f) { spawnCrescentBurst(Vector3.down, 90.0f, 9, 10, true, 10); timer = 0.0f; }
        if(bulletsToFire.Count > 0)
        {
            bulletFireTimer += Time.deltaTime;
            if (bulletFireTimer > bulletFireDelay)
            {
                futureBullet next = bulletsToFire.Dequeue();
                spawnBullet(next.direction, next.speed, next.reflects, next.damage);
                bulletFireTimer = 0.0f;
                bulletFireDelay = next.delayTilNextBullet;
            }
        }
        if (Input.GetKeyDown(KeyCode.Return)) { spawnCrescentWave(Vector3.down, 90.0f, 9, 0.2f, 3, 10, true, 10); }
        if (Input.GetKeyDown(KeyCode.C)) { spawnCrescentBurst(Vector3.down, 90.0f, 9, 10, true, 10); }
    }

    //spawns a single bullet
    public void spawnBullet(Vector3 direction, float speed, bool reflects, int damage)
    {
        Bullet bullet = GameObject.Instantiate(bulletPrefab);
        bullet.Init(direction.normalized, speed, reflects, damage);
        Vector3 spawnPosition = transform.position + direction.normalized * bulletSpawnOffset;
        spawnPosition.z = -5;
        bullet.transform.position = spawnPosition;
        bullet.transform.SetParent(transform);
        bullet.gameObject.tag = "Bullet";
    }

    //sends a burst of bullets in an arc
    public void spawnCrescentBurst(Vector3 centerDir, float degrees, int bulletCount, float speed, bool reflects, int damage)
    {
        Vector3 angle = Quaternion.AngleAxis(-degrees / 2.0f, Vector3.forward) * centerDir;
        for (int i = 0; i < bulletCount; i++)
        {
            spawnBullet(angle, speed, reflects, damage);
            angle = Quaternion.AngleAxis(degrees / bulletCount, Vector3.forward) * angle;
        }
    }

    //send an arc of bullets one at a time. Can double back and forth
    public void spawnCrescentWave(Vector3 centerDir, float degrees, int bulletCount, float fireRate, int doubleBackCount, float speed, bool reflects, int damage)
    {
        Vector3 angle = Quaternion.AngleAxis(-degrees / 2.0f, Vector3.forward) * centerDir;
        for (int doubleback = 0; doubleback < doubleBackCount; doubleback++)
        {
            int dir = 1;
            if (doubleback % 2 == 1) { dir = -1; }
            for (int i = 0; i < bulletCount; i++)
            {
                bulletsToFire.Enqueue(new futureBullet(angle, speed, reflects, damage, fireRate));
                angle = Quaternion.AngleAxis(dir * degrees / bulletCount, Vector3.forward) * angle;
            }
        }
    }
}
