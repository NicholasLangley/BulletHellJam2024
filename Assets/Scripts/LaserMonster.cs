using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserMonster : MonoBehaviour
{

    public float fireRate, appearTime;
    float timer;
    bool activated, fired;
    float totalFireTime;
    public float minX, maxX;

    public float retractedY, extendedY;
    [SerializeField]
    Sprite closedMouth, openingMouth, openMouth, firingMouth1, firingMouth2;
    float fireAnimationTimer;
    bool firstAnimation;
    SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Awake()
    {
        activated = false;
        fired = false;
        timer = 0.0f;
        totalFireTime = GetComponent<LaserSpawner>().duration + GetComponent<LaserSpawner>().windupTime + GetComponent<LaserSpawner>().growthTime;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = closedMouth;
        fireAnimationTimer = 0.0f;
        firstAnimation = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (activated)
        {
            timer += Time.deltaTime;
            //Appearing
            if (timer > fireRate && timer < fireRate + appearTime)
            {
                //mouth animations
                if (timer - fireRate < 0.5f * appearTime) { spriteRenderer.sprite = openingMouth; }
                else if (timer - fireRate < 0.75f * appearTime) { spriteRenderer.sprite = openMouth; }
                ///////////////////////

                Vector3 newPos = transform.position;
                newPos.y = Mathf.Lerp(retractedY, extendedY, timer - fireRate / appearTime);
                transform.position = newPos;
            }
            //firing
            else if (timer > fireRate + appearTime && timer < fireRate + appearTime + totalFireTime)
            {
                //firing animation
                fireAnimationTimer += Time.deltaTime;
                if (fireAnimationTimer > 0.25f)
                {
                    spriteRenderer.sprite = firstAnimation ? firingMouth1 : firingMouth2;
                    firstAnimation = !firstAnimation;
                    fireAnimationTimer = 0.0f;
                }
                ///////
                if (fired == false)
                {
                    GetComponent<LaserSpawner>().fireLaser();
                    fired = true;
                }
            }
            //disappearing
            else if (timer > fireRate + appearTime + totalFireTime && timer <  fireRate + appearTime + totalFireTime + appearTime + 0.07f)
            {
                //mouth animations
                if (timer - fireRate -appearTime -totalFireTime < 0.2f * appearTime) { spriteRenderer.sprite = openMouth; }
                else if (timer - fireRate - appearTime - totalFireTime < 0.5f * appearTime) { spriteRenderer.sprite = openingMouth; }
                else { spriteRenderer.sprite = closedMouth; }
                ///////////////////////

                Vector3 newPos = transform.position;
                newPos.y = Mathf.Lerp(extendedY, retractedY, timer - fireRate - appearTime - totalFireTime - 0.05f / appearTime);
                transform.position = newPos;
            }
            //move
            else if(timer > fireRate + appearTime + totalFireTime + appearTime + 0.07f) 
            { Reposition();}
        }
    }

    public void Reposition()
    {
        Vector3 newPos = transform.position;
        newPos.x = Random.Range(minX, maxX);
        transform.position = newPos;
        timer = 0.0f;
        fired = false;
    }

    public void activate()
    {
        activated = true;
        timer = 0.0f;
    }

    public void deactivate()
    {
        activated = false;
        timer = 0.0f;
        Vector3 newPos = transform.position;
        newPos.y = retractedY;
        transform.position = newPos;
    }
}
