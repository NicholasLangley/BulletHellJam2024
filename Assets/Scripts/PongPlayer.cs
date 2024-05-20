using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PongPlayer : Player
{

    [Header("Pong Player")]
    [Header("Speed and Positioning")]
    public float xSpeed = 0.1f;

    [SerializeField]
    float minX, maxX;


    [Header("Sprint Variables")]
    public float sprintSpeedMultiplier, sprintEnergyCost;

    protected override void Awake()
    {
        base.Awake();
        GetComponentInChildren<DeflectZone>().enableDeflect();
        energyBar.glow(true);
    }

    // Update is called once per frame
    void Update()
    {
        increaseEnergy(energyRechargeRate * Time.deltaTime);
        move();  
    }


    public void move()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal") * xSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space) && energy > sprintEnergyCost * Time.deltaTime)
        {
            moveHorizontal *= sprintSpeedMultiplier;
            decreaseEnergy(sprintEnergyCost * Time.deltaTime);
        }
        else
        {
            energyBar.notEnoughEnergy();
        }

        Vector3 newPos = transform.localPosition;
        newPos.x = Mathf.Clamp(newPos.x + moveHorizontal, minX, maxX);

        transform.localPosition = newPos;
    }
}
