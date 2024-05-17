using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatMonster : Monster
{
    [Header("Bat Monster Class")]
    [SerializeField]
    float timeBetweenAttacks, bulletSpeed;
    [SerializeField]
    int attackDamage;
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
            bulletSpawner.spawnBullet(Vector3.down, bulletSpeed, false, attackDamage);
        }
    }

    protected override void Attack()
    {

    }
}
