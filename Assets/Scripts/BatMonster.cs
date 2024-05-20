using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatMonster : Monster
{
    [Header("Bat Monster Class")]
    [SerializeField]
    float timeBetweenAttacks, bulletSpeed;
    [SerializeField]
    float attackDamage;
    float attackTimer;

    float yPos;

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        attackTimer = 0.0f;
        yPos = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 nextPos = new Vector3(transform.localPosition.x, yPos + 0.1f * Mathf.Sin(Mathf.PI * Time.time) - 0.3f, transform.localPosition.z);
        transform.localPosition = nextPos;

        attackTimer += Time.deltaTime;
        if(attackTimer > timeBetweenAttacks)
        {
            attackTimer = 0.0f;
            Attack();
        }
    }

    protected override void Attack()
    {
        switch(level)
        {
            case 0:
                bulletSpawner.spawnBullet(Vector3.down, bulletSpeed, false, attackDamage);
                break;
            case 1:
                bulletSpawner.spawnCrescentBurst(Vector3.down, 30.0f, 3, bulletSpeed, true, attackDamage);
                break;

            case 2:
                bulletSpawner.spawnCrescentBurst(Vector3.down, 30.0f, 5, bulletSpeed * 1.25f, true, attackDamage);
                break;

            case 3:
                bulletSpawner.spawnCrescentBurst(Vector3.down, 30.0f, 5, bulletSpeed * 1.5f, true, attackDamage);
                break;

            case 4:
                bulletSpawner.spawnCrescentBurst(Vector3.down, 45f, 7, bulletSpeed * 1.5f, true, attackDamage);
                break;

            case 5:
                bulletSpawner.spawnCrescentBurst(Vector3.down, 45.0f, 10, bulletSpeed * 1.5f, true, attackDamage);
                break;

            case 6:
                bulletSpawner.spawnCrescentBurst(Vector3.down, 45.0f, 10, bulletSpeed * 1.75f, true, attackDamage);
                break;

            case 7:
                bulletSpawner.spawnCrescentBurst(Vector3.down, 45.0f, 10, bulletSpeed * 2f, true, attackDamage);
                break;

            default:
                bulletSpawner.spawnCrescentBurst(Vector3.down, 45.0f, 10 + (int)(level / 2), bulletSpeed * (2f + 0.01f * level), true, attackDamage);
                break;
        }
        
    }
}
