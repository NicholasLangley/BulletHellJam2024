using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordPlayer : Player
{

    [Header("Sword Player")]
    [Header("Speed and Positioning")]
    public float xSpeed = 0.1f;
    public float ySpeed = 0.1f;

    [SerializeField]
    float minX, maxX, minY, maxY;

    //dashing variables
    
    bool dashing;

    [Header("Dashing Variables")]
    public float dashTime;
    public float dashSpeed, dashEnergyCost;
    float dashTimer;
    Vector3 dashDir;


    protected override void Awake()
    {
        base.Awake();
        dashing = false;
        dashTimer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (dashing)
        {
            dash();
        }
        else if(Input.GetKeyDown(KeyCode.Space))
        {
            tryDash();
        }
        else 
        {
            move(); 
        }
        
    }


    public void move()
    {
        float moveVertical = Input.GetAxisRaw("Vertical") * xSpeed * Time.deltaTime;
        float moveHorizontal = Input.GetAxisRaw("Horizontal") * ySpeed * Time.deltaTime;

        Vector3 newPos = transform.localPosition;
        newPos.y = Mathf.Clamp(newPos.y + moveVertical, minY, maxY);
        newPos.x = Mathf.Clamp(newPos.x + moveHorizontal, minX, maxX);

        transform.localPosition = newPos;
    }

    public void dash()
    {
        Vector3 newPos = transform.localPosition + dashDir * dashSpeed * Time.deltaTime;
        newPos.y = Mathf.Clamp(newPos.y, minY, maxY);
        newPos.x = Mathf.Clamp(newPos.x, minX, maxX);

        transform.localPosition = newPos;

        dashTimer += Time.deltaTime;
        if (dashTimer >= dashTime) { dashing = false; }
    }

    public void tryDash()
    {
        if (energy >= dashEnergyCost)
        {
            dashing = true;
            dashTimer = 0.0f;
            //Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //mousePos.z = transform.position.z;
            //dashDir = mousePos - transform.position;
            dashDir = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0.0f);
            dashDir = dashDir.normalized;
            decreaseEnergy(dashEnergyCost);
        }
        else
        {
            Debug.Log("No Energy");
        }
    }
}
