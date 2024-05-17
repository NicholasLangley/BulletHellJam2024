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
        bulletSpawner.spawnCrescentWave(Vector3.down + Vector3.right, degrees, bulletCount, bulletFireRate, bulletDoubleBackCount, bulletSpeed, true, attackDamage);
    }


}
