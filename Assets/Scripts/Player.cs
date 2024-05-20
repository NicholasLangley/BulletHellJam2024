using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : MonoBehaviour
{
    [Header("Shared Player Variables")]
    public float maxEnergy = 100.0f;
    public float energy = 50;
    public float energyRechargeRate = 10;

    public ResourceBar energyBar;

    [Header("Bullet Push variables")]
    [SerializeField]
    protected float pushDuration;
    [SerializeField]
    protected float pushStrength;
    protected Vector3 pushDir;
    protected float pushTimer = 0.0f;
    protected bool pushing;

    public AudioSource startChargingSound;
    public AudioSource stopChargingSound;

    protected SpriteRenderer spriteRenderer;

    [Header("lightning stuff for charging feedback")]
    [SerializeField]
    protected Material lightningMat, lightningMat2;
    protected bool charging;
    protected LineRenderer lr;
    [SerializeField]
    float lightningHeight;
    protected float lightningMatTimer;
    protected bool firstMat;


    // Start is called before the first frame update
    protected virtual void Awake()
    {
        //energyBar.setMaxValue(maxEnergy);
        //energyBar.setValue(energy);
        spriteRenderer = GetComponent<SpriteRenderer>();


        //lightning stuff
        charging = false;

        lr = gameObject.AddComponent(typeof(LineRenderer)) as LineRenderer;
        lr.positionCount = 2;
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, transform.position);
        lr.textureMode = LineTextureMode.Tile;
        lr.material = lightningMat;
        lightningMatTimer = 0.0f;
        firstMat = true;
    }

    public void setEnergyBar(ResourceBar bar)
    {
        energyBar = bar;
        energyBar.setMaxValue(maxEnergy);
        energyBar.setValue(energy);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("DrillRecharge"))
        {
            energyBar.glow(true);
            if (!startChargingSound.isPlaying) { startChargingSound.Play(); }
            StartCoroutine("Recharge");
            charging = true;
            lr.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("DrillRecharge"))
        {
            energyBar.glow(false);
            if (!stopChargingSound.isPlaying)
            {stopChargingSound.Play();}
            StopCoroutine("Recharge");
            charging = false;
            lr.enabled = false;
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

    public void Push(Vector3 origin)
    {
        pushDir = transform.position - origin;
        pushDir.z = 0;
        pushDir.Normalize();
        pushTimer = 0.0f;
        pushing = true;
    }

    public void stopPushing()
    {
        pushing = false;
    }

    protected void drawLightning()
    {
        lr.SetPosition(0, transform.position);
        Vector3 newPos = transform.position;
        newPos.y = lightningHeight;
        lr.SetPosition(1, newPos);
    }
}
