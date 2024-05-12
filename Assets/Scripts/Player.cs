using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float maxEnergy = 100.0f;
    public float energy;
    public float energyRechargeRate = 10;

    // Start is called before the first frame update
    void Start()
    {
        energy = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Drill"))
        {
            StartCoroutine("Recharge");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Drill"))
        {
            StopCoroutine("Recharge");
        }
    }

    IEnumerator Recharge()
    {
        while (true)
        {
            energy += energyRechargeRate * Time.deltaTime;
            if (energy > maxEnergy) { energy = maxEnergy; }
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}
