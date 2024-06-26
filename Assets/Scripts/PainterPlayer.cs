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
    float paintTimer;
    public float paintLinewidth;
    public float paintDuration;
    bool currentlyPainting;
    PaintLine currentLine;
    public float paintTickRate;

    [Header("Attacking Variables")]
    public float attackCost;
    bool attacking;

    [Header("playerSprites")]
    [SerializeField]
    Sprite defaultSprite;
    [SerializeField]
    Sprite PaintSprite, hitSprite, dashSprite, attackSprite;

    public AudioSource attackSound;
    public AudioSource paintSound;

    protected override void Awake()
    {
        base.Awake();
        currentlyPainting = false;
        stopAttacking();
        spriteRenderer.sprite = defaultSprite;
    }

    // Update is called once per frame
    void Update()
    {
        if (pushing)
        {
            stopPaint(); 
            stopAttacking();
            spriteRenderer.sprite = hitSprite;
            pushTimer += Time.deltaTime;
            if (pushTimer > pushDuration) { stopPushing(); spriteRenderer.sprite = defaultSprite; }
            else
            {
                Vector3 newPos = transform.localPosition + pushDir * pushStrength * Time.deltaTime;

                newPos.y = Mathf.Clamp(newPos.y, minY, maxY);
                newPos.x = Mathf.Clamp(newPos.x, minX, maxX);

                transform.localPosition = newPos;
                spriteRenderer.sprite = hitSprite;
            }
        }
        else
        {
            move();
            //painting stuff
            if (Input.GetKeyDown(KeyCode.Mouse0) && energy > paintCost * Time.deltaTime * 3)
            {
                stopAttacking();
                startPaint();
            }
            else if (Input.GetKeyUp(KeyCode.Mouse0) && currentlyPainting) { stopPaint(); }
            else if (currentlyPainting)
            {
                paintTimer += Time.deltaTime;
                if (paintTimer >= paintTickRate) { paint(); paintTimer = 0; }
            }
            //attacking stuff
            else if (Input.GetKey(KeyCode.Mouse1) && (energy > attackCost * Time.deltaTime))
            {
                startAttacking();
            }
            else if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                stopAttacking();
            }
            else if (attacking)
            {
                attack();
            }
        }

        if (charging)
        {
            drawLightning();
            lightningMatTimer += Time.deltaTime;
            if (lightningMatTimer > 0.25f)
            {
                lr.material = firstMat ? lightningMat2 : lightningMat;
                firstMat = !firstMat;
                lightningMatTimer = 0.0f;
            }
        }
    }


    public void move()
    {
        float moveVertical = Input.GetAxisRaw("Vertical") * ySpeed * Time.deltaTime;
        float moveHorizontal = Input.GetAxisRaw("Horizontal") * xSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space) && energy > sprintEnergyCost * Time.deltaTime)
        {
            moveVertical *= sprintSpeedMultiplier;
            moveHorizontal *= sprintSpeedMultiplier;
            decreaseEnergy(sprintEnergyCost * Time.deltaTime);
        }
        else { energyBar.notEnoughEnergy(); }

        Vector3 newPos = transform.localPosition;
        newPos.y = Mathf.Clamp(newPos.y + moveVertical, minY, maxY);
        newPos.x = Mathf.Clamp(newPos.x + moveHorizontal, minX, maxX);

        transform.localPosition = newPos;
    }

    void startPaint()
    {
        GameObject newLine = new GameObject();
        newLine.tag = "DestroyZone";
        newLine.name = "Painted Line";
        newLine.layer = LayerMask.NameToLayer("laser");
        currentLine = newLine.AddComponent(typeof(PaintLine)) as PaintLine;
        currentLine.setLinewidth(paintLinewidth);
        currentLine.CreateLine(transform.position, paintDuration, paintTickRate);
        currentlyPainting = true;
        paintTimer = 0;
        if (!attackSound.isPlaying) { paintSound.Play(); }
        spriteRenderer.sprite = PaintSprite;
        currentLine.setMat(lightningMat, lightningMat2);
    }

    void paint()
    {
        if(energy > paintCost * Time.deltaTime * (1.0f / paintTickRate))
        {
            currentLine.addPoint(transform.position);
            decreaseEnergy(paintCost * Time.deltaTime * (1.0f / paintTickRate));
        }
        else
        {
            stopPaint();
            energyBar.notEnoughEnergy();
        }
        
    }

    void stopPaint()
    {
        currentlyPainting = false;
        currentLine = null;
        paintSound.Stop();
        spriteRenderer.sprite = defaultSprite;
    }

    void startAttacking()
    {
        attacking = true;
        attack();
        GetComponentInChildren<AttackZone>().enableAttack();
        if (!attackSound.isPlaying) { attackSound.Play(); }
        spriteRenderer.sprite = attackSprite;
    }

    void attack()
    {
        if (energy < attackCost * Time.deltaTime) { stopAttacking(); energyBar.notEnoughEnergy(); return; }
        decreaseEnergy(attackCost * Time.deltaTime);
    }

    void stopAttacking()
    {
        attacking = false;
        GetComponentInChildren<AttackZone>().disableAttack();
        if (attackSound != null) {attackSound.Stop(); }
        spriteRenderer.sprite = defaultSprite;
    }
}