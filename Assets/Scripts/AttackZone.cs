using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackZone : MonoBehaviour
{
    void Awake()
    {
        gameObject.GetComponent<Collider2D>().enabled = true;
    }

        public void enableAttack()
    {
        GetComponent<Collider2D>().enabled = true;
        gameObject.layer = LayerMask.NameToLayer("Default");
        GetComponent<SpriteRenderer>().enabled = true;
    }
    public void disableAttack()
    {
        //GetComponent<Collider2D>().enabled = false;
        gameObject.layer = LayerMask.NameToLayer("NoCollision");
        GetComponent<SpriteRenderer>().enabled = false;
    }
}
