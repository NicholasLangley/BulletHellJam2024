using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMonster : Monster
{
    [Header("Wall Monster Class")]
    [SerializeField]
    float timeBetweenAttacks, bulletSpeed;
    [SerializeField]
    int attackDamage;
    float attackTimer;

    [Header("Bullet Settings")]
    [SerializeField]
    float degrees;
    [SerializeField]
    float bulletFireRate;
    [SerializeField]
    int bulletCount, bulletDoubleBackCount;

    public bool rightSide;
    
    protected override void Awake()
    {
        base.Awake();
        attackTimer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer > timeBetweenAttacks)
        {
            attackTimer = 0.0f;
            Attack();
        }
    }

    protected override void Attack()
    {
        Vector3 fireDir = rightSide ? Vector3.down + Vector3.left : Vector3.down + Vector3.right;
        switch (level)
        {
            case 0:
                bulletSpawner.spawnCrescentWave(fireDir, 30, 3, 0.2f, 0, bulletSpeed, true, attackDamage);
                break;
            case 1:
                bulletSpawner.spawnCrescentWave(fireDir, 30, 4, 0.2f, 0, bulletSpeed, true, attackDamage);
                break;

            case 2:
                bulletSpawner.spawnCrescentWave(fireDir, 30, 5, 0.2f, 1, bulletSpeed * 1.25f, true, attackDamage);
                break;

            case 3:
                bulletSpawner.spawnCrescentWave(fireDir, 30, 5, 0.2f, 2, bulletSpeed * 1.5f, true, attackDamage);
                break;

            case 4:
                bulletSpawner.spawnCrescentWave(fireDir, 45, 7, 0.2f, 2, bulletSpeed * 1.5f, true, attackDamage);
                break;

            case 5:
                bulletSpawner.spawnCrescentWave(fireDir, 45, 10, 0.2f, 2, bulletSpeed * 1.5f, true, attackDamage);
                break;

            case 6:
                bulletSpawner.spawnCrescentWave(fireDir, 45, 10, 0.1f, 2, bulletSpeed * 1.75f, true, attackDamage);
                break;

            case 7:
                bulletSpawner.spawnCrescentWave(fireDir, 45, 10, 0.1f, 2, bulletSpeed * 2f, true, attackDamage);
                break;

            default:
                bulletSpawner.spawnCrescentWave(fireDir, 45, level + 3, 0.1f, (int)(level /2.0f ), bulletSpeed * (2f + level * 0.02f), true, attackDamage);
                break;
        }
    }


}
