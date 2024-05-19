using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSpawner : MonoBehaviour
{

    public float drillHeight;
    public float startWidth, maxWidth;
    public int sections;
    public float  windupTime, growthTime, duration;
    public Explosion expPrefab;
    public float damage;

    float timer = 0;

    float sectionWidth;

    bool lasersCreated, firing;

    private void Awake()
    {
        sectionWidth = maxWidth / sections;
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            fireLaser();
        }

        if (firing)
        {
            timer += Time.deltaTime;
            if(timer > windupTime + growthTime)
            {
                Vector3 leftmost = transform.position + Vector3.left * maxWidth / 2.0f + Vector3.right * sectionWidth /2.0f;
                for (int i = 0; i < sections; i++)
                {
                    var child = new GameObject();
                    child.transform.SetParent(transform);
                    child.name = "laser child";
                    Vector3 start = leftmost + Vector3.right * sectionWidth * i;
                    Vector3 end = start;   end.y = drillHeight;
                    Laser laser = child.AddComponent<Laser>();
                    laser.Init(start, end, sectionWidth, sectionWidth, 0, 0, duration, expPrefab, damage);
                }
                firing = false;
            }
        }
    }


    public void fireLaser()
    {
        var child = new GameObject();
        child.transform.SetParent(transform);
        child.name = "initial laser child";
        Vector3 end = transform.position;
        end.y = drillHeight;
        Laser laser = child.AddComponent<Laser>();
        laser.Init(transform.position, end, startWidth, maxWidth, windupTime, growthTime, 0, expPrefab, 0);

        timer = 0.0f;
        firing = true;
    }

}
