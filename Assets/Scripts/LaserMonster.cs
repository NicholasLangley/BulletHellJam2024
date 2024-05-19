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
    // Start is called before the first frame update
    void Awake()
    {
        activated = false;
        fired = false;
        timer = 0.0f;
        totalFireTime = GetComponent<LaserSpawner>().duration + GetComponent<LaserSpawner>().windupTime + GetComponent<LaserSpawner>().growthTime;
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
                Vector3 newPos = transform.position;
                newPos.y = Mathf.Lerp(retractedY, extendedY, timer - fireRate / appearTime);
                transform.position = newPos;
            }
            //firing
            else if (timer > fireRate + appearTime && fired == false)
            {
                GetComponent<LaserSpawner>().fireLaser();
                fired = true;
            }
            //disappearing
            else if (timer > fireRate + appearTime + totalFireTime && timer <  fireRate + appearTime + totalFireTime + appearTime + 0.07f)
            {
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
}
