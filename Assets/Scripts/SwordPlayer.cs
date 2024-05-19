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


    [Header("Redirect Variables")]
    public float redirectLength;
    public float redirectCooldown;
    public float slashCost;
    bool redirecting;

    protected override void Awake()
    {
        base.Awake();
        dashing = false;
        dashTimer = 0.0f;
        redirecting = false;
        GetComponentInChildren<DeflectZone>().disableDeflect();
        GetComponentInChildren<AttackZone>().disableAttack();
    }

    // Update is called once per frame
    void Update()
    {
        if (dashing) { pushing = false; }
        if (pushing)
        {
            if (dashing) { pushing = false; }
            pushTimer += Time.deltaTime;
            if (pushTimer > pushDuration) { stopPushing(); }
            else
            {
                Vector3 newPos = transform.localPosition + pushDir * pushStrength * Time.deltaTime;

                newPos.y = Mathf.Clamp(newPos.y, minY, maxY);
                newPos.x = Mathf.Clamp(newPos.x, minX, maxX);

                transform.localPosition = newPos;
            }
        }
        else
        {
            if (dashing)
            {
                dash();
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                tryDash();
            }
            else
            {
                move();
                if (Input.GetKeyDown(KeyCode.Mouse0) && !redirecting)
                {
                    Redirect();
                }
            }

            
        }
        
    }


    public void move()
    {
        float moveVertical = Input.GetAxisRaw("Vertical") * ySpeed * Time.deltaTime;
        float moveHorizontal = Input.GetAxisRaw("Horizontal") * xSpeed * Time.deltaTime;

        Vector3 newPos = transform.localPosition;
        newPos.y = Mathf.Clamp(newPos.y + moveVertical, minY, maxY);
        newPos.x = Mathf.Clamp(newPos.x + moveHorizontal, minX, maxX);

        transform.localPosition = newPos;
    }

    //TODO: damage/deflect on dash?
    public void dash()
    {
        Vector3 newPos = transform.localPosition + dashDir * dashSpeed * Time.deltaTime;
        newPos.y = Mathf.Clamp(newPos.y, minY, maxY);
        newPos.x = Mathf.Clamp(newPos.x, minX, maxX);

        transform.localPosition = newPos;

        dashTimer += Time.deltaTime;
        if (dashTimer >= dashTime) { dashing = false; GetComponentInChildren<AttackZone>().disableAttack(); }
    }

    public void tryDash()
    {
        if (energy >= dashEnergyCost)
        {
            dashing = true;
            dashTimer = 0.0f;
            dashDir = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);
            dashDir = dashDir.normalized;
            decreaseEnergy(dashEnergyCost);
            GetComponentInChildren<AttackZone>().enableAttack();
        }
        else
        {
            energyBar.notEnoughEnergy();
        }
    }

    public void Redirect()
    {
        if (energy >= slashCost)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = transform.position.z;
            GetComponentInChildren<DeflectZone>().setTarget(mousePos);

            GetComponentInChildren<DeflectZone>().enableDeflect();
            redirecting = true;
            Invoke("stopRedirecting", redirectLength);
            decreaseEnergy(slashCost);
        }
        else
        {
            //no energy feedback
            energyBar.notEnoughEnergy();
        }

    }

    void stopRedirecting()
    {
        GetComponentInChildren<DeflectZone>().disableDeflect();
        Invoke("finishRedirectCooldown", redirectCooldown);
    }

    void finishRedirectCooldown()
    {
        redirecting = false;
    }
}
