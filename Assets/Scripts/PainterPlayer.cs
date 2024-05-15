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


    protected override void Awake()
    {
        base.Awake();
        currentlyPainting = false;
    }

    // Update is called once per frame
    void Update()
    {
        move();
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            startPaint();
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0)) { stopPaint(); }
        else if (currentlyPainting)
        {
            paintTimer += Time.deltaTime;
            if (paintTimer >= 0.01f) { paint(); paintTimer = 0; }
        }
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

    void startPaint()
    {
        GameObject newLine = new GameObject();
        newLine.tag = "DestroyZone";
        newLine.name = "Painted Line";
        currentLine = newLine.AddComponent(typeof(PaintLine)) as PaintLine;
        currentLine.setLinewidth(paintLinewidth);
        currentLine.CreateLine(transform.position, paintDuration);
        currentlyPainting = true;
        paintTimer = 0;
    }

    void paint()
    {
        currentLine.addPoint(transform.position);
    }

    void stopPaint()
    {
        currentlyPainting = false;
        currentLine = null;
    }
}