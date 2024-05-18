using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeflectZone : MonoBehaviour
{
    [SerializeField]
    public Vector3 target; 
    public bool pongZone = false;

    // Start is called before the first frame update
    void Awake()
    {
        gameObject.GetComponent<Collider2D>().enabled = true;
        if (target == null)
        {
            target = new Vector3(0, 0, 0);
        }
    }

    public void setTarget(Vector3 t)
    {
        target = t;
    }

    public Vector3 getTarget()
    {
        return target;
    }


    public void enableDeflect()
    {
        gameObject.layer = LayerMask.NameToLayer("Default");
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }
    public void disableDeflect()
    {
        //gameObject.GetComponent<Collider2D>().enabled = false;
        gameObject.layer = LayerMask.NameToLayer("NoCollision");
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }
}
