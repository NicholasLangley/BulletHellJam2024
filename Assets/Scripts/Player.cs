using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header ("Shared Player Variables")]
    public float maxEnergy = 100.0f;
    public float energy;
    public float energyRechargeRate = 10;

    public ResourceBar energyBar;

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        energyBar.setMaxValue(maxEnergy);
        energyBar.setValue(energy);
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
            increaseEnergy(energyRechargeRate * Time.deltaTime);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    public void decreaseEnergy(float e)
    {
        energy -= e;
        if (energy < 0) { energy = 0; }
        energyBar.setValue(energy);
    }

    public void increaseEnergy(float e)
    {
        energy += e;
        if (energy > maxEnergy) { energy = maxEnergy; }
        energyBar.setValue(energy);
    }

    public void setEnergyMax(float max)
    {
        maxEnergy = max;
        energyBar.setMaxValue(maxEnergy);
    }
}
