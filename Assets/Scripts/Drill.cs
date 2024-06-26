using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drill : MonoBehaviour
{
    public float health = 100;
    public float maxHealth = 100;

    public ResourceBar healthBar;

    [SerializeField]
    GameController gc;

    [SerializeField]
    GameObject bigExplosion;

    // Start is called before the first frame update
    void Awake()
    {
        healthBar.setMaxValue(maxHealth);
        healthBar.setValue(health);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage(float dmg)
    {
        if (health > 0) {
            health -= dmg;
            healthBar.setValue(health);
            if (health <= 0.0f) { gc.endGame(); GameObject.Instantiate(bigExplosion); }
        }
    }
    public void Repair(int heal)
    {
        health += heal;
        healthBar.setValue(health);
        if (health >= maxHealth) { health = maxHealth; }
    }
}
