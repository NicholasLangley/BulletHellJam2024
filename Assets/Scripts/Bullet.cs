using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float _speed = 0;
    public Vector3 _direction;
    public bool _reflects;

    public int _damage = 1;

    [SerializeField]
    GameObject explosionPrefab;

    // Start is called before the first frame update
    void Start()
    {
        if (_speed == 0)
        {
            Init(Vector3.down, 1, false, 10);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = transform.position;
        newPos += _direction * _speed * Time.deltaTime;
        transform.position = newPos;
    }

    public void Init(Vector3 dir, float speed, bool doesReflect, int damage)
    {
        _speed = speed;
        _direction = dir;
        _reflects = doesReflect;
        _damage = damage;
    }

    public void reflectX()
    {
        _direction.x *= -1f;
    }

    public void reflectY()
    {
        _direction.y *= -1f;
    }

    void setDirection(Vector3 dir)
    {
        _direction = dir;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerBullet") && gameObject.CompareTag("Bullet"))
        {
            collision.gameObject.GetComponent<Bullet>().killBullet();
            killBullet();
        }
    }

    private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        if (collision.gameObject.CompareTag("yDeflect") && !gameObject.CompareTag("PlayerBullet"))
        {
            reflectY(); 
            makePlayerBullet();
        }

        else if (collision.gameObject.CompareTag("RedirectZone"))
        {
            Vector3 newDir = collision.gameObject.GetComponent<DeflectZone>().getTarget() - transform.position;
            newDir.z = 0;
            setDirection(newDir.normalized);
            makePlayerBullet();
        }
        else if (collision.gameObject.CompareTag("DestroyZone"))
        {
            killBullet();
        }
        else if (collision.gameObject.CompareTag("PongZone") && !gameObject.CompareTag("PlayerBullet"))
        {
            reflectY();
            Vector3 newDir = collision.gameObject.GetComponent<DeflectZone>().getTarget();
            newDir.z = 0;
            newDir.Normalize();
            newDir += _direction.normalized;
            setDirection(newDir.normalized);
            makePlayerBullet();
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            if (_reflects) { reflectX(); }
            else { killBullet(); }
        }

        else if (collision.gameObject.CompareTag("Drill"))
        {
            collision.gameObject.GetComponent<Drill>().Damage(_damage);
            killBullet();
        }

        else if (collision.gameObject.CompareTag("Monster") && gameObject.CompareTag("PlayerBullet"))
        {
            collision.gameObject.GetComponent<Monster>().Damage(_damage);
            killBullet();
        }
        else if (collision.gameObject.CompareTag("DestroyBulletsOnlyZone"))
        {
            killBullet();
        }


        else if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().Push(transform.position);
            killBullet();
        }
    }

    void makePlayerBullet()
    {
        //gameObject.GetComponent<Collider2D>().isTrigger = true;
        gameObject.tag = "PlayerBullet";
        gameObject.layer = LayerMask.NameToLayer("Default");
        gameObject.GetComponent<Rigidbody2D>().excludeLayers = LayerMask.GetMask("Nothing");
    }

    public void killBullet()
    {
        GameObject explosion = GameObject.Instantiate(explosionPrefab);
        Vector3 expTrans = explosion.transform.position;
        expTrans.x = transform.position.x;
        expTrans.y = transform.position.y;
        explosion.transform.position = expTrans; 
        GameObject.Destroy(gameObject);
    }
}
