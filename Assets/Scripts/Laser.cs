using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    //if sword player has attacked it
    public bool cut;
    public Vector3 start, end;

    LineRenderer lr;

    public float startWidth, maxWidth, windupTime, growthTime,  duration;
    float timer, expTimer;
    Explosion eplosionPrefab;
    float damage;

    Vector3 cutPos;

    public void Init(Vector3 s, Vector3 e, float initialWidth, float fullWidth, float windup, float growthT, float dur, Explosion eplosion, float dmg, Material mat)
    {

        start = s;
        end = e;
        startWidth = initialWidth;
        maxWidth = fullWidth;
        windupTime = windup;
        growthTime = growthT;
        duration = dur;
        eplosionPrefab = eplosion;

        lr = gameObject.AddComponent(typeof(LineRenderer)) as LineRenderer;
        lr.positionCount = 2;

        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        lr.startWidth = startWidth;
        lr.endWidth = startWidth;
        timer = 0.0f;
        expTimer = 0.0f;
        damage = dmg;
        lr.material = mat;
        cut = false;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (cut == true)
        {
            Vector3 endPos = Vector3.Lerp(cutPos, start, timer / 1.0f);
            lr.SetPosition(1, endPos);
            expTimer += Time.deltaTime;
            if (expTimer > 0.05f)
            {
                Explosion midExp = GameObject.Instantiate(eplosionPrefab);
                midExp.transform.position = lr.GetPosition(1);
                midExp.GetComponent<SpriteRenderer>().color = new Color(0.3f, 0.0f, 0.3f, 1.0f);

                Explosion leftExp = GameObject.Instantiate(eplosionPrefab);
                Vector3 left = lr.GetPosition(1);
                left.x -= lr.startWidth / 2.0f;
                leftExp.transform.position = left;
                leftExp.GetComponent<SpriteRenderer>().color = new Color(0.3f, 0.0f, 0.3f, 1.0f);

                Explosion rightExp = GameObject.Instantiate(eplosionPrefab);
                Vector3 right = lr.GetPosition(1);
                right.x += lr.startWidth / 2.0f;
                rightExp.transform.position = right;
                rightExp.GetComponent<SpriteRenderer>().color = new Color(0.3f, 0.0f, 0.3f, 1.0f);

                expTimer = 0.0f;
            }
            if (timer > 1.0f) { GameObject.Destroy(lr); GameObject.Destroy(this); }
        }
        else 
        {
            if (timer >= windupTime + duration + growthTime) { GameObject.Destroy(lr); GameObject.Destroy(this); }
            else if (timer >= windupTime + duration)
            {
                float newWidth = Mathf.Lerp(maxWidth, startWidth, (timer - windupTime - duration) / (growthTime));
                lr.startWidth = newWidth;
                lr.endWidth = newWidth;
            }
            else if (timer >= windupTime)
            {
                float newWidth = Mathf.Lerp(startWidth, maxWidth, (timer - windupTime) / (growthTime));
                lr.startWidth = newWidth;
                lr.endWidth = newWidth;

                expTimer += Time.deltaTime;
                if (expTimer > 0.2f)
                {
                    Explosion midExp = GameObject.Instantiate(eplosionPrefab);
                    midExp.transform.position = lr.GetPosition(1);
                    midExp.GetComponent<SpriteRenderer>().color = new Color(0.3f, 0.0f, 0.3f, 1.0f);

                    Explosion leftExp = GameObject.Instantiate(eplosionPrefab);
                    Vector3 left = lr.GetPosition(1);
                    left.x -= lr.startWidth / 2.0f;
                    leftExp.transform.position = left;
                    leftExp.GetComponent<SpriteRenderer>().color = new Color(0.3f, 0.0f, 0.3f, 1.0f);

                    Explosion rightExp = GameObject.Instantiate(eplosionPrefab);
                    Vector3 right = lr.GetPosition(1);
                    right.x += lr.startWidth / 2.0f;
                    rightExp.transform.position = right;
                    rightExp.GetComponent<SpriteRenderer>().color = new Color(0.3f, 0.0f, 0.3f, 1.0f);

                    expTimer = 0.0f;
                }

            }

            //raycast to check for collision
            Vector2 start2d = new Vector2();
            start2d.x = start.x;
            start2d.y = start.y;

            //Debug.Log(origin);
            RaycastHit2D hit = Physics2D.Raycast(start2d, Vector2.down, Mathf.Infinity, LayerMask.GetMask("laser"));
            if (hit != null)
            {
                if (hit.collider.CompareTag("RedirectZone") && hit.collider.gameObject.name == "SwordPlayerDeflectZone" && timer > windupTime + growthTime)
                {
                    cut = true;
                    timer = 0.0f;
                    Vector3 hitPos = hit.point;
                    hitPos.x = start.x;
                    hitPos.z = start.z;
                    lr.SetPosition(1, hitPos);
                    cutPos = hitPos;
                }
                else if (hit.collider.CompareTag("yDeflect") || hit.collider.CompareTag("RedirectZone") || hit.collider.CompareTag("DestroyZone") || hit.collider.CompareTag("DestroyBulletsOnlyZone") || hit.collider.CompareTag("laserKiller"))
                {
                    Vector3 hitPos = hit.point;
                    hitPos.x = start.x;
                    hitPos.z = start.z;
                    lr.SetPosition(1, hitPos);
                }
                else if (hit.collider.CompareTag("Drill"))
                {
                    if (timer > windupTime) { hit.collider.gameObject.GetComponent<Drill>().Damage(damage * Time.deltaTime); }
                    lr.SetPosition(1, end);
                }
            }
        }
    }
}
