using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drill : MonoBehaviour
{
    public float health = 100;
    public float maxHealth = 100;

    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage(int dmg)
    {
        health -= dmg;
        if (health <= 0.0f) { Debug.Log("Game Over"); }
    }
    public void Repair(int heal)
    {
        health += heal;
        if (health >= maxHealth) { health = maxHealth; }
    }
}
