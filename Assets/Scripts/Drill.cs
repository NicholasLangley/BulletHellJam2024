using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drill : MonoBehaviour
{
    public float health;
    public float maxHealth;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage(float dmg)
    {
        health -= dmg;
        if (health <= 0.0f) { Debug.Log("Game Over"); }
    }
    public void Repair(float heal)
    {
        health += heal;
        if (health >= maxHealth) { health = maxHealth; }
    }
}
