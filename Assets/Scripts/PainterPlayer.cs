using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PainterPlayer : Player
{

    [Header("Painter Player")]
    [Header("Speed and Positioning")]
    public float xSpeed = 0.1f;
    public float ySpeed = 0.1f;

    [SerializeField]
    float minX, maxX, minY, maxY;

    //sprint variables
    [Header("Sprint Variables")]
    public float sprintSpeedMultiplier;
    public float sprintEnergyCost;


    [Header("Painting Variables")]
    public float paintCost;


    protected override void Awake()
    {
        base.Awake();
    }

    // Update is called once per frame
    void Update()
    {
        move();
    }


    public void move()
    {
        float moveVertical = Input.GetAxisRaw("Vertical") * ySpeed * Time.deltaTime;
        float moveHorizontal = Input.GetAxisRaw("Horizontal") * xSpeed * Time.deltaTime;

        if(Input.GetKey(KeyCode.Space) && energy > sprintEnergyCost * Time.deltaTime)
        {
            moveVertical *= sprintSpeedMultiplier;
            moveHorizontal *= sprintSpeedMultiplier;
            decreaseEnergy(sprintEnergyCost * Time.deltaTime);
        }

        Vector3 newPos = transform.localPosition;
        newPos.y = Mathf.Clamp(newPos.y + moveVertical, minY, maxY);
        newPos.x = Mathf.Clamp(newPos.x + moveHorizontal, minX, maxX);

        transform.localPosition = newPos;
    }
}