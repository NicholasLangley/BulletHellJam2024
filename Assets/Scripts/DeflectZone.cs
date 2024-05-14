using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeflectZone : MonoBehaviour
{
    public Vector3 target; 

    // Start is called before the first frame update
    void Awake()
    {
        target = new Vector3(0,0,0);
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
        gameObject.GetComponent<Collider2D>().enabled = true;
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }
    public void disableDeflect()
    {
        gameObject.GetComponent<Collider2D>().enabled = false;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }
}
