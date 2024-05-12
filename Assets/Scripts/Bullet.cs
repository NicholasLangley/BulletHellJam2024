using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float speed = 0;
    public Vector3 direction;
    public bool reflects;

    public float damage = 1.0f;


    // Start is called before the first frame update
    void Start()
    {
        if (speed == 0)
        {
            Init(1, Vector3.down, false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = transform.position;
        newPos += direction * speed * Time.deltaTime;
        transform.position = newPos;
    }

    void Init(float setSpeed, Vector3 dir, bool doesReflect)
    {
        speed = setSpeed;
        direction = dir;
        reflects = doesReflect;
    }

    public void reflectX()
    {
        direction.x *= -1f;
    }

    public void reflectY()
    {
        direction.y *= -1f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Wall"))
        {
            if (reflects) { reflectX(); }
            else { GameObject.Destroy(gameObject); }
        }

        else if (collision.gameObject.CompareTag("Drill"))
        {
            collision.gameObject.GetComponent<Drill>().Damage(damage);
            GameObject.Destroy(gameObject);
        }
    }
    
}
